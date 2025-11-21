namespace TriangleTypeAndArea
{
    public static class TriangleCalculator
    {
        // Допуск для сравнения чисел с плавающей точкой (для определения прямоугольного треугольника).
        private const double RightAngleTolerance = 0.000001;

        /// <summary>
        /// Определяет тип треугольника (Остроугольный, Тупоугольный, Прямоугольный).
        /// </summary>
        public static string GetTypeTriangle(decimal a, decimal b, decimal c)
        {
            if (!TriangleValidator.IsValid(a, b, c))
            {
                return "Невалидный";
            }

            try
            {
                // Необходимо, чтобы sideC всегда была самой длинной стороной для корректной проверки по Пифагору.
                decimal[] sides = { a, b, c };
                Array.Sort(sides);

                decimal sideA = sides[0];
                decimal sideB = sides[1];
                decimal sideC = sides[2];

                // При сторонах > ~10^14 произойдет OverflowException.
                double c2 = Math.Pow((double)sideC, 2);
                double ab2Sum = Math.Pow((double)sideA, 2) + Math.Pow((double)sideB, 2);

                // Проверка на прямоугольный: a^2 + b^2 = c^2 (с допуском)
                if (Math.Abs(ab2Sum - c2) < RightAngleTolerance)
                {
                    return "Прямоугольный";
                }
                // Проверка на тупоугольный: a^2 + b^2 < c^2
                else if (ab2Sum < c2)
                {
                    return "Тупоугольный";
                }
                // Остальное — остроугольный: a^2 + b^2 > c^2
                else
                {
                    return "Остроугольный";
                }
            }
            catch (OverflowException)
            {
                // Ловим переполнение, если входные числа настолько велики, что вызывают сбой Math.Pow.
                return "Невозможно определить (слишком большие числа)";
            }
        }

        /// <summary>
        /// Вычисляет площадь треугольника по формуле Герона.
        /// Возвращает double.NaN, если треугольник невалиден или произошло переполнение.
        /// </summary>
        public static double GetAreaTriangle(decimal a, decimal b, decimal c)
        {
            if (!TriangleValidator.IsValid(a, b, c))
            {
                return double.NaN;
            }

            try
            {
                // 1. Полупериметр (в decimal)
                decimal pDecimal = (a + b + c) / 2m;

                // 2. Множители формулы Герона (в decimal, для максимальной точности)
                decimal factorA = pDecimal - a;
                decimal factorB = pDecimal - b;
                decimal factorC = pDecimal - c;

                // 3. Произведение под корнем (в decimal)
                // Произведение выполняется в decimal для максимальной точности.
                decimal valueUnderRootDecimal = pDecimal * factorA * factorB * factorC;

                // 4. Преобразование результата в double
                // valueUnderRootDecimal преобразуется в double только для вызова Math.Sqrt().
                double valueUnderRootDouble = (double)valueUnderRootDecimal;

                // Используем Math.Max(0, ...) для защиты от ошибок округления:
                // Если из-за потери точности подкоренное выражение стало минимально отрицательным 
                // (хотя должно быть >= 0), Math.Max(0, ...) гарантирует, что Math.Sqrt() не сгенерирует NaN.
                double areaDouble = Math.Sqrt(Math.Max(0, valueUnderRootDouble));

                return areaDouble;
            }
            catch (OverflowException)
            {
                // Ловим переполнение, если произведение valueUnderRootDecimal превысило диапазон decimal (7.9 * 10^28).
                return double.NaN;
            }
        }
    }
}
