using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sanayii.Core.Entities;
using Sanayii.Core.Repositories;
using Sanayii.Repository.Repository;
using Sanayii.Repository.Data;
using Sanayii.Repository.Repository;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly UnitOFWork _unitOfWork;

        public CategoriesController(UnitOFWork unitOFWork)
        {
            _unitOfWork = unitOFWork;
        }

        // GET: api/Categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories =  _unitOfWork._CategoryRepo.GetAll();
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id:int}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _unitOfWork._CategoryRepo.GetById(id);

            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        //// GET : api/Categories/{name}
        [HttpGet("{name}")]
        public IActionResult GetCategoryByName(string name)
        {
            var category = _unitOfWork._CategoryRepo.getByName(name);

            if (category is null)
                return NotFound();
            else
                return Ok(category);
        }
    }
}
