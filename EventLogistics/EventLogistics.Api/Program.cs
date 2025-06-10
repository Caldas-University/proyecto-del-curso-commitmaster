using EventLogistics.Application.Interfaces;
using EventLogistics.Application.Services;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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

// Registrar el DbContext con SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
builder.Services.AddScoped<IReassignmentRuleRepository, ReassignmentRuleRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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