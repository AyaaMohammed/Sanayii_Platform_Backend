using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sanayii.Core.DTOs.ArtisanDTOS;
using Sanayii.Core.DTOs.CustomerDTOs;
using Sanayii.Core.Entities;
using Sanayii.Enums;
using Sanayii.Repository.Data;
using Sanayii.Repository.Repository;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly UnitOFWork _unitOfWork;
        private readonly SanayiiContext _dbContext;
        public CustomerController(UnitOFWork unitOFWork, SanayiiContext dbContext) {
            _dbContext = dbContext;
            _unitOfWork = unitOFWork;
        }
        //[HttpGet("{customerid}")]
        //public IActionResult Get(string customerid)
        //{
        //    var customer = _unitOfWork._CustomerRepo.GetById(customerid);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(customer);
        //}
        [HttpGet("{customerid}")]
        public IActionResult Get(string customerid)
        {
            var customer = _unitOfWork._CustomerRepo.GetById(customerid);


            if (customer == null)
            {
                return NotFound();
            }
            Console.WriteLine($"UserPhones is null: {customer.UserPhones == null}");
            Console.WriteLine($"UserPhones count: {customer.UserPhones?.Count ?? 0}");
            if (customer.UserPhones != null)
            {
                foreach (var phone in customer.UserPhones)
                {
                    Console.WriteLine($"Phone: {phone.PhoneNumber}");
                }
            }

            var customerDto = new CustomerDTO
            {
                Id = customer.Id,
                FName = customer.FName,
                LName = customer.LName,
                userName = customer.UserName,
                Age = customer.Age,
                City = customer.City,
                Street = customer.Street,
                Government = customer.Government,
                Email = customer.Email,
                
            };
            var phones = _dbContext.UserPhones.Where(p => p.UserId == customerid).Select(s => s.PhoneNumber).ToList();
            customerDto.phoneNumber = phones;

            return Ok(customerDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(string id, EditCustomerDTO dTO)
        {
            try
            {
                if (id != dTO.Id)
                {
                    return BadRequest("Customer ID mismatch.");
                }
                var existingCustomer = _unitOfWork._CustomerRepo.GetCustomerById(id);
                if (existingCustomer == null)
                {
                    return NotFound("Customer not found.");
                }
                existingCustomer.FName = dTO.FName;
                existingCustomer.LName = dTO.LName;
                existingCustomer.Age = dTO.Age;
                existingCustomer.City = dTO.City;
                existingCustomer.Street = dTO.Street;
                existingCustomer.Government = dTO.Government;
                existingCustomer.Email = dTO.Email;
                //existingCustomer.UserPhones = dTO.PhoneNumber
                //    .Select(phone => new UserPhones { PhoneNumber = phone })
                //    .ToList();
                existingCustomer.UserPhones?.Clear();

                // Add new phone numbers
                if (dTO.PhoneNumber != null)
                {
                    foreach (var phone in dTO.PhoneNumber)
                    {
                        existingCustomer.UserPhones.Add(new UserPhones
                        {
                            PhoneNumber = phone,
                            UserId = existingCustomer.Id
                        });
                    }
                }


                _unitOfWork._CustomerRepo.Edit(existingCustomer);
                _unitOfWork._CustomerRepo.SaveChanges();
                return Ok("Customer updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }
        [HttpGet("getServiceRequest/{id}")]
        public async Task<IActionResult> GetCustomerServiceRequests(string id)
        {
            var allServicesRequests = await _dbContext.ServiceRequestPayments
                .Where(s => s.CustomerId == id)
                .Include(s => s.Service)
                .Include(s => s.Artisan)
                .Include(s => s.Payment) // ✅ include Payment
                .ToListAsync();

            if (allServicesRequests == null || !allServicesRequests.Any())
            {
                return NotFound("No service requests found for this customer.");
            }

            var allServicesDtos = allServicesRequests.Select(s => new CustomerServiceRequestDto
            {
                CreatedAt = s.CreatedAt,
                ExecutionTime = s.ExecutionTime,
                Status = (int)s.Status,
                PaymentMethod = s.Payment != null ? (int)s.Payment.Method : 0,       // default 0 = Unknown
                PaymentAmount = s.Payment?.Amount ?? 0,
                ServiceName = s.Service?.ServiceName ?? "Unknown",
                ArtisanName = s.Artisan != null ? $"{s.Artisan.FName} {s.Artisan.LName}" : "Not assigned",

                artisanId = s.ArtisanId,
                ServiceId = s.ServiceId
            }).ToList();

            return Ok(allServicesDtos);
        }



    }
}
