using LizardSpockGame.Core;
using LizardSpockGame.Core.Validators;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ChanceNET;

namespace LizardSpockGame {
    public class Program {
        public static void Main(string[] args) {
            if (!LoggedValidateInputParameters(args)) {
                return;
            }
            (var key, var rules) = (new Key(), new Rules(args));
            PrintHeader(args, key);
            ProcessUserInput(args, rules.GetValues(), key);
            Console.ReadKey();
        }
        private static bool LoggedValidateInputParameters(string[] args) {
            var inputParametersValidator = new InputParametersValidator(args);
            var validationResult = inputParametersValidator.Validate();
            if (!validationResult.Item1) {
                Console.WriteLine(validationResult.Item2);
            }
            return validationResult.Item1;
        }
        private static void PrintHeader(string[] args, Key key) {
            Console.WriteLine($"HMAC: {key}");
            Console.WriteLine("Available moves:");
            PrintMoves(args);
            Console.WriteLine("0 - exit");
            Console.WriteLine("? - help");
        }
        private static void PrintMoves(string[] moves) {
            moves.Select((Value, Index) => new { i = Index + 1, v = Value })
                .ToList()
                .ForEach(x => Console.WriteLine($"{x.i} - {x.v}"));
        }
        private static void ProcessUserInput(string[] moves, IEnumerable<(string, string[], string[])> rules, Key key) {
            var exit = false;
            while (!exit) {
                Console.Write("Enter your move: ");
                var userInput = Console.ReadLine();
                Console.WriteLine();
                var isQuestionMark = (userInput == "?");
                if (isQuestionMark) {
                    PrintTable(moves);
                    continue;
                }
                var isZeroDigit = (userInput == "0");
                if (isZeroDigit) {
                    Console.WriteLine("see ya");
                    exit = true;
                    continue;
                }
                if (!int.TryParse(userInput, out int number)) {
                    exit = true;
                    continue;
                }
                var isInRange = ((number >= 1) && (number <= moves.Length));
                if (!isInRange) {
                    exit = false;
                    continue;
                }
                var userIndex = number - 1;
                Console.WriteLine($"Your move number: {moves[userIndex]}");
                var pcIndex = new ChanceNET.Chance().Integer(0, moves.Length);
                Console.WriteLine($"Computer move: {moves[pcIndex]}");
                Console.WriteLine(GetResult(moves[userIndex], moves[pcIndex], rules));
                Console.WriteLine($"HMAC key: {key}");
            }
        }
        private static string GetResult(string userChoice, string pcChoice, IEnumerable<(string, string[], string[])> rules) {
            var sameChoice = (userChoice == pcChoice);
            if (sameChoice) {
                return "Draw!";
            }
            var doesUserWon = rules.Where(t => t.Item1 == pcChoice)
                .First()
                .Item2
                .Contains(userChoice);
            return (doesUserWon) ? "You win!" : "You lost!";
        }
        //private static void ValidateUserInput(string[] moves) {
        //    var exit = false;
        //    while (!exit) {
        //        Console.Write("Enter your move: ");
        //        var userInput = Console.ReadKey().KeyChar;
        //        Console.WriteLine();
        //        var isQuestionMark = (userInput == '?');
        //        if (isQuestionMark) {
        //            PrintTable(moves);
        //            continue;
        //        }
        //        var isZeroDigit = (userInput == '0');
        //        if (isZeroDigit) {
        //            Console.WriteLine("see ya");
        //            exit = true;
        //            continue;
        //        }
        //        var isDigit = char.IsDigit(userInput);
        //        if (!isDigit) {
        //            exit = true;
        //            continue;
        //        }
        //        int digit = userInput - '0';
        //        var isInRange = ((digit >= 1) && (digit <= moves.Length));
        //        if (!isInRange) {
        //            exit = true;
        //            continue;
        //        }
        //        var index = digit--;
        //        Console.WriteLine(index);
        //        Console.WriteLine($"Your move: {digit}");
        //        Console.WriteLine("is differents");
        //    }

        //}
        private static void PrintTable(string[] moves) {
            Console.WriteLine("print table");
        }
    }
}
