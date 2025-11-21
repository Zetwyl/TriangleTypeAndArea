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
        // ГРУППА 2: Проверка Площади (GetAreaTriangle)
        // -------------------------------------------------------------------

        [TestMethod]
        [DataRow(3.0, 4.0, 5.0, 6.0d, DisplayName = "Calculator_01_Area (Прямоугольный 3-4-5)")] // Прямоугольный
        [DataRow(10.0, 10.0, 10.0, 43.30127d, DisplayName = "Calculator_01_Area (Равносторонний 10-10-10)")] // Равносторонний
        [DataRow(2.0, 3.0, 4.0, 2.9047375d, DisplayName = "Calculator_01_Area (Разносторонний 2-3-4)")] // Разносторонний
        public void Calculator_01_AreaCalculation_Valid(double a, double b, double c, double expectedArea)
        {
            // Act
            double actualArea = Calculator.GetAreaTriangle((decimal)a, (decimal)b, (decimal)c);

            // Assert: Проверяет, что фактическая площадь совпадает с ожидаемой в пределах AreaTolerance.
            Assert.AreEqual(expectedArea, actualArea, AreaTolerance,
                 $"Ошибка площади для сторон ({a}, {b}, {c}). Ожидалось успешное вычисление площади: {expectedArea:F5}, Фактически: {actualArea:F5}.");
        }

        [TestMethod]
        [DataRow(1.0, 2.0, 10.0, DisplayName = "Calculator_02_Area (Невалидный - ожидается NaN)")]
        [DataRow(-5.0, 5.0, 5.0, DisplayName = "Calculator_02_Area (Отрицательная сторона - ожидается NaN)")]
        [DataRow(0.0, 5.0, 5.0, DisplayName = "Calculator_02_Area (Нулевая сторона - ожидается NaN)")]
        public void Calculator_02_AreaCalculation_Invalid_ExpectsNaN(double a, double b, double c)
        {
            // Act
            double actualArea = Calculator.GetAreaTriangle((decimal)a, (decimal)b, (decimal)c);

            // Assert: Проверяем, что для невалидного ввода возвращается double.NaN.
            Assert.IsTrue(double.IsNaN(actualArea),
                $"Площадь должна быть NaN для невалидного треугольника ({a},{b},{c}), но получено число: {actualArea}.");
        }


        // -------------------------------------------------------------------
        // ГРУППА 3: Проверка Типа (GetTypeTriangle)
        // -------------------------------------------------------------------

        [TestMethod]
        [DataRow(3.0, 4.0, 5.0, "Прямоугольный", DisplayName = "Calculator_03_Type (Прямоугольный 3-4-5)")]
        [DataRow(10.0, 10.0, 10.0, "Остроугольный", DisplayName = "Calculator_03_Type (Остроугольный/Равносторонний 10-10-10)")]
        [DataRow(2.0, 3.0, 4.0, "Тупоугольный", DisplayName = "Calculator_03_Type (Тупоугольный 2-3-4)")]
        [DataRow(1.0, 2.0, 10.0, "Невалидный", DisplayName = "Calculator_03_Type (Невалидный ввод)")]
        [DataRow(5.0, 3.0, 4.0, "Прямоугольный", DisplayName = "Calculator_03_Type (Прямоугольный несортированный)")]
        [DataRow(7.07106781, 7.07106781, 10.0, "Прямоугольный", DisplayName = "Calculator_03_Type (Равнобедренный Прямоугольный с √2)")]
        public void Calculator_03_TypeClassification(double a, double b, double c, string expectedType)
        {
            // Act
            string actualType = Calculator.GetTypeTriangle((decimal)a, (decimal)b, (decimal)c);

            // Assert: Проверяем, что возвращаемый тип соответствует ожидаемому.
            Assert.AreEqual(expectedType, actualType,
                 $"Ошибка определения типа для сторон ({a}, {b}, {c}). Ожидалось успешное определение как '{expectedType}', Фактически: '{actualType}'.");
        }

        // -------------------------------------------------------------------
        // ГРУППА 4: Тесты крайних случаев и переполнения (OverflowException)
        // -------------------------------------------------------------------

        [TestMethod]
        [DataRow(1E14, 1E14, 1E14, DisplayName = "Calculator_04_AreaOverflow (Равносторонний, вызывает Decimal Overflow)")]
        [DataRow(7E13, 8E13, 9E13, DisplayName = "Calculator_04_AreaOverflow (Разносторонний, вызывает Decimal Overflow)")]
        public void Calculator_04_AreaCalculation_Overflow_ExpectsNaN(double a, double b, double c)
        {
            // Act
            decimal decimalA = (decimal)a;
            decimal decimalB = (decimal)b;
            decimal decimalC = (decimal)c;

            // Используем очень большие числа, которые приведут к OverflowException 
            // при умножении (S^2) в типе decimal.
            double actualArea = Calculator.GetAreaTriangle(decimalA, decimalB, decimalC);

            // Assert: Проверяем, что при переполнении decimal, GetAreaTriangle возвращает double.NaN.
            Assert.IsTrue(double.IsNaN(actualArea),
                $"Площадь должна быть NaN из-за переполнения decimal для ({a},{b},{c}), но получено: {actualArea}.");
        }
    }
}