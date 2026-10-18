namespace VyshKat.Domain.Interfaces;

public interface IBeginnerFriendly : IInventoryItem  // Интерфейс для оценки простоты, наследуем от IInventoryItem
{
    int SimplicityScore { get; }  // Оценка простоты
}
