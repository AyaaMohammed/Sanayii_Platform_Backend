using Microsoft.AspNetCore.Mvc;
using Sanayii.Repository.Repository;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly UnitOFWork _unitOfWork;
        public ServiceController(UnitOFWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("{id}")]
        public IActionResult get(int id)
        {
            var service = _unitOfWork._ServiceRepo.GetById(id);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }
    }
}
