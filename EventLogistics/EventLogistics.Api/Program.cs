using EventLogistics.Application.Interfaces;
using EventLogistics.Application.Services;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using EventLogistics.Domain.Repositories;
using EventLogistics.Application.Interfaces;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure; // Agrega este using al inicio si no está

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventLogisticsDbContext>(options =>
    options.UseSqlite("Data Source=eventlogistics.db"));
=======
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Agregar Swagger (para la interfaz UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "EventLogistics API", 
        Version = "v1",
        Description = "API para gestión de logística de eventos"
    });
});
>>>>>>> sebas

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IReasignacionRepository, ReasignacionRepository>();
builder.Services.AddScoped<IReasignacionServiceApp, ReasignacionServiceApp>();
builder.Services.AddScoped<INotificationServiceApp, NotificationServiceApp>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IResourceServiceApp, ResourceServiceApp>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IAttendanceServiceApp, AttendanceServiceApp>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IParticipantActivityRepository, ParticipantActivityRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

<<<<<<< HEAD
// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
=======
// Registrar servicios de aplicación
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<IReassignmentService, ReassignmentService>();
// Registrar los nuevos repositorios y servicios
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IResourceAssignmentRepository, ResourceAssignmentRepository>();
builder.Services.AddScoped<IConflictValidationService, ConflictValidationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();

// Agregar controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configurar CORS
builder.Services.AddCors(options =>
>>>>>>> sebas
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventLogistics API", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
<<<<<<< HEAD
    var dbContext = scope.ServiceProvider.GetRequiredService<EventLogisticsDbContext>();
    SeedData.Initialize(dbContext);
}

// Middleware para Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventLogistics API v1");
    c.RoutePrefix = string.Empty; // Esto pone Swagger en la raíz
});

// Configura la licencia de QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

app.MapControllers();
app.Run();
=======
    // OpenAPI (mantén lo que ya tienes)
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

app.Run();
>>>>>>> sebas
