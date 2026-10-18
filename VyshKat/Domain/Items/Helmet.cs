using VyshKat.Domain.Interfaces;

namespace VyshKat.Domain.Items;

public class Helmet : IInventoryItem, IBeginnerFriendly  // Шлем в инвентаре, реализует интерфейс IInventoryItem и IBeginnerFriendly
{
    public string InventoryNumber { get; }  // Уникальный идентификатор шлема в инвентаре
    public string Name { get; }  // Название шлема
    public int SimplicityScore { get; }  // Оценка простоты использования шлема

    public Helmet(string inventoryNumber, string name, int simplicityScore)
    {
        if (string.IsNullOrWhiteSpace(inventoryNumber))
        {
            throw new ArgumentException("Inventory number cannot be null or whitespace.", nameof(inventoryNumber));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }
        if (simplicityScore < 1 || simplicityScore > 10)
        {
            throw new ArgumentException("Simplicity score must be between 1 nd 10.", nameof(simplicityScore));
        }

        InventoryNumber = inventoryNumber;
        Name = name;
        SimplicityScore = simplicityScore;
    }
}
