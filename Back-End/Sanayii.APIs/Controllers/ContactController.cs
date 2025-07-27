using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sanayii.Core.DTOs.ContactFormDTO;
using Sanayii.Services;

namespace Sanayii.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly EmailSenderService _emailService;
        private readonly IConfiguration _config;
        public ContactController(EmailSenderService emailService, IConfiguration config)
        {
            _emailService = emailService;
            _config = config;
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] ContactFormDto dto)
        {
            var toEmail = _config["EmailSettings:FromEmail"]; 
            var subject = "New Contact Message from Website";
            var body = $@"
            <b>Name:</b> {dto.FirstName} {dto.LastName}<br/>
            <b>Email:</b> {dto.Email}<br/>
            <b>Message:</b><br/>{dto.Message}";

            await _emailService.SendEmailAsync(toEmail, subject, body, true);

            return Ok(new { success = true, message = "Message sent" });
        }
    }
}
