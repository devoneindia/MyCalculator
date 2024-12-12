namespace Computer.Calc;

class Program
{
    static void Main(string[] args)
    {
        var calculator = new Calculator();
        bool running = true;

        while (running)
        {
            Console.WriteLine("Simple Calculator");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Subtract");
            Console.WriteLine("3. Multiply");
            Console.WriteLine("4. Divide");
            Console.WriteLine("5. Show History");
            Console.WriteLine("6. Exit");
            Console.Write("\nChoose an option (1-6): ");

            string? choice = Console.ReadLine();

            if (choice == "6")
            {
                running = false;
                continue;
            }

            if (choice == "5")
            {
                calculator.ShowHistory();
                continue;
            }

            if (choice is "1" or "2" or "3" or "4")
            {
                try
                {
                    Console.Write("Enter first number: ");
                    if (!double.TryParse(Console.ReadLine(), out double num1))
                    {
                        Console.WriteLine("Invalid number. Please try again.");
                        continue;
                    }

                    Console.Write("Enter second number: ");
                    if (!double.TryParse(Console.ReadLine(), out double num2))
                    {
                        Console.WriteLine("Invalid number. Please try again.");
                        continue;
                    }

                    double result = choice switch
                    {
                        "1" => calculator.Add(num1, num2),
                        "2" => calculator.Subtract(num1, num2),
                        "3" => calculator.Multiply(num1, num2),
                        "4" => calculator.Divide(num1, num2),
                        _ => throw new ArgumentException("Invalid choice")
                    };

                    Console.WriteLine($"\nResult: {result}\n");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("\nError: Cannot divide by zero!\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nAn error occurred: {ex.Message}\n");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid option. Please try again.\n");
            }
        }
    }
}