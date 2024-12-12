using System.Text.Json;

namespace Computer.Calc;

public class Calculator
{
    private const string HistoryFile = "calculator_history.json";
    private const int MaxHistoryItems = 25;
    private List<CalculationHistory> _history;

    public Calculator()
    {
        LoadHistory();
    }

    private void LoadHistory()
    {
        if (File.Exists(HistoryFile))
        {
            string jsonString = File.ReadAllText(HistoryFile);
            _history = JsonSerializer.Deserialize<List<CalculationHistory>>(jsonString) ?? new List<CalculationHistory>();
        }
        else
        {
            _history = new List<CalculationHistory>();
        }
    }

    private void SaveHistory()
    {
        string jsonString = JsonSerializer.Serialize(_history);
        File.WriteAllText(HistoryFile, jsonString);
    }

    private void AddToHistory(string operation, double num1, double num2, double result)
    {
        var calculation = new CalculationHistory
        {
            Operation = operation,
            Number1 = num1,
            Number2 = num2,
            Result = result,
            Timestamp = DateTime.Now
        };

        _history.Add(calculation);
        if (_history.Count > MaxHistoryItems)
        {
            _history.RemoveAt(0);
        }
        SaveHistory();
    }

    public void ShowHistory()
    {
        Console.WriteLine("\nCalculation History (Last 25 operations):");
        Console.WriteLine("----------------------------------------");
        foreach (var calc in _history.OrderByDescending(h => h.Timestamp))
        {
            Console.WriteLine(calc);
        }
        Console.WriteLine("----------------------------------------\n");
    }

    public double Add(double num1, double num2)
    {
        double result = num1 + num2;
        AddToHistory("+", num1, num2, result);
        return result;
    }

    public double Subtract(double num1, double num2)
    {
        double result = num1 - num2;
        AddToHistory("-", num1, num2, result);
        return result;
    }

    public double Multiply(double num1, double num2)
    {
        double result = num1 * num2;
        AddToHistory("*", num1, num2, result);
        return result;
    }

    public double Divide(double num1, double num2)
    {
        if (num2 == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero!");
        }
        double result = num1 / num2;
        AddToHistory("/", num1, num2, result);
        return result;
    }
}
