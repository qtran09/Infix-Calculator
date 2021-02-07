using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator calc = new Calculator();
            string input;
            Console.WriteLine("To exit application, enter EXIT or an empty message");
            while (true)
            {
                Console.Write("Expression to calculate: ");
                if ((input = Console.ReadLine()).Equals("EXIT") || string.IsNullOrEmpty(input)) break;
                string res = calc.Calculate(input);
                Console.WriteLine(input + " = " + res);
            }
        }
    }
}
