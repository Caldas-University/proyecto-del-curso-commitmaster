using EventLogistics.Application.Interfaces;
using EventLogistics.Application.Services;
using EventLogistics.Application.Contracts.Services;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Registrar ÚNICAMENTE EventLogisticsDbContext con SQLite
builder.Services.AddDbContext<EventLogisticsDbContext>(options =>
    options.UseSqlite("Data Source=eventlogistics.db"));

// Registrar repositorios unificados
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IReasignacionRepository, ReasignacionRepository>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IParticipantActivityRepository, ParticipantActivityRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentSolutionRepository, IncidentSolutionRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
builder.Services.AddScoped<IReassignmentRuleRepository, ReassignmentRuleRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IResourceAssignmentRepository, ResourceAssignmentRepository>();

// Registrar servicios de aplicación
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IReasignacionServiceApp, ReasignacionServiceApp>();
builder.Services.AddScoped<INotificationServiceApp, NotificationServiceApp>();
builder.Services.AddScoped<IResourceServiceApp, ResourceServiceApp>();
builder.Services.AddScoped<IReportServiceApp, ReportServiceApp>();
builder.Services.AddScoped<IAttendanceServiceApp, AttendanceServiceApp>();
builder.Services.AddScoped<IIncidentServiceApp, IncidentServiceApp>();
builder.Services.AddScoped<IParticipantServiceApp, ParticipantServiceApp>();

// Registrar servicios adicionales
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<IReassignmentService, ReassignmentService>();
builder.Services.AddScoped<IConflictValidationService, ConflictValidationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();

// Agregar controladores con configuración JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "EventLogistics API", 
        Version = "v1",
        Description = "API para gestión de logística de eventos"
    });
});

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Registrar licencia de QuestPDF (Community)
QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Initialize database with seed data
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<EventLogisticsDbContext>();
        SeedData.Initialize(dbContext);
    }
    catch
    {
        // Si falla, continuar sin seed data
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Agregar Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventLogistics API v1");
        c.RoutePrefix = string.Empty; // Swagger UI en la raíz
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Mostrar enlaces útiles en la consola
app.Lifetime.ApplicationStarted.Register(() =>
{
    var addresses = app.Services.GetRequiredService<IServer>().Features
        .Get<IServerAddressesFeature>()?.Addresses;
    
    if (addresses != null && addresses.Any())
    {
        var address = addresses.First();
        Console.WriteLine();
        Console.WriteLine("🚀 EventLogistics API está ejecutándose!");
        Console.WriteLine($"📋 Swagger UI: {address}");
        Console.WriteLine($"🔗 API Base URL: {address}/api");
        Console.WriteLine($"📊 Health Check: {address}/health");
        Console.WriteLine();
    }
});

app.Run();
