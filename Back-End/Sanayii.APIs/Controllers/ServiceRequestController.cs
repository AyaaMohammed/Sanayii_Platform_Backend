using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sanayii.Repository.Repository;
using Sanayii.Core.Entities;
using Sanayii.Enums;
using Twilio.TwiML.Voice;
using Stripe;
using Sanayii.Repository.Data;
using Sanayii.Core.DTOs.PaymentDTOs;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private UnitOFWork _unitOfWork;
        private SanayiiContext db;
        public ServiceRequestController(UnitOFWork unitOFWork, SanayiiContext sanayiiContext)
        {
            db = sanayiiContext;
            _unitOfWork = unitOFWork;
        }
        [HttpPost]
        public IActionResult CreateServiceRequest([FromBody] SeriveRequestViewModel viewModel)
        {
            // Validate the request
            var Categ = _unitOfWork._CategoryRepo.GetById(viewModel.CategoryId);
            if (Categ == null)
            {
                return BadRequest("Service not found");
            }
            var customer = _unitOfWork._CustomerRepo.GetAllCustomers()
                           .Include(c => c.UserPhones)
                           .FirstOrDefault(c => c.Id == viewModel.CustomerId);
            if (customer == null)
            {
                return Unauthorized();
            }
            // Create the service request
            Sanayii.Core.Entities.Service service = new Sanayii.Core.Entities.Service
            {
                ServiceName = viewModel.ServiceName,
                Description = viewModel.Description,
                BasePrice = 0,
                AdditionalPrice = 0,
                CategoryId = viewModel.CategoryId
            };
            _unitOfWork._ServiceRepo.Add(service);
            // Create the payment 
            Payment payment = new Payment
            {
                Status = PaymentStatus.Pending,
                Amount = 0,
                Method = (PaymentMethods)viewModel.PaymentMethod
            };
            _unitOfWork._PaymentRepo.Add(payment);
            _unitOfWork.save();
            // Create the service request payment
            ServiceRequestPayment serviceRequestPayment = new ServiceRequestPayment
            {
                CustomerId = viewModel.CustomerId,
                ServiceId = service.Id,
                ArtisanId = null,
                PaymentId = payment.Id,
                CreatedAt = viewModel.RequestDate,
                ExecutionTime = 0,
                Status = (ServiceStatus)viewModel.Status,
            };
            db.ServiceRequestPayments.Add(serviceRequestPayment);
            _unitOfWork.save();
            // Assign ArtisanId to the service request payment
            Artisan artisan = GetAvailableArtisan(viewModel.CategoryId, customer.Government);
            if (artisan != null)
            {
                serviceRequestPayment.ArtisanId = artisan.Id;
                db.ServiceRequestPayments.Update(serviceRequestPayment);
                _unitOfWork.save();
            }
            else
            {
                return NotFound("No artisans available for this service.");
            }
            // Return Data to show the service request payment details
            SeriveRequestDto requestDto = new SeriveRequestDto
            {
                CategoryId = Categ.Id,
                CategoryName = Categ.Name,
                ServiceId = service.Id,
                ServiceName = service.ServiceName,
                Description = service.Description,
                RequestDate = serviceRequestPayment.CreatedAt,
                PaymentId = payment.Id,
                PaymentMethod = (int)payment.Method,
                CustomerName = customer.UserName,
                CustomerCity = customer.City,
                CustomerGovernment = customer.Government,
                CustomerStreet = customer.Street,
                CustomerPhoneNumbers = customer?.UserPhones?.Select(up => up.PhoneNumber).ToList() ?? new List<string>(),
                artisanId = artisan.Id,
                ArtisanName = artisan.UserName
            };
            return Ok(requestDto);
        }
        private Artisan GetAvailableArtisan(int categoryId, string governmentCustomer)
        {
            // Retrieve available artisans based on category and government

            var artisans = db.Artisans.Where(x=> x.CategoryId == categoryId && x.Government == governmentCustomer).ToList();
            //var artisans = db.Artisans.ToList();
            // If no artisans are found, return null
            if (!artisans.Any())
            {
                return null;
            }

            // Retrieve artisan ids who are assigned to a pending or in-progress service request
            var busyArtisanIds = db.ServiceRequestPayments
                .Where(s => s.ArtisanId != null && (s.Status == ServiceStatus.Service_Requested || s.Status== ServiceStatus.Awaiting_Approval  || s.Status == ServiceStatus.Artisan_Busy|| s.Status == ServiceStatus.In_Progress))
                .Select(s => s.ArtisanId ?? string.Empty)
                .Where(id => !string.IsNullOrEmpty(id))
                .Distinct()
                .ToList();

            // Filter out busy artisans, ensure to convert a.Id to string for comparison
            var availableArtisans = artisans.Where(a => !busyArtisanIds.Contains(a.Id.ToString())).ToList();

            // If no available artisans, return null
            if (!availableArtisans.Any())
            {
                return null;
            }

            // Return the artisan with the highest rating
            return availableArtisans.OrderByDescending(a => a.Rating).FirstOrDefault();

        }

        [HttpDelete]
        public IActionResult cancel(string? CustomerId, string artisanId, int serviceId, int paymentId)
        {
            _unitOfWork._ServiceRequestPaymentRepo.cancelRequest(CustomerId, artisanId, serviceId, paymentId);
            return Ok();
        }
    }


}
