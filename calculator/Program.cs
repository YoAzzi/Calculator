using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorApp
{
    // Interface pour définir le comportement des différentes opérations
    public interface IOperation
    {
        double Operate(IEnumerable<double> numbers);
    }

    // Opération par défaut : somme
    public class SumOperation : IOperation
    {
        public double Operate(IEnumerable<double> numbers)
        {
            return numbers.Sum();
        }
    }

    // Opération de multiplication
    public class MultiplyOperation : IOperation
    {
        public double Operate(IEnumerable<double> numbers)
        {
            double result = 1;
            foreach (var num in numbers)
            {
                result *= num;
            }
            return result;
        }
    }

    // Calculator class
    public class Calculator
    {
        private readonly IOperation _operation;
        private readonly char _separator;

        // Constructeur avec injection de dépendance pour spécifier l'opération et le séparateur
        public Calculator(IOperation operation, char separator = ',')
        {
            _operation = operation ?? new SumOperation();
            _separator = separator;
        }

        // Méthode pour calculer
        public double Calculate(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            var numbers = input.Split(_separator)
                               .Where(s => !string.IsNullOrWhiteSpace(s))
                               .Select(s =>
                               {
                                   if (double.TryParse(s, out double number))
                                       return number;
                                   return 0; // or handle invalid input differently
                               });

            return _operation.Operate(numbers);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            // Exemple d'utilisation
            Calculator calculator = new Calculator(new SumOperation(), ',');
            Console.WriteLine(calculator.Calculate("1, 2, 3"));  // Output: 6
            Console.WriteLine(calculator.Calculate("0, , "));   // Output: 0
            Console.WriteLine(calculator.Calculate("1, -1, 9")); // Output: 9
            Console.WriteLine(calculator.Calculate(null));      // Output: 0
            Console.WriteLine(calculator.Calculate("0, null, 2")); // Output: 2
        }
    }
}
