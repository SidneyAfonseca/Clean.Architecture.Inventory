using Clean.Architecture.Inventory.Application.Behaviors;
using Clean.Architecture.Inventory.Application.Commands;
using Clean.Architecture.Inventory.Application.Handlers;
using Clean.Architecture.Inventory.Application.Interfaces;
using Clean.Architecture.Inventory.Application.Queries;
using Clean.Architecture.Inventory.Domain.Entities;
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
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Registrar Behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ErrorLoggingBehavior<,>));

builder.Services.AddTransient<IRequestHandler<CreateProductCommand, int>, CreateProductCommandHandler>();
builder.Services.AddTransient<IRequestHandler<CreateInventoryTransactionCommand, int>, CreateInventoryTransactionCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteProductCommand, Unit>, DeleteProductCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateProductCommand, Unit>, UpdateProductCommandHandler>();

builder.Services.AddTransient<IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>, GetAllProductsQueryHandler>();
builder.Services.AddTransient<IRequestHandler<GetInventoryTransactionByIdQuery, InventoryTransaction>, GetInventoryTransactionByIdQueryHandler>();
builder.Services.AddTransient<IRequestHandler<GetProductByIdQuery, Product>, GetProductByIdQueryHandler>();
builder.Services.AddTransient<IRequestHandler<GetTodayTransactionsQuery, IEnumerable<InventoryTransaction>>, GetTodayTransactionsQueryHandler>();




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