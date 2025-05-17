using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationHistoryRepository _notificationHistoryRepository;

        public NotificacionController(INotificationRepository notificationRepository, 
                                     INotificationHistoryRepository notificationHistoryRepository)
        {
            _notificationRepository = notificationRepository;
            _notificationHistoryRepository = notificationHistoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAll()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetById(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [HttpGet("history/{notificationId}")]
        public async Task<ActionResult<IEnumerable<NotificationHistory>>> GetHistory(int notificationId)
        {
            var history = await _notificationHistoryRepository.GetByNotificationIdAsync(notificationId);
            return Ok(history);
        }

        [HttpPost]
        public async Task<ActionResult<Notification>> Create(Notification notification)
        {
            notification.Timestamp = DateTime.UtcNow;
            var result = await _notificationRepository.AddAsync(notification);
            
            // Registrar la creación en el historial
            var history = new NotificationHistory
            {
                NotificationId = result.Id,
                Action = "Created",
                ActionTimestamp = DateTime.UtcNow,
                Details = "Notification created",
                Result = "Success"
            };
            await _notificationHistoryRepository.AddAsync(history);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}