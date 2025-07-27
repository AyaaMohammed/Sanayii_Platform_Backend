using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sanayii.Core.Entities;
using Sanayii.Core.Repositories;
using Sanayii.Repository.Data;
using Sanayii.Repository;
using Sanayii.Enums;
using Microsoft.Extensions.Logging;
using Sanayii.Repository.Repository;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebhook : ControllerBase
    {
        private readonly string _webhookSecret;
        private readonly UnitOFWork _unitOfWork;
        private readonly ILogger<StripeWebhook> _logger;

        public StripeWebhook(IConfiguration config, UnitOFWork unitOfWork, ILogger<StripeWebhook> logger)
        {
            _webhookSecret = config["Stripe:WebhookSecret"];
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhookAsync()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                // When a payment is successfully completed in Stripe, Stripe sends a notification to the application via the Webhook.
                // The application receives this notification, processes the data, and then updates the payment status in the database.

                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret
                );

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    if (paymentIntent != null)
                    {
                        if (int.TryParse(paymentIntent.Metadata["PaymentId"], out int paymentId))
                        {
                            var payment = _unitOfWork._PaymentRepo.GetById(paymentId);
                            if (payment != null)
                            {
                                payment.Status = PaymentStatus.Completed;
                                _unitOfWork.save();
                                _logger.LogInformation($"Payment {paymentId} succeeded.");
                            }
                            else
                            {
                                _logger.LogWarning($"Payment with ID {paymentId} not found.");
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Invalid PaymentId in metadata.");
                            return BadRequest("Invalid PaymentId in metadata.");
                        }
                    }
                }
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    if (paymentIntent != null)
                    {
                        if (int.TryParse(paymentIntent.Metadata["PaymentId"], out int paymentId))
                        {
                            var payment = _unitOfWork._PaymentRepo.GetById(paymentId);
                            if (payment != null)
                            {
                                payment.Status = PaymentStatus.Failed;
                                _unitOfWork.save();
                                _logger.LogInformation($"Payment {paymentId} failed.");
                            }
                            else
                            {
                                _logger.LogWarning($"Payment with ID {paymentId} not found.");
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Invalid PaymentId in metadata.");
                            return BadRequest("Invalid PaymentId in metadata.");
                        }
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing webhook: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}