using Xunit;
using System;
using System.IO;
using Computer.Calc;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Computer.Calc.Tests;

public class ProgramTests : IDisposable
{
    private readonly StringWriter _outputWriter;
    private readonly TextWriter _originalOutput;
    private readonly TextReader _originalInput;

    public ProgramTests()
    {
        // Setup - Save original console I/O and redirect output
        _originalOutput = Console.Out;
        _originalInput = Console.In;
        _outputWriter = new StringWriter();
        Console.SetOut(_outputWriter);
    }

    public void Dispose()
    {
        // Cleanup - Restore original console I/O
        Console.SetOut(_originalOutput);
        Console.SetIn(_originalInput);
        _outputWriter.Dispose();
    }

    private void SimulateUserInput(string input)
    {
        var stringReader = new StringReader(input);
        Console.SetIn(stringReader);
    }

    [Fact]
    public void Main_Exit_TerminatesProgram()
    {
        // Arrange
        SimulateUserInput("6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Simple Calculator", output);
        Assert.DoesNotContain("Invalid option", output);
    }

    [Fact]
    public void Main_InvalidOption_ShowsErrorMessage()
    {
        // Arrange
        SimulateUserInput("invalid\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Invalid option", output);
    }

    [Fact]
    public void Main_Addition_CalculatesCorrectly()
    {
        // Arrange
        SimulateUserInput("1\n5\n3\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Result: 8", output);
    }

    [Fact]
    public void Main_Subtraction_CalculatesCorrectly()
    {
        // Arrange
        SimulateUserInput("2\n10\n4\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Result: 6", output);
    }

    [Fact]
    public void Main_Multiplication_CalculatesCorrectly()
    {
        // Arrange
        SimulateUserInput("3\n6\n7\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Result: 42", output);
    }

    [Fact]
    public void Main_Division_CalculatesCorrectly()
    {
        // Arrange
        SimulateUserInput("4\n15\n3\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Result: 5", output);
    }

    [Fact]
    public void Main_DivideByZero_ShowsErrorMessage()
    {
        // Arrange
        SimulateUserInput("4\n10\n0\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Cannot divide by zero", output);
    }

    [Fact]
    public void Main_InvalidFirstNumber_ShowsErrorMessage()
    {
        // Arrange
        SimulateUserInput("1\ninvalid\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Invalid number", output);
    }

    [Fact]
    public void Main_InvalidSecondNumber_ShowsErrorMessage()
    {
        // Arrange
        SimulateUserInput("1\n5\ninvalid\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Invalid number", output);
    }

    [Fact]
    public void Main_ShowHistory_DisplaysCalculationHistory()
    {
        // Arrange
        SimulateUserInput("1\n5\n5\n5\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Calculation History", output);
        Assert.Contains("5 + 5 = 10", output);
    }

    [Fact]
    public void Main_MultipleOperations_ExecutesSequentially()
    {
        // Arrange
        SimulateUserInput("1\n5\n5\n2\n10\n3\n6\n");

        // Act
        Program.Main(new string[] { });

        // Assert
        string output = _outputWriter.ToString();
        Assert.Contains("Result: 10", output); // First operation
        Assert.Contains("Result: 7", output);  // Second operation
    }
}