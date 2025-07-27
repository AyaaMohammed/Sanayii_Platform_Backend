using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sanayii.APIs.Controllers;
using Sanayii.Core.Entities;
using Sanayii.Repository;
using Sanayii.Repository.Data;
using Sanayii.Repository.Repository;
using Sanayii.Service.Hubs;


[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<ChatController> _logger;
    private readonly UnitOFWork unitOFWork;
   
    public NotificationController(IHubContext<NotificationHub> hubContext, ILogger<ChatController> _logger,SanayiiContext DB, UnitOFWork unitOFWork)
    {
        _hubContext = hubContext;
        this._logger = _logger;
        this.unitOFWork = unitOFWork;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var n = unitOFWork._NotificationRepo.GetAll();
        return Ok(n);
    }
    [HttpGet("{customerid}")]
    public IActionResult Index(string customerid)
    {
        var n = unitOFWork._NotificationRepo.getCustomerNotification(customerid);
        return Ok(n);
    }
    [HttpGet("MarkAsRead/{customerid}")]
    public IActionResult MarkasRead(string customerid)
    {
        unitOFWork._NotificationRepo.MarkAsRead(customerid);
        unitOFWork.save();
        return Ok(new { message = "All Notifications Marked as Read successfully" });
    }
    [HttpGet("MarkNotification")]
    public IActionResult MarkNotification(string customerid,int id)
    {
        var notfication = unitOFWork._NotificationRepo.GetById(id);
        if (notfication.Id != id)
            return BadRequest();

        unitOFWork._NotificationRepo.MarkNotification(id);
        unitOFWork.save();
        return Ok(new { message = "Notification Marked as Read successfully" });
    }
    [HttpDelete("DeleteCustomerNotification/{customerid}")]
    public IActionResult DeleteCustomerNotification(string customerid)
    {
        unitOFWork._NotificationRepo.DeleteCustomerNotification(customerid);
        unitOFWork.save();

        return Ok(new { message = "All Notifications Deleted successfully" });
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var notfication = unitOFWork._NotificationRepo.GetById(id);
        if (notfication == null)
            return BadRequest();

        unitOFWork._NotificationRepo.Delete(id);
        unitOFWork.save();
        return Ok(new { message = "Notification Deleted successfully" });
    }
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] Notification notification)
    {
        if (notification == null)
        {
            return BadRequest("Notification data is required.");
        }

        // Broadcast the notification to client
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);

        // Log the notification action
        _logger.LogInformation($"Notification sent to CustomerId={notification.UserId}: Your service request status has been updated to: {notification.Content}");

        return Ok();
    }
}
