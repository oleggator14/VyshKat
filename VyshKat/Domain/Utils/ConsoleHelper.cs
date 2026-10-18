namespace VyshKat.Domain.Utils;

public static class ConsoleHelper  // Методы помощи работы с консолью
                                   // Методы чтения реализуют корректную работу завершения программы через Ctrl+C
{
    public static double ReadDouble()  // Считывает число с плавающей точкой из консоли с повторным запросом при ошибке
    {
        while (true)
        {
            string? input = Console.ReadLine();

            // Если input == null, значит был Ctrl+C.
            if (input == null)
            {
                Console.WriteLine("\nРабота прервана пользователем.");
                Environment.Exit(0);  // Корректное завершение из любой точки программы
            }

            if (double.TryParse(input, out double value))
                return value;
            Console.Write("❌ Некорректное число. Попробуйте снова: ");
        }
    }

    public static int ReadInt()  // Считывает целое число из консоли с повторным запросом при ошибке
    {
        while (true)
        {
            string? input = Console.ReadLine();

            // Если input == null, значит был Ctrl+C.
            if (input == null)
            {
                Console.WriteLine("\nРабота прервана пользователем.");
                Environment.Exit(0);  // Корректное завершение из любой точки программы
            }

            if (int.TryParse(input, out int value))
                return value;
            Console.Write("❌ Некорректное число. Попробуйте снова: ");
        }
    }

    public static string ReadString()
    {
        string? input = Console.ReadLine();

        if (input == null) // Ctrl+C
        {
            Console.WriteLine("\nРабота прервана пользователем.");
            Environment.Exit(0);
        }

        return input;
    }

    public static void WaitForAnyKey()  // Ожидание ввода для возврата в меню
    {
        Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
        Console.ReadKey(true);  // true = скрыть ввод
        Console.WriteLine("\n" + new string('-', 40));
    }
}
