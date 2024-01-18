using System;
using System.Collections.Generic;
using System.IO;

class RPNCalculator
{
    static void Main()
    {
        Console.WriteLine("Calculatrice en notation polonaise inverse (RPN)");

        // Demande à l'utilisateur d'entrer le chemin du fichier contenant l'expression RPN
        Console.WriteLine("Entrez le chemin du fichier contenant l'expression RPN :");
        string filePath = Console.ReadLine();

        try
        {
            string input = ReadExpressionFromFile(filePath);
            double result = CalculateRPN(input);
            Console.WriteLine("Résultat : " + result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }

    static string ReadExpressionFromFile(string filePath)
    {
        try
        {
            // Lit le contenu du fichier
            string expression = File.ReadAllText(filePath);
            return expression;
        }
        catch (FileNotFoundException ex)
        {
            throw new FileNotFoundException("Fichier non trouvé. Veuillez vérifier le chemin du fichier.", ex);
        }
        catch (FileLoadException ex)
        {
            throw new FileLoadException("Erreur lors de la lecture du fichier.", ex);
        }
    }

    static double CalculateRPN(string input)
    {
        string[] tokens = input.Split(' ');
        Stack<double> stack = new Stack<double>();

        foreach (string token in tokens)
        {
            if (double.TryParse(token, out double operand))
            {
                stack.Push(operand);
            }
            else
            {
                if (stack.Count < 2)
                {
                    throw new ArgumentException("Expression RPN invalide : Nombre insuffisant d'opérandes.");
                }

                double operand2 = stack.Pop();
                double operand1 = stack.Pop();
                double result = PerformOperation(token, operand1, operand2);
                stack.Push(result);
            }
        }

        if (stack.Count == 1)
        {
            return stack.Pop();
        }
        else
        {
            throw new ArgumentException("Expression RPN invalide : Opérandes restantes après l'évaluation.");
        }
    }

    static double PerformOperation(string operation, double operand1, double operand2)
    {
        switch (operation)
        {
            case "+":
                return operand1 + operand2;
            case "-":
                return operand1 - operand2;
            case "*":
                return operand1 * operand2;
            case "/":
                if (operand2 != 0)
                {
                    return operand1 / operand2;
                }
                else
                {
                    throw new DivideByZeroException("Erreur : Division par zéro.");
                }
            default:
                throw new ArgumentException("Erreur : Opérateur non pris en charge.");
        }
    }
}
