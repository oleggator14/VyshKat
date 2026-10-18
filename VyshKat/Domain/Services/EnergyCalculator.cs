using VyshKat.Domain.Interfaces;

namespace VyshKat.Domain.Services;

public class EnergyCalculator  // Класс для расчета энергопотребления
{
    public double CalculateTotalDailyConsumption(IEnumerable<IEnergyConsumer> consumers)  // Метод подсчета общего ежедневного энергопотребления
    {
        return consumers.Sum(c => c.DailyEnergyConsumption);
    }
}
