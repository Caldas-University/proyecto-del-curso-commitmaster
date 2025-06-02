using EventLogistics.Application.Services;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using EventLogistics.Domain.Repositories;
using EventLogistics.Application.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventLogisticsDbContext>(options =>
    options.UseSqlite("Data Source=eventlogistics.db"));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IReasignacionRepository, ReasignacionRepository>();
builder.Services.AddScoped<IReasignacionServiceApp, ReasignacionServiceApp>();
builder.Services.AddScoped<INotificationServiceApp, NotificationServiceApp>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventLogistics API", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EventLogisticsDbContext>();
    SeedData.Initialize(dbContext);
}

// Middleware para Swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventLogistics API v1"));

app.MapControllers();
app.Run();
