using EventLogistics.Application.Services;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationHistoryRepository _historyRepository;
        private readonly NotificationService _notificationService;

        public NotificacionController(
            INotificationRepository notificationRepository,
            INotificationHistoryRepository historyRepository,
            NotificationService notificationService)
        {
            _notificationRepository = notificationRepository;
            _historyRepository = historyRepository;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAll()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return Ok(notifications);
        }        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetById(Guid id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }[HttpGet("history/{notificationId}")]
        public async Task<ActionResult<IEnumerable<NotificationHistory>>> GetHistory(Guid notificationId)
        {
            var history = await _historyRepository.GetByNotificationIdAsync(notificationId);
            return Ok(history);
        }

        [HttpPost]
        public async Task<ActionResult<Notification>> Create(Notification notification)
        {
            notification.CreatedBy = notification.CreatedBy ?? "System";
            notification.UpdatedBy = notification.UpdatedBy ?? "System";
            notification.Timestamp = DateTime.UtcNow;
            notification.Status = notification.Status ?? "Pending";

            var result = await _notificationService.SendNotification(notification);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }        [HttpPost("confirm/{id}")]
        public async Task<IActionResult> ConfirmNotification(Guid id)
        {
            var success = await _notificationService.ConfirmNotification(id);
            if (success)
            {
                return Ok(new { message = "Notification confirmed successfully" });
            }
            return NotFound(new { message = "Notification not found" });
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var metrics = await _notificationService.CalculateMetrics(startDate, endDate);
            return Ok(metrics);
        }        [HttpGet("pending/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetPendingNotifications(Guid userId)
        {
            var notifications = await _notificationRepository.GetByRecipientIdAsync(userId);
            var pendingNotifications = notifications.Where(n => n.Status == "Pending" || n.Status == "Sent").ToList();
            return Ok(pendingNotifications);
        }        

        [HttpGet("assignment/{assignmentId}")]
        public async Task<IActionResult> GetAssignmentNotifications(Guid assignmentId)
        {
            var histories = await _notificationService.GetNotificationHistoryForAssignment(assignmentId);
            return Ok(histories);
        }
        
        [HttpPost("resend/{notificationId}")]
        public async Task<IActionResult> ResendNotification(Guid notificationId)
        {
            var result = await _notificationService.ResendNotification(notificationId);
            return result ? Ok() : BadRequest();
        }
    }
}