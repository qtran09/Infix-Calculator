using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Commands: " + args[0] + "," + args[1]);
            Calculator calc = new Calculator();
            string input;
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
