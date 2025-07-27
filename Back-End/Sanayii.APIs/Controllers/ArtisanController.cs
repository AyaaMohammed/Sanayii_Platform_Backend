using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sanayii.Repository.Data;
using Sanayii.Repository.Repository;
using Sanayii.Core.DTOs.ArtisanDTOS;
using Sanayii.Core.Entities;
using Sanayii.Enums;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtisanController : ControllerBase
    {
        private readonly UnitOFWork _unitOfWork;
        private readonly SanayiiContext _dbContext;
        public ArtisanController(UnitOFWork unitOfWork, SanayiiContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        [HttpGet("getServiceRequest/{id}")]
        public async Task<IActionResult> GetArtisanService(string id)
        {

            var allServicesRequests = await _dbContext.ServiceRequestPayments
                .Where(s => s.ArtisanId == id)
                .Include(s => s.Service)
                .Include(s => s.Customer)
                .ToListAsync();

            if (allServicesRequests == null)
            {
                return NotFound("No service requests found for this artisan.");
            }

            var allServicesDtos = new List<ArtisanServiceRequestDto>();

            for (int i = 0; i < allServicesRequests.Count; i++)
            {
                var dto = new ArtisanServiceRequestDto
                {
                    CreatedAt = allServicesRequests[i].CreatedAt,
                    ExecutionTime = allServicesRequests[i].ExecutionTime,
                    Status = ((ServiceStatus)allServicesRequests[i].Status).ToString(),
                    ServiceName = allServicesRequests[i].Service?.ServiceName,
                    CustomerName = allServicesRequests[i].Customer?.FName + " " + allServicesRequests[i].Customer?.LName,
                };

                var rate = _dbContext.Review.Where(s => s.ServiceId == allServicesRequests[i].ServiceId &&
                s.CustomerId == allServicesRequests[i].CustomerId && s.ArtisanId == id).Select(s => s.Rating).FirstOrDefault();

                dto.rating = rate;
                allServicesDtos.Add(dto);
            }

            return Ok(allServicesDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtisanById(string id)
        {
            var artisan = await _dbContext.Artisans
        .Include(a => a.UserPhones)
        .FirstOrDefaultAsync(a => a.Id == id);


            if (artisan == null)
                return NotFound();

            var dto = new ArtisanDTO
            {
                Id = artisan.Id,
                Rating = artisan.Rating,
                FName = artisan.FName,
                LName = artisan.LName,
                City = artisan.City,
                Street = artisan.Street,
                Government = artisan.Government,
                UserName = artisan.UserName,
                Email = artisan.Email
            };
            var phones = _dbContext.UserPhones.Where(p => p.UserId == id).Select(s => s.PhoneNumber).ToList();
            dto.UserPhones = phones;

            return Ok(dto);
        }
       

    }
}
