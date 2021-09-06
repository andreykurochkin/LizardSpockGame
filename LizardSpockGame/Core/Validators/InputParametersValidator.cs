using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardSpockGame.Core.Validators {
    /// <summary>
    /// validates parameters
    /// </summary>
    public class InputParametersValidator {
        private readonly string[] _source;
        public InputParametersValidator(string[] source) => _source = source;
        public (bool, string) Validate() {
            return (true, string.Empty);
        }
        private bool LengthIsLessThenThree() {
            throw new NotImplementedException();
        }
        private bool LengthIsEven() {
            throw new NotImplementedException();
        }
        private bool ContainsRepeatedItems() {
            throw new NotImplementedException
        }
    }
}
