using EventLogistics.Application.Services;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registrar el DbContext con SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
builder.Services.AddScoped<IReassignmentRuleRepository, ReassignmentRuleRepository>();

// Registrar servicios de aplicación
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReassignmentService>();

// Agregar controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configurar opciones de serialización para manejar referencias circulares
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
    app.MapOpenApi();
}


app.MapControllers();

app.Run();
