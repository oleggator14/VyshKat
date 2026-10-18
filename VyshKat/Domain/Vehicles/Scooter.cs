using VyshKat.Domain.Interfaces;

namespace VyshKat.Domain.Vehicles;

public class Scooter : IVehicle, IEnergyConsumer, IBeginnerFriendly  // Класс для электросамоката, реализует интерфейсы IVehicle, IEnergyConsumer и IBeginnerFriendly
{
    public string InventoryNumber { get; }         // Уникальный идентификатор электросамоката в инвентаре
    public string Name { get; }                    // Полное наименование электросамоката
    public double DailyEnergyConsumption { get; }  // Ежедневное потребление энергии в киловатт-часах (кВт·ч)
    public int SimplicityScore { get; }            // Оценка простоты

    public Scooter(string inventoryNumber, string name, double dailyEnergyConsumption, int simplicityScore)  // Конструктор класса Scooter
    {
        if (string.IsNullOrWhiteSpace(inventoryNumber))
        {
            throw new ArgumentException("Inventory number cannot be null or whitespace.", nameof(inventoryNumber));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }
        if (dailyEnergyConsumption < 0)
        {
            throw new ArgumentException("Daily energy consumption cannot be negative.", nameof(dailyEnergyConsumption));
        }
        if (simplicityScore < 1 || simplicityScore > 10)
        {
            throw new ArgumentException("Simplicity score must be between 1 and 10.", nameof(simplicityScore));
        }

        InventoryNumber = inventoryNumber;
        Name = name;
        DailyEnergyConsumption = dailyEnergyConsumption;
        SimplicityScore = simplicityScore;
    }
}
