using LizardSpockGame.Core;
using LizardSpockGame.Core.Validators;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ChanceNET;
using LizardSpockGame.Core.Providers;

namespace LizardSpockGame {
    public class Program {
        public static void Main(string[] args) {
            if (!LoggedValidateInputParameters(args)) {
                return;
            }
            var key = new Key();
            var game = new Game(args, new InRangeRandomNumberProvider(0, args.Length), key);
            game.Play();
        }

        //public static void Main(string[] args) {
        //    if (!LoggedValidateInputParameters(args)) {
        //        return;
        //    }
        //    var key = new Key();
        //    while (true) {
        //        var game = new Game(args, new InRangeRandomNumberProvider(0, args.Length), key);
        //        var keepPlaying = game.Play() == GameResult.TurnAgain;
        //        if (!keepPlaying) {
        //            break;
        //        }
        //    }
        //    Console.ReadLine();
        //}

        //public static void Main(string[] args) {
        //    if (!LoggedValidateInputParameters(args)) {
        //        return;
        //    }
        //    (var key, var rules, var pcTurnIndex) = (new Key(), new RulesInfo(args).GetValues(), GenerateRandomIndex(args.Length));
        //    PlayTheGame(args, rules, key, pcTurnIndex);
        //    Console.ReadKey();
        //}
        private static bool LoggedValidateInputParameters(string[] args) {
            var inputParametersValidator = new InputParametersValidator(args);
            var validationResult = inputParametersValidator.Validate();
            if (!validationResult.Item1) {
                Console.WriteLine(validationResult.Item2);
            }
            return validationResult.Item1;
        }

        private static void PrintHmac(byte[] hmac) => Console.WriteLine($"HMAC: {hmac}");

        private static void PrintTurnsInfo(string[] turns) {
            Console.WriteLine("Available turns:");
            turns.Select((Value, Index) => new { i = Index + 1, v = Value })
                .ToList()
                .ForEach(x => Console.WriteLine($"{x.i} - {x.v}"));
            Console.WriteLine("0 - exit");
            Console.WriteLine("? - help");
        }
        private static void PlayTheGame(string[] turns, IEnumerable<(string, string[], string[])> rules, Key key, int pcTurnIndex) {
            var exitGame = false;
            //PrintHeader(args, key.ComputeHmac(args[pcTurnIndex]));
            while (!exitGame) {
                Console.Write("Enter your turn: ");
                var userInput = Console.ReadLine();
                Console.WriteLine();
                var isQuestionMark = (userInput == "?");
                if (isQuestionMark) {
                    PrintTable(rules);
                    exitGame = false;
                    continue;
                }
                var isZeroDigit = (userInput == "0");
                if (isZeroDigit) {
                    Console.WriteLine("see ya");
                    exitGame = true;
                    continue;
                }
                if (!int.TryParse(userInput, out int number)) {
                    //exitGame = true;
                    Console.WriteLine("data is invalid, new data needed ...");
                    exitGame = false;
                    continue;
                }
                var isInRange = ((number >= 1) && (number <= turns.Length));
                if (!isInRange) {
                    Console.WriteLine("number is outside of the range, new data needed ...");
                    exitGame = false;
                    continue;
                }
                Console.WriteLine($"Your turn number: {turns[number - 1]}");
                var pcIndex = new ChanceNET.Chance().Integer(0, turns.Length);
                //Console.WriteLine($"Computer turn: {turns[gene]}");
                //Console.WriteLine(GetResult(turns[userIndex], turns[pcIndex], rules));
                Console.WriteLine($"HMAC key: {key}");
                exitGame = true;
            }
        }
        private static string GetRandomTurnResult(string[] turns) => turns[GenerateRandomIndex(turns.Length)];
        private static int GenerateRandomIndex(int maxValue) => new ChanceNET.Chance().Integer(0, maxValue);
        private static string GetResult(string userTurn, string pcTurn, IEnumerable<(string, string[], string[])> rules) {
            return IsDraw(userTurn, pcTurn)
                ? "Draw!"
                : (DoesUserWin(userTurn, pcTurn, rules))
                    ? "You win!"
                    : "You lost!";
        }

        private static bool IsDraw(string userTurn, string pcTurn) => userTurn == pcTurn;

        private static bool DoesUserWin(string userTurn, string pcTurn, IEnumerable<(string, string[], string[])> rules) =>
            rules.Where(t => t.Item1 == pcTurn)
                .First()
                .Item2
                .Contains(userTurn);

        private static void PrintTable(IEnumerable<(string, string[], string[])> rules) {
            Console.WriteLine("print table");
        }
    }
}
