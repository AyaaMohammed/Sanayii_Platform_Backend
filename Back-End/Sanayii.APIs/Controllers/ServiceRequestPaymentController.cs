using Microsoft.AspNetCore.Mvc;
using Sanayii.Core.Entities;
using Sanayii.Enums;
using Sanayii.Repository.Repository;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestPaymentController : Controller
    {
        private readonly UnitOFWork _unitOfWork;
        public ServiceRequestPaymentController(UnitOFWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: api/ServiceRequestPayments
     [HttpGet]
     public async Task<ActionResult<IEnumerable<ServiceRequestPayment>>> GetAll()
     {
         var payments = await _unitOfWork.ServiceRequestPaymentRepos.GetAllAsync();
         return Ok(payments);
     }

     // GET: api/ServiceRequestPayments/by-customer/{customerId}
     [HttpGet("by-customer/{customerId}")]
     public async Task<ActionResult<IEnumerable<ServiceRequestPayment>>> GetByCustomer(string customerId)
     {
         var payments = await _unitOfWork.ServiceRequestPaymentRepos.GetByCustomerAsync(customerId);
         return Ok(payments);
     }

     // GET: api/ServiceRequestPayments/by-artisan/{artisanId}
     [HttpGet("by-artisan/{artisanId}")]
     public async Task<ActionResult<IEnumerable<ServiceRequestPayment>>> GetByArtisan(string artisanId)
     {
         var payments = await _unitOfWork.ServiceRequestPaymentRepos.GetByArtisanAsync(artisanId);
         return Ok(payments);
     }

     // GET: api/ServiceRequestPayments/by-service/{serviceId}
     [HttpGet("by-service/{serviceId}")]
     public async Task<ActionResult<IEnumerable<ServiceRequestPayment>>> GetByService(int serviceId)
     {
         var payments = await _unitOfWork.ServiceRequestPaymentRepos.GetByServiceAsync(serviceId);
         return Ok(payments);
     }
        

        // GET: api/ServiceRequestPayments/by-status/{status}
        [HttpGet("by-status/{status}")]
     public async Task<ActionResult<IEnumerable<ServiceRequestPayment>>> GetByStatus(ServiceStatus status)
     {
         var payments = await _unitOfWork.ServiceRequestPaymentRepos.GetByStatusAsync(status);
         return Ok(payments);
     }

     // GET: api/ServiceRequestPayments/{customerId}/{paymentId}/{serviceId}
     [HttpGet("{customerId}/{paymentId}/{serviceId}")]
     public async Task<ActionResult<ServiceRequestPayment>> GetByIds(string customerId, int paymentId, int serviceId)
     {
         var payment = await _unitOfWork._ServiceRequestPaymentRepo.GetByIdsAsync(customerId, paymentId, serviceId);

         if (payment == null)
         {
             return NotFound();
         }

         return Ok(payment);
     }

     // POST: api/ServiceRequestPayments
     [HttpPost]
     public async Task<ActionResult<ServiceRequestPayment>> Create(ServiceRequestPayment payment)
     {
         await _unitOfWork._ServiceRequestPaymentRepo.AddAsync(payment);
         await _unitOfWork.SaveAsync(); // Save changes via Unit of Work

         return CreatedAtAction(
             nameof(GetByIds),
             new { customerId = payment.CustomerId, paymentId = payment.PaymentId, serviceId = payment.ServiceId },
             payment);
     }

     // PUT: api/ServiceRequestPayments
     [HttpPut]
     public async Task<IActionResult> Update(ServiceRequestPayment payment)
     {
         var existingPayment = await _unitOfWork._ServiceRequestPaymentRepo.GetByIdsAsync(payment.CustomerId, payment.PaymentId, payment.ServiceId);
         if (existingPayment == null)
         {
             return NotFound();
         }

         _unitOfWork._ServiceRequestPaymentRepo.Update(payment);
         await _unitOfWork.SaveAsync();

         return NoContent();
     }

     // DELETE: api/ServiceRequestPayments/{customerId}/{paymentId}/{serviceId}
     [HttpDelete("{customerId}/{paymentId}/{serviceId}")]
     public async Task<IActionResult> Delete(string customerId, int paymentId, int serviceId)
     {
         var payment = await _unitOfWork._ServiceRequestPaymentRepo.GetByIdsAsync(customerId, paymentId, serviceId);
         if (payment == null)
         {
             return NotFound();
         }

         _unitOfWork._ServiceRequestPaymentRepo.Delete(payment);
         await _unitOfWork.SaveAsync();

         return NoContent();
     }
    }
}
