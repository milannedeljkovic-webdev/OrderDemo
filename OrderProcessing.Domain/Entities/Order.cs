namespace OrderProcessing.Domain.Entities;

public class Order
{
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() { }

    public Order(Product product, int quantity)
    {
        ProductId = product.Id;
        Quantity = quantity;
        TotalPrice = product.Price * quantity;
        CreatedAt = DateTime.UtcNow;
    }
}