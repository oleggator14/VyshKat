using VyshKat.Domain.Interfaces;

namespace VyshKat.Domain.Items;

public class DockingStation : IInventoryItem, IEnergyConsumer  // Док-станция для зарядки и хранения оборудования, реализует интерфейсы IInventoryItem и IEnergyConsumer
{
    public string InventoryNumber { get; }
    public string Name { get; }
    public double DailyEnergyConsumption { get; }

    public DockingStation(string inventoryNumber, string name, double dailyEnergyConsumption)
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

        InventoryNumber = inventoryNumber;
        Name = name;
        DailyEnergyConsumption = dailyEnergyConsumption;
    }
}
