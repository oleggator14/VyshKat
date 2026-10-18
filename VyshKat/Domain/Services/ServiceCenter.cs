using VyshKat.Domain.Interfaces;

namespace VyshKat.Domain.Services;

internal class ServiceCenter : IServiceCenter  // Класс сервисного центра, реализующий интерфейс IServiceCenter
{
    public bool Inspect(IVehicle vehicle)  // Метод для проведения инспекции ТС
    {
        if (vehicle == null)
        {
            return false; // Отклонен: vehicle is null
        }

        if (vehicle is IEnergyConsumer energyConsumer && energyConsumer.DailyEnergyConsumption > 100)
        {
            return false; // Отклонен: аномальное энергопотребление
        }

        // Место для дополнительных проверок
        return true;
    }
}
