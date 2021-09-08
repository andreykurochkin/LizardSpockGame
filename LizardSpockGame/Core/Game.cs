using ChanceNET;
using LizardSpockGame.Core.Providers;
using LizardSpockGame.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardSpockGame.Core {
    public class Game {
        private Lazy<IEnumerable<(string, string[], string[])>> _rules;
        private InRangeRandomNumberProvider _randomNumberProvider;
        private int _pcTurnIndex;
        private string[] Turns { get; }
        internal string PcTurn { get => Turns[_pcTurnIndex]; }
        internal Key Key { get; }
        internal IEnumerable<(string, string[], string[])> Rules { get => _rules.Value; }
        private LoggedUserInputValidator _userInputValidator = new LoggedUserInputValidator();

        public Game(string[] turns, InRangeRandomNumberProvider randomNumberProvider, Key key) {
            (Turns, _randomNumberProvider, Key) = (turns, randomNumberProvider, key);
            _rules = new(() => new RulesInfo(Turns).GetValues());
        }

        //public GameResult Play() {
        //    PrintDisclaimer();
        //    MakePcTurn();
        //    PrintHmac();
        //    PrintAvailableTurns();
        //    var userResponse = RequestUser();
        //    if (userResponse == "0") {
        //        Console.WriteLine("see ya");
        //        return GameResult.ExitGame;
        //    }
        //    if (userResponse == "?") {
        //        PrintTable();
        //    }
        //    if (!int.TryParse(userResponse, out int number)) {
        //        Console.WriteLine("data is invalid, new data needed ...");
        //        return GameResult.TurnAgain;
        //    }
        //    var isInRange = ((number >= 1) && (number <= Turns.Length));
        //    if (!isInRange) {
        //        Console.WriteLine("number is outside of the range, new data needed ...");
        //        return GameResult.TurnAgain;
        //    }
        //    PrintResult(Turns[number - 1]);
        //    return GameResult.TurnAgain;
        //}

        //private GameResult ValidateAndActUserInput(string userInput) {
        //    var validationResult = _userInputValidator.Validate(userInput);
        //    if (validationResult.Item2 is not null) {
        //        validationResult.Item2.Invoke();
        //    }
        //    return validationResult.Item1;
        //}

        public GameResult Play() {
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
                PrintResult(Turns[Convert.ToInt32(userResponse)]);
                return GameResult.TurnAgain;
            } while (true);
        }
        private bool ValidateUserProvidedDataAndLog(string userInput, IEnumerable<int> range) {
            var validationResult = _userInputValidator.Validate(userInput, range);
            validationResult.Item2.Invoke();
            return validationResult.Item1;
        }
        private bool IsExitRequired(string userInput) => userInput == "0";
        private void PrintResult(string userTurn) {
            Console.WriteLine($"Your turn number: {userTurn}");
            Console.WriteLine($"Computer turn: {PcTurn}");
            Console.WriteLine(GetResult(userTurn, PcTurn));
            Console.WriteLine($"HMAC key: {Key}");
            Console.WriteLine();
        }
        private void PrintDisclaimer() {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("New game started");
        }

        private void MakePcTurn() => _pcTurnIndex = _randomNumberProvider.GenerateValue();

        private string RequestUser() {
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
        private void ExitApplicationIfRequired(string userInput) {
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

        private bool IsDraw(string userTurn, string pcTurn) => userTurn == pcTurn;

        private bool DoesUserWin(string userTurn, string pcTurn) =>
            Rules.Where(t => t.Item1 == pcTurn)
                .First()
                .Item2
                .Contains(userTurn);
    }
}
