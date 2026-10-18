using Microsoft.Extensions.DependencyInjection;
using VyshKat.Domain.Factories;
using VyshKat.Domain.Interfaces;
using VyshKat.Domain.Services;

namespace VyshKat;

class Program
{
    static void Main(string[] args)
    {
        // Исправление кодировки - без него знаки вопроса (?) вместо букв
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Создаем и заполняем коллекцию зависимостей
        var services = new ServiceCollection();
        ConfigureServices(services);

        // Создаем DI контейнер, using здесь для автоматического освобождения ресурсов после завершения работы
        using var provider = services.BuildServiceProvider();

        if (args.Length == 0 || args[0] == "--interactive" || args[0] == "-i")
        {
            // Просим у контейнера InteractiveRunner, и запускаем
            var runner = provider.GetRequiredService<InteractiveRunner>();
            runner.Run();
        }

        else if (args[0] == "--demo" || args[0] == "-d")
        {
            // Просим у контейнера DemoRunner, и запускаем
            var runner = provider.GetRequiredService<DemoRunner>();
            runner.RunAllScenarios();
        }

        else if (args[0] == "--help" || args[0] == "-h")
        {
            ShowFullHelp();
        }
        else
        {
            Console.WriteLine($"❌ Неизвестная команда: {args[0]}");
            ShowBriefHelp();
        }
    }

    private static void ConfigureServices(IServiceCollection services)  // Регистрация сервисов в контейнере зависимостей
    {
        // Singleton подходит для сервисов, т.к. они должны быть единственными в приложении и хранить состояние
        services.AddSingleton<IServiceCenter, ServiceCenter>();
        services.AddSingleton<Park>();
        services.AddSingleton<EnergyCalculator>();
        services.AddSingleton<VehicleFactory>();
        services.AddSingleton<InventoryItemFactory>();

        // Transient подходит для раннеров, т.к. они не хранят состояние между вызовами и создаются заново при каждом запросе
        services.AddTransient<InteractiveRunner>();
        services.AddTransient<DemoRunner>();
    }

    private static void ShowBriefHelp()
    {
        Console.WriteLine("Использование: VyshKat [команда]");
        Console.WriteLine();
        Console.WriteLine("Команды:");
        Console.WriteLine("  (без команд)     Запуск интерактивного меню");
        Console.WriteLine("  --demo, -d       Автоматическая демонстрация сценариев из ТЗ");
        Console.WriteLine("  --help, -h       Показать подробную справку");
        Console.WriteLine();
        Console.WriteLine("Примеры запуска:");
        Console.WriteLine("  ./VyshKat --demo");
        Console.WriteLine("  dotnet run -- --demo");
    }

    private static void ShowFullHelp()
    {
        ShowBriefHelp();
        Console.WriteLine("=== ПОДРОБНАЯ СПРАВКА ===");
        Console.WriteLine();
        Console.WriteLine("Интерактивное меню позволяет:");
        Console.WriteLine("  1. Добавить транспорт (проходит техосмотр в ServiceCenter)");
        Console.WriteLine("  2. Добавить вещь на баланс (шлем, станция и т.д., без ТО)");
        Console.WriteLine("  3. Рассчитать суммарное суточное энергопотребление парка");
        Console.WriteLine("  4. Показать список техники, подходящей для новичков (оценка >= 6)");
        Console.WriteLine("  5. Вывести полный инвентарный список (вся техника и вещи)");
        Console.WriteLine();
        Console.WriteLine("Для запуска юнит-тестов используйте команду: dotnet test");
    }
}
