using VyshKat.Domain.Interfaces;
using VyshKat.Domain.Utils;
using VyshKat.Domain.Vehicles;

namespace VyshKat.Domain.Factories;

public class VehicleFactory  // Класс фабрики транспортных средств
{
    private record VehicleDescriptor(string TypeName, Func<string, string, IVehicle> createFunc);  // Record дескриптора Наименовние<->Функция-создатель ТС

    private readonly Dictionary<int, VehicleDescriptor> _vehicleTypes;  // Словарь соответствия индекса и дескриптора ТС

    public VehicleFactory()  // Конструктор фабрики - первая (из 2) точка добавления нового вида ТС
    {
        _vehicleTypes = new()
        {
            {1, new VehicleDescriptor("Электросамокат", CreateScooter) },
            {2, new VehicleDescriptor("Велосипед", CreateBicycle) },
            {3, new VehicleDescriptor("Электровелосипед", CreateElectricBike) },
        };
    }

    private IVehicle CreateScooter(string inventoryNumber, string name)
    {
        Console.Write("Потребление энергии (кВт·ч): ");
        double energy = ConsoleHelper.ReadDouble();
        Console.Write("Оценка простоты (1-10): ");
        int score = ConsoleHelper.ReadInt();
        return new Scooter(inventoryNumber, name, energy, score);
    }

    private IVehicle CreateBicycle(string inventoryNumber, string name)
    {
        Console.Write("Оценка простоты (1-10): ");
        int score = ConsoleHelper.ReadInt();
        return new Bicycle(inventoryNumber, name, score);
    }

    private IVehicle CreateElectricBike(string inventoryNumber, string name)
    {
        Console.Write("Потребление энергии (кВт·ч): ");
        double energy = ConsoleHelper.ReadDouble();
        Console.Write("Оценка простоты (1-10): ");
        int score = ConsoleHelper.ReadInt();
        return new ElectricBike(inventoryNumber, name, energy, score);
    }

    // Добавление нового метода-создателя ТС - вторая (из 2) точка добавления нового ТС

    public IReadOnlyDictionary<int, string> GetAvailableTypes() =>
        _vehicleTypes.ToDictionary(kv => kv.Key, kv => kv.Value.TypeName);  // Метод, возвращающий словарь Индекс<->Наименование (для показа раннером)

    public IVehicle Create(int choice, string inventoryNumber, string name)  // Метод для создания ТС, использует существующие выше методы. Поля номера и имени спрашивает раннер, они гарантированы интерфейсом
    {
        if (!_vehicleTypes.TryGetValue(choice, out var descriptor))
        {
            throw new ArgumentException($"Неизвестный тип: {choice}");  // Мягкая (без исключения) проверка корректности осуществляется раннером, это исключение сработает при явных ошибках
        }
        return descriptor.createFunc(inventoryNumber, name);
    }
}
