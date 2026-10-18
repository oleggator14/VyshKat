using VyshKat.Domain.Interfaces;
using VyshKat.Domain.Items;
using VyshKat.Domain.Utils;

namespace VyshKat.Domain.Services;

public class InventoryItemFactory  // Класс фабрики инвентаря
{
    private record InventoryItemDescriptor(string TypeName, Func<string, string, IInventoryItem> createFunc);  // Record дескриптора Наименовние<->Функция-создатель предмета

    private readonly Dictionary<int, InventoryItemDescriptor> _inventoryItemTypes;  // Словарь соответствия индекса и дескриптора предмета

    public InventoryItemFactory()  // Конструктор фабрики - первая (из 2) точка добавления нового типа предмета инвентаря
    {
        _inventoryItemTypes = new()
        {
            {1, new InventoryItemDescriptor("Шлем", CreateHelmet)},
            {2, new InventoryItemDescriptor("Шкаф для хранения батарей", CreateChargingCabinet)},
            {3, new InventoryItemDescriptor("Док-станция", CreateDockingStation)},
        };
    }

    private IInventoryItem CreateHelmet(string inventoryNumber, string name)
    {
        Console.Write("Оценка простоты (1-10): ");
        int score = ConsoleHelper.ReadInt();
        return new Helmet(inventoryNumber, name, score);
    }

    private IInventoryItem CreateChargingCabinet(string inventoryNumber, string name)
    {
        Console.Write("Потребление энергии (кВт·ч): ");
        double energy = ConsoleHelper.ReadDouble();
        return new ChargingCabinet(inventoryNumber, name, energy);
    }

    private IInventoryItem CreateDockingStation(string inventoryNumber, string name)
    {
        Console.Write("Потребление энергии (кВт·ч): ");
        double energy = ConsoleHelper.ReadDouble();
        return new DockingStation(inventoryNumber, name, energy);
    }

    // Добавление нового метода-создателя предмета - вторая (из 2) точка добавления нового типа предмета инвентаря

    public IReadOnlyDictionary<int, string> GetAvailableTypes() =>
        _inventoryItemTypes.ToDictionary(kv => kv.Key, kv => kv.Value.TypeName);  // Метод, возвращающий словарь Индекс<->Наименование (для показа раннером)

    public IInventoryItem Create(int choice, string inventoryNumber, string name)  // Метод для создания предмета, использует существующие выше методы. Поля номера и имени спрашивает раннер, они гарантированы интерфейсом
    {
        if (!_inventoryItemTypes.TryGetValue(choice, out var descriptor))
        {
            throw new ArgumentException($"Неизвестный тип: {choice}");  // Мягкая (без исключения) проверка корректности осуществляется раннером, это исключение сработает при явных ошибках
        }
        return descriptor.createFunc(inventoryNumber, name);
    }
}
