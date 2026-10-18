namespace VyshKat.Domain.Interfaces;

public interface IServiceCenter  // Интерфейс для сервисного центра, предоставляющего услуги по допуску ТС в парк
{
    bool Inspect(IVehicle vehicle);  // Метод для проведения инспекции ТС - возвращает true, если инспекция успешна, иначе false
}
