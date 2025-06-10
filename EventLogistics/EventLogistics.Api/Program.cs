using EventLogistics.Application.Contracts.Services;
using EventLogistics.Application.Services;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

// Registro de un repositorio de incidentes
using EventLogistics.Infrastructure.Repositories;
using EventLogistics.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro del repositorio de incidentes
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentServiceApp, IncidentServiceApp>(); // <-- CORREGIDO

// Registro del repositorio de soluciones de incidentes
builder.Services.AddScoped<IIncidentSolutionRepository, IncidentSolutionRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();