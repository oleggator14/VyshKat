using VyshKat.Domain.Interfaces;
using VyshKat.Domain.Items;
using VyshKat.Domain.Vehicles;

namespace VyshKat.Domain.Services;

public class DemoRunner  // Демонстрация работы программы
{
    private readonly Park _park;
    private readonly EnergyCalculator _energyCalculator;

    public DemoRunner(Park park, EnergyCalculator energyCalculator)
    {
        _park = park;
        _energyCalculator = energyCalculator;
    }

    public void RunAllScenarios()
    {
        Console.Clear();
        Console.WriteLine("ЗАПУСК ДЕМО-СЦЕНАРИЕВ СИСТЕМЫ «ВЫШКАТ»\n");
        Console.WriteLine(new string('=', 60));

        Scenario1_AddValidVehicle();
        Wait();

        Scenario2_RejectInvalidVehicle();
        Wait();

        Scenario3_AddBicycles();
        Wait();

        Scenario4_AddInventoryItems();
        Wait();

        Scenario5_CalculateTotalEnergy();
        Wait();

        Scenario6_BeginnerFriendlyList();
        Wait();

        Scenario7_FullInventoryReport();

        Console.WriteLine(new string('=', 60));
        Console.WriteLine("✅ ВСЕ ДЕМО-СЦЕНАРИИ УСПЕШНО ЗАВЕРШЕНЫ");
        Console.WriteLine("Нажмите любую клавишу для выхода в главное меню...");
        Console.ReadKey(true);
    }

    private void Scenario1_AddValidVehicle()
    {
        Console.WriteLine("\nСценарий 1: Добавление исправного транспорта");
        Console.WriteLine("   Действие: Создание электросамоката с нормальными характеристиками...");

        var goodScooter = new Scooter("SC-001", "Xiaomi Mi Electric Scooter 4 Pro", 1.5, 8);
        Console.WriteLine($"   Отправка '{goodScooter.Name}' в ServiceCenter на техосмотр...");

        bool added = _park.AddVehicle(goodScooter);

        if (added)
            Console.WriteLine("   ✅ Результат: Транспорт ПРИНЯТ в активный парк.");
        else
            Console.WriteLine("   ❌ Результат: Транспорт ОТКЛОНЕН.");
    }

    private void Scenario2_RejectInvalidVehicle()
    {
        Console.WriteLine("\nСценарий 2: Отклонение транспорта сервисным центром");
        Console.WriteLine("   Действие: Попытка добавить самокат с аномальным энергопотреблением (150 кВт·ч)...");

        var brokenScooter = new Scooter("SC-002", "Ржавое корыто с КТЗ", 150.0, 2);
        Console.WriteLine($"   Отправка '{brokenScooter.Name}' в ServiceCenter на техосмотр...");

        bool added = _park.AddVehicle(brokenScooter);

        if (!added)
            Console.WriteLine("   🛑 Результат: Техосмотр ОТКЛОНИЛ транспорт (превышен лимит энергопотребления).");
        else
            Console.WriteLine("   ⚠️ Результат: Транспорт ошибочно принят (проверьте логику ServiceCenter).");
    }

    private void Scenario3_AddBicycles()
    {
        Console.WriteLine("\nСценарий 3: Пополнение парка велосипедами");
        Console.WriteLine("   Действие: Добавление электровелосипеда и обычного велосипеда...");

        var eBike = new ElectricBike("EB-001", "Volteco Flex", 0.8, 7);
        var bike = new Bicycle("BC-001", "Stels Navigator 700", 9);

        _park.AddVehicle(eBike);
        _park.AddVehicle(bike);

        Console.WriteLine($"   ✅ Результат: '{eBike.Name}' и '{bike.Name}' успешно прошли техосмотр и добавлены в парк.");
        Console.WriteLine("   (Примечание: обычный велосипед не имеет параметра энергопотребления)");
    }

    private void Scenario4_AddInventoryItems()
    {
        Console.WriteLine("\nСценарий 3: Постановка вещей на баланс (без техосмотра)");
        Console.WriteLine("   Действие: Добавление шлема и док-станции напрямую в парк...");

        var helmet = new Helmet("HL-001", "Шлем защитный городской (ГОСТ)", 9);
        var dockingStation = new DockingStation("DS-001", "Док-станция ВышКат v2.1", 15.5);

        _park.AddItem(helmet);
        _park.AddItem(dockingStation);

        Console.WriteLine($"   ✅ Результат: '{helmet.Name}' и '{dockingStation.Name}' успешно добавлены на баланс.");
    }

    private void Scenario5_CalculateTotalEnergy()
    {
        Console.WriteLine("\nСценарий 4: Расчёт суммарного суточного энергопотребления");
        Console.WriteLine("   Действие: Фильтрация парка по интерфейсу IEnergyConsumer и агрегация...");

        var consumers = _park.GetAllItems().OfType<IEnergyConsumer>().ToList();
        double totalEnergy = _energyCalculator.CalculateTotalDailyConsumption(consumers);

        Console.WriteLine($"   Результат: Суммарное потребление парка составляет {totalEnergy:F2} кВт·ч/сутки.");
        Console.WriteLine($"   (Учтено устройств-потребителей: {consumers.Count})");
    }

    private void Scenario6_BeginnerFriendlyList()
    {
        Console.WriteLine("\nСценарий 5: Формирование списка техники для новичков");
        Console.WriteLine("   Действие: Фильтрация транспорта с оценкой простоты >= 6...");

        var beginners = _park.GetAllItems()
            .OfType<IVehicle>()
            .OfType<IBeginnerFriendly>()
            .Where(v => v.SimplicityScore >= 6)
            .ToList();

        if (!beginners.Any())
        {
            Console.WriteLine("   ⚠️ Результат: Подходящих устройств не найдено.");
            return;
        }

        Console.WriteLine("   ✅ Результат: Найдено устройств: " + beginners.Count);
        foreach (var device in beginners)
        {
            Console.WriteLine($"      • [{device.GetType().Name,-18}] {device.Name,-35} (Оценка: {device.SimplicityScore}/10)");
        }
    }

    private void Scenario7_FullInventoryReport()
    {
        Console.WriteLine("\nСценарий 6: Полный инвентарный отчёт");
        Console.WriteLine("   Действие: Вывод всей техники и вещей с разделением по категориям...");

        var allItems = _park.GetAllItems().ToList();
        var vehicles = allItems.OfType<IVehicle>().ToList();
        var items = allItems.Where(x => x is not IVehicle).ToList();

        if (vehicles.Any())
        {
            Console.WriteLine("\n   🚴 Транспорт:");
            foreach (var v in vehicles)
            {
                Console.WriteLine($"      • [{v.GetType().Name,-18}] Инв. № {v.InventoryNumber,-10} | {v.Name}");
            }
        }

        if (items.Any())
        {
            Console.WriteLine("\n   📦 Вещи на балансе:");
            foreach (var item in items)
            {
                Console.WriteLine($"      • [{item.GetType().Name,-18}] Инв. № {item.InventoryNumber,-10} | {item.Name}");
            }
        }

        Console.WriteLine($"\n   📊 Итого в парке: {allItems.Count} ед. (транспорт: {vehicles.Count}, вещи: {items.Count})");
    }

    // Вспомогательный метод для создания паузы и визуального разделителя
    private void Wait()
    {
        Console.WriteLine(new string('-', 60));
        Thread.Sleep(4000);  // Пауза 4 секунды для чтения
    }
}
