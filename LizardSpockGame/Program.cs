using LizardSpockGame.Core;
using LizardSpockGame.Core.Validators;
using System;
using System.Security.Cryptography;
using System.Text;
namespace LizardSpockGame {
    class Program {
        static void Main(string[] args) {
            var inputParametersValidator = new InputParametersValidator(args);
            var validationResult = inputParametersValidator.Validate();
            if (!validationResult.Item1) {
                Console.WriteLine(validationResult.Item2);
                return;
            }
            var key = new Key();
            Console.Write("HMAC: ");
            Console.Write(key.ToString());
            Console.WriteLine("Available moves:");
            Console.ReadKey();
        }
    }
}
