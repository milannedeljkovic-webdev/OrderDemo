namespace OrderProcessing.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    
    // PROMENA OVDE: Dodali smo = default!;
    public string Name { get; private set; } = default!; 
    
    public int StockQuantity { get; private set; }
    public decimal Price { get; private set; }
    
    // I OVDE: Dodali smo = default!;
    public byte[] RowVersion { get; private set; } = default!; 

    // Ovaj konstruktor sada ne baca upozorenje jer smo gore stavili default!
    private Product() { } 

    public Product(int id, string name, int stockQuantity, decimal price)
    {
        Id = id;
        Name = name;
        StockQuantity = stockQuantity;
        Price = price;
        // RowVersion se ne setuje ovde, to radi baza/EF
    }

    public void DebitStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");
        if (StockQuantity < quantity) throw new InvalidOperationException("Nema dovoljno robe na stanju!");
        
        StockQuantity -= quantity;
    }
}