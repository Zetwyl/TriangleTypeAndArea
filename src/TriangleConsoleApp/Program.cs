using System;
using TriangleTypeAndArea;

// Используем псевдонимы для ясности, хотя в консольном проекте это не обязательно.
using Calculator = TriangleTypeAndArea.TriangleCalculator;

namespace TriangleConsoleApp
{
    public class Program
    {
        /// <summary>
        /// Безопасно запрашивает у пользователя ввод положительной стороны треугольника.
        /// Использует цикл для повторного запроса при некорректном вводе.
        /// </summary>
        /// <param name="sideName">Название стороны для отображения в подсказке (например, "A").</param>
        /// <returns>Валидное значение стороны типа decimal.</returns>
        public static decimal GetSideInput(string sideName)
        {
            decimal side;
            bool isValid = false;

            do
            {
                Console.Write($"Введите длину стороны {sideName} (должна быть > 0): ");
                string input = Console.ReadLine();

                // Проверка, что ввод является числом (decimal)
                if (decimal.TryParse(input, out side))
                {
                    // Проверка, что число положительное
                    if (side > 0)
                    {
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Длина стороны должна быть строго положительным числом.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: Некорректный ввод. Пожалуйста, введите число.");
                }
            }
            while (!isValid);

            return side;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("--- Калькулятор Треугольника ---");

            // 1. Сбор данных от пользователя
            decimal a = GetSideInput("A");
            decimal b = GetSideInput("B");
            decimal c = GetSideInput("C");

            Console.WriteLine("\n--- Результаты расчетов ---");

            // 2. Валидация
            if (TriangleValidator.IsValid(a, b, c))
            {
                // 3. Вычисление типа
                string triangleType = Calculator.GetTypeTriangle(a, b, c);
                Console.WriteLine($"Тип треугольника: {triangleType}");

                // 4. Вычисление площади
                double area = Calculator.GetAreaTriangle(a, b, c);
                Console.WriteLine($"Площадь треугольника: {area:F4}");
            }
            else
            {
                // Обработка случая, когда стороны не образуют треугольник
                Console.WriteLine("Внимание: Введенные стороны не могут образовать валидный треугольник.");
                Console.WriteLine("Пожалуйста, проверьте, что все стороны положительны и выполняется неравенство треугольника (сумма двух сторон всегда больше третьей).");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}