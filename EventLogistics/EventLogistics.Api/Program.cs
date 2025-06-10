using EventLogistics.Application.Interfaces;
using EventLogistics.Application.Services;
using EventLogistics.Application.Contracts.Services;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Registrar el DbContext con SQLite (combinando ambas configuraciones)
builder.Services.AddDbContext<EventLogisticsDbContext>(options =>
    options.UseSqlite("Data Source=eventlogistics.db"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios de HEAD (rama camilo)
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IReasignacionRepository, ReasignacionRepository>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IParticipantActivityRepository, ParticipantActivityRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentSolutionRepository, IncidentSolutionRepository>();

// Registrar repositorios de origin/dev
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
builder.Services.AddScoped<IReassignmentRuleRepository, ReassignmentRuleRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IResourceAssignmentRepository, ResourceAssignmentRepository>();

// Registrar servicios de aplicación de HEAD (rama camilo)
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IReasignacionServiceApp, ReasignacionServiceApp>();
builder.Services.AddScoped<INotificationServiceApp, NotificationServiceApp>();
builder.Services.AddScoped<IResourceServiceApp, ResourceServiceApp>();
builder.Services.AddScoped<IReportServiceApp, ReportServiceApp>();
builder.Services.AddScoped<IAttendanceServiceApp, AttendanceServiceApp>();
builder.Services.AddScoped<IIncidentServiceApp, IncidentServiceApp>();
builder.Services.AddScoped<IParticipantServiceApp, ParticipantServiceApp>();

// Registrar servicios de aplicación de origin/dev
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventLogistics API V1");
        c.RoutePrefix = "swagger"; // Accesible en /swagger
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Imprimir URLs importantes en la consola
if (app.Environment.IsDevelopment())
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var addresses = app.Services.GetRequiredService<Microsoft.AspNetCore.Hosting.Server.IServer>()
            .Features.Get<Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>()?.Addresses;
        
        if (addresses != null && addresses.Any())
        {
            foreach (var address in addresses)
            {
                Console.WriteLine($"🌐 API ejecutándose en: {address}");
                Console.WriteLine($"📚 Documentación Swagger: {address}/swagger");
                Console.WriteLine($"📖 OpenAPI JSON: {address}/swagger/v1/swagger.json");
                Console.WriteLine();
            }
        }
        else
        {
            // Fallback a las URLs configuradas en launchSettings.json
            Console.WriteLine($"🌐 API ejecutándose en: http://localhost:5158");
            Console.WriteLine($"📚 Documentación Swagger: http://localhost:5158/swagger");
            Console.WriteLine($"📖 OpenAPI JSON: http://localhost:5158/swagger/v1/swagger.json");
            Console.WriteLine();
        }
    });
}

app.Run();
