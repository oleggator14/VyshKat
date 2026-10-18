using VyshKat.Domain.Interfaces;
using VyshKat.Domain.Items;
using VyshKat.Domain.Services;
using VyshKat.Domain.Vehicles;

namespace VyshKat.Tests;

public class VyshKatTests
{
    // Вспомогательные классы для подмены зависимостей DI
    private class FakeApprovingServiceCenter : IServiceCenter
    {
        public bool Inspect(IVehicle vehicle) => true;
    }

    private class FakeRejectingServiceCenter : IServiceCenter
    {
        public bool Inspect(IVehicle vehicle) => false;
    }

    // Сценарий 1: Добавление транспорта (успешное)
    [Fact]
    public void Park_AddVehicles_ShouldAcceptValidTransport()
    {
        var park = new Park(new FakeApprovingServiceCenter());

        var scooter = new Scooter("SC-001", "Xiaomi Pro", 1.5, 8);
        var eBike = new ElectricBike("EB-001", "Volteco Flex", 0.8, 7);
        var bike = new Bicycle("BC-001", "Stels Navigator", 9);

        park.AddVehicle(scooter);
        park.AddVehicle(eBike);
        park.AddVehicle(bike);

        Assert.Equal(3, park.GetAllItems().Count);
        Assert.Contains(scooter, park.GetAllItems());
        Assert.Contains(bike, park.GetAllItems());
    }

    // Сценарий 2: Отклонение транспорта сервисным центром
    [Fact]
    public void Park_AddVehicle_WhenServiceCenterRejects_ShouldNotAdd()
    {
        var park = new Park(new FakeRejectingServiceCenter());
        var brokenScooter = new Scooter("SC-002", "Broken", 150.0, 2);

        bool result = park.AddVehicle(brokenScooter);

        Assert.False(result);
        Assert.Empty(park.GetAllItems());
    }

    // Сценарий 3: Добавление вещей на баланс (без ТО)
    [Fact]
    public void Park_AddItems_ShouldAddDirectlyWithoutInspection()
    {
        var park = new Park(new FakeRejectingServiceCenter());

        var helmet = new Helmet("HL-001", "Шлем ГОСТ", 9);
        var dockingStation = new DockingStation("DS-001", "Док-станция", 15.5);
        var chargingCabinet = new ChargingCabinet("CC-001", "Шкаф для зарядки", 20.0);

        park.AddItem(helmet);
        park.AddItem(dockingStation);
        park.AddItem(chargingCabinet);

        Assert.Equal(3, park.GetAllItems().Count);
        Assert.Contains(helmet, park.GetAllItems());
        Assert.Contains(chargingCabinet, park.GetAllItems());
    }

    // Сценарий 4: Расчет суммарного энергопотребления
    [Fact]
    public void EnergyCalculator_ShouldSumOnlyEnergyConsumers()
    {
        var calculator = new EnergyCalculator();

        // Собираем парк устройств
        var items = new IInventoryItem[]
        {
            new Scooter("SC-001", "Scooter", 1.5, 8),                // 1.5
            new ElectricBike("EB-001", "E-Bike", 0.8, 7),            // 0.8
            new Bicycle("BC-001", "Bike", 9),                        // 0
            new Helmet("HL-001", "Helmet", 9),                       // 0
            new DockingStation("DS-001", "Dock", 15.5),              // 15.5
            new ChargingCabinet("CC-001", "Шкаф для зарядки", 20.0)  // 20.0
        };

        // Фильтруем и считаем (как это делает реальный раннер)
        double total = calculator.CalculateTotalDailyConsumption(
            items.OfType<IEnergyConsumer>()
        );

        // 1.5 + 0.8 + 15.5 + 20.0 = 37.8
        Assert.Equal(37.8, total, precision: 2);
    }

    // Сценарий 5: Фильтрация списка для новичков
    [Fact]
    public void Park_BeginnerFriendlyFilter_ShouldReturnOnlySimpleVehicles()
    {
        var park = new Park(new FakeApprovingServiceCenter());

        park.AddVehicle(new Scooter("SC-001", "Easy Scooter", 1.0, 8)); // Подходит (>= 6)
        park.AddVehicle(new Bicycle("BC-001", "Hard Bike", 4));         // Не подходит (< 6)
        park.AddItem(new Helmet("HL-001", "Helmet", 9));                // Не транспорт

        var beginners = park.GetAllItems()
            .OfType<IVehicle>()
            .OfType<IBeginnerFriendly>()
            .Where(v => v.SimplicityScore >= 6)
            .ToList();

        Assert.Single(beginners);
        Assert.Equal("Easy Scooter", beginners[0].Name);
    }

    // Валидация доменной модели (защита от некорректных данных)
    [Theory]
    [InlineData("", "Name", 1.0, 5)]    // Пустой номер
    [InlineData("SC-01", "Name", 1.0, 0)] // Оценка < 1
    [InlineData("SC-01", "Name", 1.0, 11)] // Оценка > 10
    [InlineData("SC-01", "Name", -5.0, 5)] // Отрицательная энергия
    public void Scooter_InvalidData_ShouldThrowArgumentException(
        string invNum, string name, double energy, int score)
    {
        Assert.Throws<ArgumentException>(() =>
            new Scooter(invNum, name, energy, score));
    }
}
