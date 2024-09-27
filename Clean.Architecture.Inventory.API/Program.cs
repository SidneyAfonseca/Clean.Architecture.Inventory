using Clean.Architecture.Inventory.Application.Behaviors;
using Clean.Architecture.Inventory.Application.Commands;
using Clean.Architecture.Inventory.Application.Interfaces;
using Clean.Architecture.Inventory.Persistence;
using Clean.Architecture.Inventory.Persistence.Repositories;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Configura��o de Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    //.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Opcional
    .CreateLogger();

builder.Host.UseSerilog();

// Configurar DbContext com MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InventoryControlDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Registrar Reposit�rios e Servi�os
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

// Registrar MediatR e Handlers com ServiceFactory
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Registrar Behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ErrorLoggingBehavior<,>));

// Adicionar suporte a valida��o com FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

// Adicionar Swagger para documenta��o
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware de Logging de Requisi��es
app.UseSerilogRequestLogging();

// Configura��o do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Iniciando a aplica��o");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplica��o falhou ao iniciar");
}
finally
{
    Log.CloseAndFlush();
}