namespace EventLogistics.Api.Controllers;

using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationServiceApp _notificationService;

    public NotificationController(INotificationServiceApp notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> GenerateNotification([FromBody] GenerateNotificationRequest request)
    {
        var notification = await _notificationService.GenerateNotificationAsync(request.RecipientId, request.Content);
        return Ok(notification);
    }

    [HttpPost("{id}/send")]
    public async Task<ActionResult<bool>> SendNotification(Guid id)
    {
        var result = await _notificationService.SendCommunicationAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/confirm")]
    public async Task<ActionResult<bool>> ConfirmNotification(Guid id)
    {
        var result = await _notificationService.ConfirmReceptionAsync(id);
        return Ok(result);
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<Dictionary<string, double>>> GetMetrics()
    {
        var metrics = await _notificationService.CalculateMetricsAsync();
        return Ok(metrics);
    }

    public class GenerateNotificationRequest
    {
        public Guid RecipientId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}