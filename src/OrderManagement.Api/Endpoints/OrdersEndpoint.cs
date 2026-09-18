using OrderManagement.Application.Orders;
using OrderManagement.Application.Orders.Handlers;
using OrderManagement.Api.Contracts;

namespace OrderManagement.Api.Endpoints;

public static class OrdersEndpoint
{
    
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/orders",
            async (
                CreateOrderRequest request,
                CreateOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateOrderCommand(request.CustomerEmail, request.TotalAmount);
                var result = await handler.HandleAsync(command, cancellationToken);

                return Results.Created($"/orders/{result.OrderId}", result);
            });

        endpoints.MapGet(
            "/orders/{orderId:guid}",
            async (
                Guid orderId,
                GetOrderHandler handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetOrderQuery(orderId);
                var result = await handler.HandleAsync(query, cancellationToken);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            });

        return endpoints;
    }
}