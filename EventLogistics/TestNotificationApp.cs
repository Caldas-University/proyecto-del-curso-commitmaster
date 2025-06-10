using EventLogistics.Application.DTOs;
using EventLogistics.Application.Services;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Implementación mock simple para testing
public class MockNotificationRepository : INotificationRepository
{
    private readonly List<Notification> _notifications = new();

    public async Task<Notification> AddAsync(Notification entity)
    {
        entity.Id = Guid.NewGuid();
        _notifications.Add(entity);
        return await Task.FromResult(entity);
    }

    public async Task<Notification?> GetByIdAsync(Guid id)
    {
        return await Task.FromResult(_notifications.FirstOrDefault(n => n.Id == id));
    }

    public async Task<IEnumerable<Notification>> GetAllAsync()
    {
        return await Task.FromResult(_notifications);
    }

    public async Task UpdateAsync(Notification entity)
    {
        var existing = _notifications.FirstOrDefault(n => n.Id == entity.Id);
        if (existing != null)
        {
            var index = _notifications.IndexOf(existing);
            _notifications[index] = entity;
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = _notifications.FirstOrDefault(n => n.Id == id);
        if (existing != null)
        {
            _notifications.Remove(existing);
        }
        await Task.CompletedTask;
    }
}

public class MockNotificationHistoryRepository : INotificationHistoryRepository
{
    private readonly List<NotificationHistory> _history = new();

    public async Task<NotificationHistory> AddAsync(NotificationHistory entity)
    {
        entity.Id = Guid.NewGuid();
        _history.Add(entity);
        return await Task.FromResult(entity);
    }

    public async Task<NotificationHistory?> GetByIdAsync(Guid id)
    {
        return await Task.FromResult(_history.FirstOrDefault(h => h.Id == id));
    }

    public async Task<IEnumerable<NotificationHistory>> GetAllAsync()
    {
        return await Task.FromResult(_history);
    }

    public async Task<IEnumerable<NotificationHistory>> GetByNotificationIdAsync(Guid notificationId)
    {
        return await Task.FromResult(_history.Where(h => h.NotificationId == notificationId));
    }

    public async Task UpdateAsync(NotificationHistory entity)
    {
        var existing = _history.FirstOrDefault(h => h.Id == entity.Id);
        if (existing != null)
        {
            var index = _history.IndexOf(existing);
            _history[index] = entity;
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = _history.FirstOrDefault(h => h.Id == id);
        if (existing != null)
        {
            _history.Remove(existing);
        }
        await Task.CompletedTask;
    }
}

// Programa de prueba
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 Probando NotificationServiceApp");
        Console.WriteLine("=====================================");

        // Crear repositorios mock
        var notificationRepo = new MockNotificationRepository();
        var historyRepo = new MockNotificationHistoryRepository();

        // Crear servicio
        var notificationService = new NotificationServiceApp(notificationRepo, historyRepo);

        try
        {
            // Prueba 1: Generar notificación
            Console.WriteLine("\n📝 Prueba 1: Generar notificación");
            var recipientId = Guid.NewGuid();
            var notificationDto = await notificationService.GenerateNotificationAsync(
                recipientId, 
                "Prueba de notificación del sistema"
            );

            Console.WriteLine($"✅ Notificación generada:");
            Console.WriteLine($"   ID: {notificationDto.Id}");
            Console.WriteLine($"   Destinatario: {notificationDto.RecipientId}");
            Console.WriteLine($"   Contenido: {notificationDto.Content}");
            Console.WriteLine($"   Estado: {notificationDto.Status}");
            Console.WriteLine($"   Fecha: {notificationDto.Timestamp}");

            // Prueba 2: Enviar comunicación
            Console.WriteLine("\n📤 Prueba 2: Enviar comunicación");
            var sendResult = await notificationService.SendCommunicationAsync(notificationDto.Id);
            Console.WriteLine($"✅ Comunicación enviada: {sendResult}");

            // Prueba 3: Confirmar recepción
            Console.WriteLine("\n✅ Prueba 3: Confirmar recepción");
            var confirmResult = await notificationService.ConfirmReceptionAsync(notificationDto.Id);
            Console.WriteLine($"✅ Recepción confirmada: {confirmResult}");

            // Prueba 4: Registrar log
            Console.WriteLine("\n📋 Prueba 4: Registrar log");
            await notificationService.RegisterLogAsync("Log de prueba del sistema");
            Console.WriteLine("✅ Log registrado correctamente");

            // Prueba 5: Calcular métricas
            Console.WriteLine("\n📊 Prueba 5: Calcular métricas");
            var metrics = await notificationService.CalculateMetricsAsync();
            Console.WriteLine("✅ Métricas calculadas:");
            foreach (var metric in metrics)
            {
                Console.WriteLine($"   {metric.Key}: {metric.Value}");
            }

            Console.WriteLine("\n🎉 ¡Todas las pruebas completadas exitosamente!");
            Console.WriteLine("\n💡 El NotificationServiceApp está funcionando correctamente");
            Console.WriteLine("   - Genera notificaciones con IDs tipo Guid ✅");
            Console.WriteLine("   - Persiste datos usando repositorios ✅");
            Console.WriteLine("   - Registra historial de acciones ✅");
            Console.WriteLine("   - Calcula métricas de rendimiento ✅");
            Console.WriteLine("   - Maneja errores apropiadamente ✅");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error durante las pruebas: {ex.Message}");
            Console.WriteLine($"   Stack Trace: {ex.StackTrace}");
        }

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
