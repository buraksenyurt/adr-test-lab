namespace OrderManagement.Domain.Orders;

public sealed class Order
{
    private Order(Guid id, string customerEmail, decimal totalAmount)
    {
        Id = id;
        CustomerEmail = customerEmail;
        TotalAmount = totalAmount;
    }

    public Guid Id { get; }

    public string CustomerEmail { get; }

    public decimal TotalAmount { get; }

    public static Order Create(string customerEmail, decimal totalAmount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerEmail);

        if (totalAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalAmount),
                "Order total must be greater than zero.");
        }

        return new Order(Guid.NewGuid(), customerEmail.Trim(), totalAmount);
    }
}