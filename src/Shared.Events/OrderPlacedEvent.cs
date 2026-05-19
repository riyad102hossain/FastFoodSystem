namespace Shared.Events;

public sealed record OrderPlacedEvent(Guid OrderId, int ItemId, int Quantity);
