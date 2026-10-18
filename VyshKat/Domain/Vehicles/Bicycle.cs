using VyshKat.Domain.Interfaces;

namespace VyshKat.Domain.Vehicles;

public class Bicycle : IVehicle, IBeginnerFriendly  // Класс для обычного велосипеда, реализует интерфейсы IVehicle и IBeginnerFriendly
{
    public string InventoryNumber { get; } // Уникальный идентификатор велосипеда в инвентаре
    public string Name { get; }  // Название велосипеда
    public int SimplicityScore { get; }  // Оценка простоты использования  велосипеда

    public Bicycle(string inventoryNumber, string name, int simplicityScore)  // Конструктор класса Bicycle
    {
        if (string.IsNullOrWhiteSpace(inventoryNumber))
            throw new ArgumentException("Inventory number cannot be null or whitespace.", nameof(inventoryNumber));
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        
        if (simplicityScore < 1 || simplicityScore > 10)
            throw new ArgumentException("Simplicity score must be between 1 nd 10.", nameof(simplicityScore));

        InventoryNumber = inventoryNumber;
        Name = name;
        SimplicityScore = simplicityScore;
    }
}
