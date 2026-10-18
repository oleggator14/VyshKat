namespace VyshKat.Domain.Interfaces;

public interface IInventoryItem  // Интерфейс для предмета в инвентаре
{
    string InventoryNumber { get; }  // Уникальный идентификатор предмета в инвентаре
    string Name { get; }             // Полное наименование предмета
}
