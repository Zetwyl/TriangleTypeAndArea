namespace TriangleTypeAndArea
{
    // Класс для проверки корректности сторон треугольника
    public static class TriangleValidator
    {

        public static bool AreSidesPositive(decimal a, decimal b, decimal c)
        {
            // Проверка на положительность сторон
            return a > 0 && b > 0 && c > 0;
        }

        public static bool DoesInequalityHold(decimal a, decimal b, decimal c)
        {
            // Проверка неравенства треугольника (строгое неравенство, исключает вырожденные треугольники)
            return a + b > c && a + c > b && c + b > a;
        }

        public static bool IsValid(decimal a, decimal b, decimal c)
        {
            // Здесь происходит композиция: вызов двух отдельных методов
            return AreSidesPositive(a, b, c) && DoesInequalityHold(a, b, c);
        }
    }
}