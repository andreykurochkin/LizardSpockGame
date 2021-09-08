using ChanceNET;
using LizardSpockGame.Core.Extensions;
using LizardSpockGame.Core.Providers;
using LizardSpockGame.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardSpockGame.Core {
    public class Game {
        private readonly Lazy<IEnumerable<(string, string[])>> _rules;
        private readonly InRangeRandomNumberProvider _randomNumberProvider;
        private int _pcTurnIndex;
        private string[] Turns { get; }
        internal string PcTurn { get => Turns[_pcTurnIndex]; }
        internal Key Key { get; }
        internal IEnumerable<(string, string[])> Rules { get => _rules.Value; }
        private readonly ConsoleLoggedUserInputValidator _userInputValidator = new();

        public Game(string[] turns, InRangeRandomNumberProvider randomNumberProvider, Key key) {
            (Turns, _randomNumberProvider, Key) = (turns, randomNumberProvider, key);
            _rules = new(() => new RulesInfo(Turns).GetValues());
        }

        public void Play() {
            PrintDisclaimer();
            MakePcTurn();
            PrintHmac();
            PrintAvailableTurns();
            do {
                var userResponse = RequestUser();
                ShowHelpIfRequired(userResponse);
                ExitApplicationIfRequired(userResponse);
                if (!ValidateUserProvidedDataAndLog(userResponse, Enumerable.Range(1, Turns.Length))) {
                    continue;
                }
                PrintResult(GetUserTurn(userResponse));
                break;
            } while (true);
        }

        private string GetUserTurn(string userInput) => Turns[Convert.ToInt32(userInput) - 1];

        private bool ValidateUserProvidedDataAndLog(string userInput, IEnumerable<int> range) {
            var validationResult = _userInputValidator.Validate(userInput, range);
            validationResult.Item2.Invoke();
            return validationResult.Item1;
        }

        private void PrintResult(string userTurn) {
            Console.WriteLine($"Your turn number: {userTurn}");
            Console.WriteLine($"Computer turn: {PcTurn}");
            Console.WriteLine(GetResult(userTurn, PcTurn));
            Console.WriteLine($"HMAC key: {Key}");
            Console.WriteLine();
        }
        private static void PrintDisclaimer() {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("New game started");
        }

        private void MakePcTurn() => _pcTurnIndex = _randomNumberProvider.GenerateValue();

        private static string RequestUser() {
            Console.Write("Enter your turn: ");
            return Console.ReadLine();
        }

        private void PrintHmac() => Console.WriteLine($"HMAC: {Key.ComputeHmac(PcTurn).ToHexString()}");

        private void PrintAvailableTurns() {
            Console.WriteLine("Available turns:");
            Turns.Select((Value, Index) => new { i = Index + 1, v = Value })
                .ToList()
                .ForEach(x => Console.WriteLine($"{x.i} - {x.v}"));
            Console.WriteLine("0 - exit");
            Console.WriteLine("? - help");
        }

        private static void ExitApplicationIfRequired(string userInput) {
            if (userInput != "0") {
                return;
            }
            Console.WriteLine("see ya");
            Environment.Exit(0);
        }

        private void ShowHelpIfRequired(string userInput) {
            if (userInput != "?") {
                return;
            }
            Console.WriteLine();
            Table.Create(Turns, Rules).Write();
            Console.WriteLine();
        }

        private string GetResult(string userTurn, string pcTurn) {
            return IsDraw(userTurn, pcTurn)
                ? "Draw!"
                : (DoesUserWin(userTurn, pcTurn))
                    ? "You win!"
                    : "You lost!";
        }

        private static bool IsDraw(string userTurn, string pcTurn) => userTurn == pcTurn;

        private bool DoesUserWin(string userTurn, string pcTurn) =>
            Rules.Where(t => t.Item1 == pcTurn)
                .First()
                .Item2
                .Contains(userTurn);
    }
}
