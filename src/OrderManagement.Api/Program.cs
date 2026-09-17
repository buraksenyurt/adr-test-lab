using OrderManagement.Application.Orders.Handlers;
using OrderManagement.Api.Endpoints;
using OrderManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<GetOrderHandler>();

var app = builder.Build();

app.MapOrderEndpoints();

await app.RunAsync();
