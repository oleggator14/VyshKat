using VyshKat.Domain.Factories;
using VyshKat.Domain.Interfaces;
using VyshKat.Domain.Utils;

namespace VyshKat.Domain.Services;

public class InteractiveRunner  // Раннер, ведущий программу
{
    private readonly Park _park;
    private readonly EnergyCalculator _energyCalculator;
    private readonly VehicleFactory _vehicleFactory;
    private readonly InventoryItemFactory _inventoryItemFactory;

    public InteractiveRunner(Park park, EnergyCalculator energyCalculator, VehicleFactory vehicleFactory, InventoryItemFactory inventoryItemFactory)
    {
        _park = park;
        _energyCalculator = energyCalculator;
        _vehicleFactory = vehicleFactory;
        _inventoryItemFactory = inventoryItemFactory;
    }

    public void Run()
    {
        Console.WriteLine("Запуск интерактивного режима «ВышКат»\n");
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Добавить транспорт (с техосмотром)");
            Console.WriteLine("2. Добавить вещь на баланс (без ТО)");
            Console.WriteLine("3. Показать суммарное суточное энергопотребление");
            Console.WriteLine("4. Показать технику для новичков");
            Console.WriteLine("5. Показать весь инвентарь");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие (0-5): ");

            string? choice = Console.ReadLine();

            if (choice == null)
            {
                Console.WriteLine("\nРабота прервана пользователем.");
                return;
            }

            switch (choice)
            {
                case "0":
                    exit = true;
                    Console.WriteLine("Выход из приложения...");
                    break;

                case "1":
                    AddVehicleInteractive();
                    break;

                case "2":
                    AddItemInteractive();
                    break;

                case "3":
                    ShowEnergyReport();
                    break;

                case "4":
                    ShowBeginnerList();
                    break;

                case "5":
                    ShowFullInventory();
                    break;

                default:
                    Console.WriteLine("⚠️ Неверный ввод. Пожалуйста, выберите число от 0 до 5.");
                    break;
            }
        }
    }

    private void AddVehicleInteractive()  // Добавление ТС
    {
        Console.WriteLine("Выберите тип транспорта:");

        var availableTypes = _vehicleFactory.GetAvailableTypes();
        int choice;

        foreach (var (id, displayName) in availableTypes)
        {
            Console.WriteLine($"{id}. {displayName}");  // Вывод возможных вариантов
        }
        Console.WriteLine("0. Отмена");

        while (true)  // Валидация введенного типа транспорта - ответственность раннера
        {
            Console.Write("Ваш выбор: ");
            choice = ConsoleHelper.ReadInt();

            if (choice == 0) return;

            if (availableTypes.ContainsKey(choice))
                break;  // Валидный выбор номера

            Console.WriteLine($"⚠️ Типа с номером {choice} не существует. Попробуйте снова.");
        }

        // Раннер спрашивает общие поля один раз, это его ответственность
        // Пустые номера и названия валидируются на уровне классов моделей, раннер сам по себе не запрещает пустые строки, это уже не его ответственность
        Console.Write("Введите инвентарный номер (напр. SC-003): ");
        string invNum = ConsoleHelper.ReadString();

        Console.Write("Введите название модели (напр. Xiaomi Pro 2): ");
        string name = ConsoleHelper.ReadString();

        try  // Создаем транспорт фабрикой, отправляем на ТО
        {
            IVehicle vehicle = _vehicleFactory.Create(choice, invNum, name);

            if (_park.AddVehicle(vehicle))
                Console.WriteLine($"✅ {vehicle.Name} принят в парк");
            else
                Console.WriteLine($"❌ {vehicle.Name} отклонён техосмотром");
        }
        catch (ArgumentException ex)  // Ошибки фабрики и создания ТС, например пустой номер у самоката
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }

        ConsoleHelper.WaitForAnyKey();
    }

    private void AddItemInteractive()  // Добавление предмета
    {
        Console.WriteLine("Выберите тип предмета:");

        var availableTypes = _inventoryItemFactory.GetAvailableTypes();
        int choice;

        foreach (var (id, displayName) in availableTypes)
        {
            Console.WriteLine($"{id}. {displayName}");  // Вывод возможных вариантов
        }
        Console.WriteLine("0. Отмена");

        while (true)  // Валидация введенного типа предмета - ответственность раннера
        {
            Console.Write("Ваш выбор: ");
            choice = ConsoleHelper.ReadInt();

            if (choice == 0) return;

            if (availableTypes.ContainsKey(choice))
                break;  // Валидный выбор номера

            Console.WriteLine($"⚠️ Типа с номером {choice} не существует. Попробуйте снова.");
        }

        // Раннер спрашивает общие поля один раз, это его ответственность
        // Пустые номера и названия валидируются на уровне классов моделей, раннер сам по себе не запрещает пустые строки, это уже не его ответственность
        Console.Write("Введите инвентарный номер (напр. HL-001): ");
        string invNum = ConsoleHelper.ReadString();

        Console.Write("Введите название модели (напр. Xiaomi Station Ultra 7): ");
        string name = ConsoleHelper.ReadString();

        try  // Создаем предмет фабрикой
        {
            IInventoryItem inventoryItem = _inventoryItemFactory.Create(choice, invNum, name);

            _park.AddItem(inventoryItem);
            Console.WriteLine($"✅ Предмет '{inventoryItem.Name}' успешно добавлен.");
        }
        catch (ArgumentException ex)  // Ошибки фабрики и создания предмета, например пустой номер у шлема
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }

        ConsoleHelper.WaitForAnyKey();
    }

    private void ShowEnergyReport()  // Показ отчета по энергопотреблению
    {
        Console.WriteLine("\n--- Отчёт по энергопотреблению ---");

        var energyConsumers = _park.GetAllItems().OfType<IEnergyConsumer>();  // Из всех элементов парка берем только тех, кто потребляет энергию 

        double totalEnergy = _energyCalculator.CalculateTotalDailyConsumption(energyConsumers);  // Считаем их энергию с помощью метода класса EnergyCalculator

        Console.WriteLine($"Суммарное суточное потребление парка: {totalEnergy:F2} кВт·ч");

        Console.WriteLine($"(Всего устройств-потребителей: {energyConsumers.Count()})");

        ConsoleHelper.WaitForAnyKey();
    }

    private void ShowBeginnerList()  // Показ списка предметов для новичков
    {
        Console.WriteLine("\n--- Список устройств для новичков (простота ≥ 6) ---");

        // Из всех элементов парка берем только тех, кто имеет характеристику для новичков, при том >= 6
        var beginnerFriendlyDevices = _park.GetAllItems().OfType<IBeginnerFriendly>().Where(x => x.SimplicityScore >= 6).ToList();

        if (beginnerFriendlyDevices.Count == 0)  // Отдельный вывод для пустого списка
        {
            Console.WriteLine("⚠️ На данный момент в парке нет устройств для новичков.");
            return;
        }

        foreach (var device in beginnerFriendlyDevices)
        {
            // GetType().Name вернёт имя класса: "Scooter", "Helmet" и т.д.
            string typeName = device.GetType().Name;

            // Форматируем строку с выравниванием для читаемости
            Console.WriteLine($"  • [{typeName,-18}] {device.Name,-35} (Инв. № {device.InventoryNumber}) | Простота: {device.SimplicityScore}/10");
        }

        Console.WriteLine($"\n✅ Всего найдено подходящих устройств: {beginnerFriendlyDevices.Count}");
        ConsoleHelper.WaitForAnyKey();
    }

    private void ShowFullInventory()
    {
        Console.WriteLine("\n=== Полный инвентарный список (техника и вещи) ===");

        var allItems = _park.GetAllItems().ToList();  // Материализуем список (Deferred Execution)

        if (allItems.Count == 0)  // Отдельный вывод для пустого списка
        {
            Console.WriteLine("⚠️ Парк пуст. Добавьте технику или вещи через меню.");
            return;
        }

        // Разделяем на транспорт и вещи для наглядности
        var vehicles = allItems.OfType<IVehicle>().ToList();
        var items = allItems.Where(x => x is not IVehicle).ToList();

        // Вывод транспорта
        if (vehicles.Count != 0)
        {
            Console.WriteLine("\nТранспорт:");
            foreach (var vehicle in vehicles)
            {
                Console.WriteLine($"  • [{vehicle.GetType().Name,-18}] Инв. № {vehicle.InventoryNumber,-10} | {vehicle.Name}");
            }
        }

        // Вывод вещей
        if (items.Count != 0)
        {
            Console.WriteLine("\nВещи на балансе:");
            foreach (var item in items)
            {
                Console.WriteLine($"  • [{item.GetType().Name,-18}] Инв. № {item.InventoryNumber,-10} | {item.Name}");
            }
        }

        Console.WriteLine($"\n✅ Всего в парке: {allItems.Count} единиц (транспорт: {vehicles.Count}, вещи: {items.Count})");
        ConsoleHelper.WaitForAnyKey();
    }
}
