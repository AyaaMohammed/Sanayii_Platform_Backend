using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sanayii.Core.DTOs.ChatDTOs;
using Sanayii.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sanayii.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(
            IChatService chatService,
            ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] ChatRequestDTO request)
        {
            try
            {
                _logger.LogInformation("Received chat request");

                if (request == null || string.IsNullOrWhiteSpace(request.Message))
                {
                    _logger.LogWarning("Invalid request received");
                    return BadRequest(new
                    {
                        Status = "Error",
                        Message = "Please provide a valid message"
                    });
                }

                _logger.LogDebug("Processing message: {Message}", request.Message);
                var response = await _chatService.SendMessageAsync(request);

                return Ok(new
                {
                    Status = "Success",
                    Response = response.Response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chat request");

                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = ex.Message,
                    Suggestion = "Please try again later or contact support"
                });
            }
        }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                Status = "Healthy",
                Service = "Chat API",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}