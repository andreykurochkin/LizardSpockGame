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
            if (!ValidateInputParametersAndLog(args)) {
                return;
            }
            new Game(
                args, 
                new InRangeRandomNumberProvider(0, args.Length), 
                new Key()
            ).Play();
        }
        private static bool ValidateInputParametersAndLog(string[] args) {
            var inputParametersValidator = new InputParametersValidator(args);
            var validationResult = inputParametersValidator.Validate();
            if (!validationResult.Item1) {
                Console.WriteLine(validationResult.Item2);
            }
            return validationResult.Item1;
        }
    }
}
