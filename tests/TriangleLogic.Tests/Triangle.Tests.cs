// Используем псевдоним для класса, чтобы избежать конфликтов при перемещении логики
using Calculator = TriangleTypeAndArea.TriangleCalculator;
using Validator = TriangleTypeAndArea.TriangleValidator;

namespace TriangleTests
{
    // Класс для тестирования
    [TestClass]
    public class TriangleLogicTests
    {
        // Допустимая погрешность для сравнения площади
        private const double AreaTolerance = 0.0001;

        // -------------------------------------------------------------------
        // ГРУППА 1: Проверка класса валидации (TriangleValidator)
        // -------------------------------------------------------------------

        [TestMethod]
        [DataRow(10.0, 10.0, 10.0, true, DisplayName = "Validator_01_SidesPositive (Все стороны > 0)")]
        [DataRow(-1.0, 5.0, 5.0, false, DisplayName = "Validator_01_SidesNegative (Отрицательная сторона)")]
        [DataRow(0.0, 5.0, 5.0, false, DisplayName = "Validator_01_SidesZero (Нулевая сторона)")]
        public void Validator_01_AreSidesPositive_Check(double a, double b, double c, bool expected)
        {
            // Act: Тестируем AreSidesPositive
            bool actual = Validator.AreSidesPositive((decimal)a, (decimal)b, (decimal)c);

            // Assert: Проверяет, что стороны {a}, {b}, {c} правильно определяются как положительные.
            Assert.AreEqual(expected, actual,
                $"Провал в тесте положительности сторон ({a}, {b}, {c}). Ожидалось: {expected}, Получено: {actual}. Стороны должны быть > 0.");
        }

        [TestMethod]
        [DataRow(3.0, 4.0, 5.0, true, DisplayName = "Validator_02_Inequality (Проверка: 3+4 > 5)")]
        [DataRow(1.0, 2.0, 10.0, false, DisplayName = "Validator_02_Violation (Проверка: 1+2 < 10)")]
        [DataRow(5.0, 5.0, 10.0, false, DisplayName = "Validator_02_Degenerate (Проверка: 5+5 = 10, должно быть false)")] // Вырожденный (строгое неравенство)
        public void Validator_02_DoesInequalityHold_Check(double a, double b, double c, bool expected)
        {
            // Act: Тестируем DoesInequalityHold
            bool actual = Validator.DoesInequalityHold((decimal)a, (decimal)b, (decimal)c);

            // Assert: Проверяет выполнение строгого неравенства треугольника (сумма двух сторон > третьей).
            Assert.AreEqual(expected, actual,
                $"Провал неравенства треугольника для сторон ({a}, {b}, {c}). Ожидалось: {expected}, Получено: {actual}.");
        }

        [TestMethod]
        [DataRow(3.0, 4.0, 5.0, true, DisplayName = "Validator_03_IsValid (Валидный ввод 3,4,5)")]
        [DataRow(-1.0, 5.0, 5.0, false, DisplayName = "Validator_03_IsInvalid (Отрицательная сторона)")]
        [DataRow(1.0, 2.0, 10.0, false, DisplayName = "Validator_03_IsInvalid (Нарушение неравенства)")]
        public void Validator_03_IsValid_Composite_Check(double a, double b, double c, bool expected)
        {
            // Act: Тестируем IsValid (композитный метод)
            bool actual = Validator.IsValid((decimal)a, (decimal)b, (decimal)c);

            // Assert: Проверяет общую валидацию.
            Assert.AreEqual(expected, actual,
                $"Провал общей валидации IsValid для ({a}, {b}, {c}). Ожидалось: {expected}, Получено: {actual}.");
        }

        // -------------------------------------------------------------------
        // ГРУППА 2: Проверка всех типов треугольников (GetTypeTriangle)
        // -------------------------------------------------------------------

        [TestMethod]
        // Разносторонние
        [DataRow(10.0, 11.0, 12.0, "Остроугольный", DisplayName = "Calculator_01_Type (Разносторонний Остроугольный)")]
        [DataRow(2.0, 3.0, 4.0, "Тупоугольный", DisplayName = "Calculator_01_Type (Разносторонний Тупоугольный)")]
        // Прямоугольные
        [DataRow(3.0, 4.0, 5.0, "Прямоугольный", DisplayName = "Calculator_01_Type (Прямоугольный 3-4-5)")]
        [DataRow(5.0, 3.0, 4.0, "Прямоугольный", DisplayName = "Calculator_01_Type (Прямоугольный несортированный)")]
        // Равнобедренные
        [DataRow(10.0, 10.0, 10.0, "Остроугольный", DisplayName = "Calculator_01_Type (Равносторонний/Остроугольный)")]
        [DataRow(5.0, 5.0, 6.0, "Остроугольный", DisplayName = "Calculator_01_Type (Равнобедренный Остроугольный)")]
        [DataRow(5.0, 5.0, 8.0, "Тупоугольный", DisplayName = "Calculator_01_Type (Равнобедренный Тупоугольный)")]
        // Проверка допусков (RightAngleTolerance)
        [DataRow(3.0, 4.0, 5.00000005, "Прямоугольный", DisplayName = "Calculator_01_Type (Допуск: Попадание в Tolerance)")]
        [DataRow(3.0, 4.0, 5.001, "Тупоугольный", DisplayName = "Calculator_01_Type (Допуск: Промах, классификация: Тупоугольный)")]
        public void Calculator_01_TypeClassification_AllValidTypes(double a, double b, double c, string expectedType)
        {
            // ACT: Проверяем, что для валидных данных корректно определяется тип.
            string actualType = Calculator.GetTypeTriangle((decimal)a, (decimal)b, (decimal)c);
            Assert.AreEqual(expectedType, actualType);
        }

        // -------------------------------------------------------------------
        // ГРУППА 3: Проверка Площади (GetAreaTriangle)
        // (Покрытие точности и переполнения)
        // -------------------------------------------------------------------

        [TestMethod]
        [DataRow(3.0, 4.0, 5.0, 6.0d, DisplayName = "Calculator_02_Area (Прямоугольный)")]
        [DataRow(10.0, 10.0, 10.0, 43.30127d, DisplayName = "Calculator_02_Area (Равносторонний)")]
        [DataRow(2.0, 3.0, 4.0, 2.9047375d, DisplayName = "Calculator_02_Area (Разносторонний)")]
        public void Calculator_02_AreaCalculation_Valid(double a, double b, double c, double expectedArea)
        {
            // ACT: Проверяем, что площадь вычисляется с заданной точностью.
            double actualArea = Calculator.GetAreaTriangle((decimal)a, (decimal)b, (decimal)c);
            Assert.AreEqual(expectedArea, actualArea, AreaTolerance);
        }

        [TestMethod]
        [DataRow(1E14, 1E14, 1E14, DisplayName = "Calculator_03_AreaOverflow (Вызывает Decimal Overflow)")]
        public void Calculator_03_AreaCalculation_Overflow_ExpectsNaN(double a, double b, double c)
        {
            // ACT: Проверяем, что при переполнении типа decimal возвращается NaN.
            double actualArea = Calculator.GetAreaTriangle((decimal)a, (decimal)b, (decimal)c);
            Assert.IsTrue(double.IsNaN(actualArea));
        }

        // -------------------------------------------------------------------
        // ГРУППА 4: Тестирование обработки невалидного ввода
        // -------------------------------------------------------------------

        [TestMethod]
        // Наборы данных для проверки работы защитного механизма на невалидных треугольниках.
        [DataRow(1.0, 2.0, 10.0, "Невалидный", double.NaN, DisplayName = "Calculator_04_Invalid (Нарушение неравенства)")]
        [DataRow(-5.0, 5.0, 5.0, "Невалидный", double.NaN, DisplayName = "Calculator_04_Invalid (Отрицательная сторона)")]
        [DataRow(5.0, 5.0, 10.0, "Невалидный", double.NaN, DisplayName = "Calculator_04_Invalid (Вырожденный)")]
        [DataRow(0.0, 5.0, 5.0, "Невалидный", double.NaN, DisplayName = "Calculator_04_Invalid (Нулевая сторона)")]
        public void Calculator_04_InvalidInput_ExpectsInvalidAndNaN(double a, double b, double c, string expectedType, double expectedArea)
        {
            // ACT 1: Проверка типа
            string actualType = Calculator.GetTypeTriangle((decimal)a, (decimal)b, (decimal)c);
            Assert.AreEqual(expectedType, actualType, "Ошибка в возвращаемом типе.");

            // ACT 2: Проверка площади
            double actualArea = Calculator.GetAreaTriangle((decimal)a, (decimal)b, (decimal)c);
            Assert.IsTrue(double.IsNaN(actualArea), "Ошибка в возвращаемой площади (должно быть NaN).");
        }
    }
}