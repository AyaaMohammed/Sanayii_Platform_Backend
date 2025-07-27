using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System;
using Sanayii.Core.Entities;
using Sanayii.Core.Repositories;
using Sanayii.Repository.Data;
using static System.Collections.Specialized.BitVector32;
using Sanayii.Repository;
using Sanayii.Enums;
using Sanayii.Core.DTOs.PaymentDTOs;
using Microsoft.EntityFrameworkCore;
using Sanayii.Repository.Repository;


namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly UnitOFWork _unitOfWork;
        private readonly string _publishableKey;
        public PaymentsController(IConfiguration config, UnitOFWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _publishableKey = config["Stripe:PublishableKey"];
        }

        [HttpPost("create-session")]
        public async Task<ActionResult> CreateCheckoutSession([FromBody] CreateSessionRequest req)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(req.Amount * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions { Name = req.ProductName }
                },
                Quantity = 1
            }
            },
                Mode = "payment",
                SuccessUrl = req.SuccessUrl,
                CancelUrl = req.CancelUrl
            };
            var service = new SessionService();
            Session session = service.Create(options);
            var payment = _unitOfWork._PaymentRepo.GetById(req.PaymentId);
            if (payment == null)
                return NotFound("Payment  not found");
            payment.Status = PaymentStatus.Completed;
            payment.Amount = req.Amount;
            return Ok(new { sessionId = session.Id, publishableKey = _publishableKey, paymentId = req.PaymentId });
        }
        [HttpGet("getPrice/{id}")]
        public ActionResult GetPrice(int id)
        {
            var service = _unitOfWork._ServiceRepo.GetById(id);
            if (service == null)
                return NotFound("Service not found");
            var price = service.BasePrice + service.AdditionalPrice;
            return Ok(price);
        }
    }

}