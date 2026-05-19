namespace Catalog.API.Domain.Entities;

public sealed class MenuItem(int id, string name, string description, decimal price, bool isAvailable, int inventoryCount, DateTime createdAt)
{
    private MenuItem()
        : this(0, string.Empty, string.Empty, 0m, false, 0, DateTime.MinValue)
    {
    }

    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public decimal Price { get; private set; } = price;
    public bool IsAvailable { get; private set; } = isAvailable;
    public int InventoryCount { get; private set; } = inventoryCount;
    public DateTime CreatedAt { get; private set; } = createdAt;

    public static MenuItem Create(string name, string description, decimal price, bool isAvailable, int inventoryCount)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Menu item name cannot be empty.", nameof(name));
        }

        if (price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");
        }

        return new MenuItem(
            id: 0,
            name: name.Trim(),
            description: description?.Trim() ?? string.Empty,
            price: price,
            isAvailable: isAvailable && inventoryCount > 0,
            inventoryCount: inventoryCount,
            createdAt: DateTime.UtcNow);
    }

    public void AdjustInventory(int inventoryCount)
    {
        if (inventoryCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(inventoryCount), "Inventory count cannot be negative.");
        }

        InventoryCount = inventoryCount;
        IsAvailable = inventoryCount > 0;
    }
}
