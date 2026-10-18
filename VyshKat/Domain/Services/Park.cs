using VyshKat.Domain.Interfaces;

namespace VyshKat.Domain.Services;

public class Park  // Класс парка ТС и инвентаря
{
    private readonly List<IInventoryItem> _items = new();  // Список всех предметов в парке (ТС и инвентарь)
    private readonly IServiceCenter _serviceCenter;        // Сервисный центр

    public Park(IServiceCenter serviceCenter)  // Конструктор класса Park, принимает объект сервисного центра
    {
        _serviceCenter = serviceCenter;
    }

    public bool AddVehicle(IVehicle vehicle)  // Метод добавления ТС, проводит проверку ТС через сервисный центр
    {
        if (_serviceCenter.Inspect(vehicle))
        {
            _items.Add(vehicle);
            return true;
        }
        return false;
    }

    public void AddItem(IInventoryItem item)  // Метод добавления инвентаря
    {
        _items.Add(item);
    }

    public IReadOnlyList<IInventoryItem> GetAllItems()  // Метод, возвращающий инвентарь !только для чтения!
    {
        return _items.AsReadOnly();
    }
}
