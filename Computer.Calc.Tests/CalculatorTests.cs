using System.Text.Json;

namespace Computer.Calc.Tests;

public class CalculatorTests : IDisposable
{
    private readonly Calculator _calculator;
    private const string TestHistoryFile = "calculator_history.json";

    public CalculatorTests()
    {
        // Setup - Clean any existing test file before each test
        if (File.Exists(TestHistoryFile))
        {
            File.Delete(TestHistoryFile);
        }
        _calculator = new Calculator();
    }

    public void Dispose()
    {
        // Cleanup - Remove test file after each test
        if (File.Exists(TestHistoryFile))
        {
            File.Delete(TestHistoryFile);
        }
    }

    [Fact]
    public void Add_TwoNumbers_ReturnsCorrectSum()
    {
        // Arrange
        double num1 = 5;
        double num2 = 3;
        double expected = 8;

        // Act
        double result = _calculator.Add(num1, num2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Subtract_TwoNumbers_ReturnsCorrectDifference()
    {
        // Arrange
        double num1 = 10;
        double num2 = 4;
        double expected = 6;

        // Act
        double result = _calculator.Subtract(num1, num2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Multiply_TwoNumbers_ReturnsCorrectProduct()
    {
        // Arrange
        double num1 = 6;
        double num2 = 7;
        double expected = 42;

        // Act
        double result = _calculator.Multiply(num1, num2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Divide_TwoNumbers_ReturnsCorrectQuotient()
    {
        // Arrange
        double num1 = 20;
        double num2 = 5;
        double expected = 4;

        // Act
        double result = _calculator.Divide(num1, num2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        double num1 = 10;
        double num2 = 0;

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide(num1, num2));
    }

    [Fact]
    public void History_IsPersistedToFile()
    {
        // Arrange
        double num1 = 10;
        double num2 = 5;

        // Act
        _calculator.Add(num1, num2);

        // Assert
        Assert.True(File.Exists(TestHistoryFile));
        string jsonContent = File.ReadAllText(TestHistoryFile);
        var history = JsonSerializer.Deserialize<List<CalculationHistory>>(jsonContent);
        Assert.NotNull(history);
        Assert.Single(history);
        Assert.Equal("+", history[0].Operation);
        Assert.Equal(num1, history[0].Number1);
        Assert.Equal(num2, history[0].Number2);
        Assert.Equal(15, history[0].Result);
    }

    [Fact]
    public void History_LimitsTo25Items()
    {
        // Arrange & Act
        for (int i = 0; i < 30; i++)
        {
            _calculator.Add(i, 1);
        }

        // Assert
        string jsonContent = File.ReadAllText(TestHistoryFile);
        var history = JsonSerializer.Deserialize<List<CalculationHistory>>(jsonContent);
        Assert.Equal(25, history.Count);
        // Verify the oldest entries were removed
        Assert.Equal(5, history.First().Number1); // First entry should be the 6th operation
    }

    [Fact]
    public void ShowHistory_OutputsCorrectFormat()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        _calculator.Add(10, 5);

        // Act
        _calculator.ShowHistory();
        string output = stringWriter.ToString();

        // Assert
        Assert.Contains("Calculation History (Last 25 operations):", output);
        Assert.Contains("10 + 5 = 15", output);
    }

    [Fact]
    public void Calculator_LoadsExistingHistory()
    {
        // Arrange
        _calculator.Add(10, 5);

        // Act
        var newCalculator = new Calculator(); // This should load the existing history
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        newCalculator.ShowHistory();
        string output = stringWriter.ToString();

        // Assert
        Assert.Contains("10 + 5 = 15", output);
    }

    [Fact]
    public void CalculationHistory_ToStringReturnsCorrectFormat()
    {
        // Arrange
        var history = new CalculationHistory
        {
            Operation = "+",
            Number1 = 10,
            Number2 = 5,
            Result = 15,
            Timestamp = new DateTime(2024, 1, 1, 12, 0, 0)
        };

        // Act
        string result = history.ToString();

        // Assert
        Assert.Equal("2024-01-01 12:00:00 | 10 + 5 = 15", result);
    }

    [Theory]
    [InlineData(double.MaxValue, 1)]
    [InlineData(double.MinValue, -1)]
    [InlineData(0, 0)]
    public void Calculator_HandlesEdgeCases(double num1, double num2)
    {
        // Act & Assert - Should not throw exceptions
        _calculator.Add(num1, num2);
        _calculator.Subtract(num1, num2);
        _calculator.Multiply(num1, num2);
        if (num2 != 0)
        {
            _calculator.Divide(num1, num2);
        }
    }
}