using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardSpockGame.Core.Validators {
    public class LoggedUserInputValidator {
        Action DoNothing = delegate { };
        public (bool, Action) Validate(string userInput, IEnumerable<int> range) {
            if (userInput == "?") {
                return (false, DoNothing);
            }
            if (!int.TryParse(userInput, out int number)) {
                return (false, (() => Console.WriteLine("data is invalid, new data needed ...")));
            }
            if (!range.Contains(number)) {
                return (false, (() => Console.WriteLine("number is outside of the range, new data needed ...")));
            }
            return (true, DoNothing);
        }
    }
}
