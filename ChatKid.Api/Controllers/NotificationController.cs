using ChatKid.Notification.Notification;
using ChatKid.Notification.Notification.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatKid.Api.Controllers
{

    [Route("api/notification")]
    [ApiController]
    [AllowAnonymous]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        
        [HttpPost("push")]
        public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
        {
            try
            {
                var result = await _notificationService.PushNotification(notificationModel);

                if (result.IsSuccess)
                {
                    // Return 200 OK for success
                    return Ok(result);
                }
                else
                {
                    // Return 500 Internal Server Error for failures
                    return StatusCode(400, result);
                }
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with a generic error message
                return StatusCode(500, new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Internal Server Error"
                });
            }
        }

    }
}
