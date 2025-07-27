using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sanayii.Core.DTOs;
using Sanayii.Core.Entities;
using Sanayii.Repository.Repository;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly CommentService commentService;
        private readonly UnitOFWork unitOfWork;
        IMapper _mapper;
        public ReviewController(CommentService commentService,UnitOFWork unitOFWork, IMapper mapper)
        {
            this.commentService = commentService;
            this.unitOfWork = unitOFWork;
            _mapper = mapper;
        }
        [HttpPost("add-review")]
        public async Task<IActionResult> AddReview(ReviewDTO review)
        {
            var reviewEntity = _mapper.Map<Review>(review);
            // Check if the review violates any rules
            var isViolate = await isViolation(review.Comment);
            if (isViolate)
            {
                reviewEntity.isViolate = true;
            }
            
            unitOfWork._ReviewRepo.Add(reviewEntity);
            unitOfWork.save();
            return Ok("Review added successfully.");
        }
        private async Task<bool> isViolation(string comment)
        {
            // Check if the comment contains any violation words
            var containsProfanity = await commentService.FilterProfanityAsync(comment);
            if (containsProfanity == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
