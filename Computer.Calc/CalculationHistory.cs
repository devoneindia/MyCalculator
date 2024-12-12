namespace Computer.Calc;

public class CalculationHistory
{
    public string Operation { get; set; }
    public double Number1 { get; set; }
    public double Number2 { get; set; }
    public double Result { get; set; }
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return $"{Timestamp:yyyy-MM-dd HH:mm:ss} | {Number1} {Operation} {Number2} = {Result}";
    }
}
