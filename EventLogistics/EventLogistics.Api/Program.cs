using EventLogistics.Application.Services;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar el DbContext con SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios genéricos y específicos
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
builder.Services.AddScoped<IReassignmentRuleRepository, ReassignmentRuleRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<BadgeGenerator>();
builder.Services.AddScoped<IBadgeGenerator, BadgeGenerator>();
builder.Services.AddScoped<ICredentialService, CredentialService>();

// Registrar servicios de aplicación
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReassignmentService>();
builder.Services.AddScoped<ReportService>();

// Agregar controladores y configuración de JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Agregar soporte para Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Middleware de desarrollo con Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Logistics API V1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

// Middleware general
app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();


app.Run();
