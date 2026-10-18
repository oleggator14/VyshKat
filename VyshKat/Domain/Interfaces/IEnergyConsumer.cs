namespace VyshKat.Domain.Interfaces;

public interface IEnergyConsumer  // Интерфейс для сущностей, потребляющих энергию
{
    double DailyEnergyConsumption { get; } // Ежедневное потребление энергии в киловатт-часах (кВт·ч)
}
