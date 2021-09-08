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
        private const string defaultSuffix = "Please enter new ones like: Stone Scissors Paper Lizard Spock";
        public InputParametersValidator(string[] source) => _source = source;
        public (bool, string) Validate() {
            var message = string.Empty;
            if (SourceIsNullOrEmpty()) {
                message = "No input parameters.";
                return (false, $"{message} {defaultSuffix}");
            }
            if (LengthIsLessThenThree()) {
                message = "Input parameters is less then three.";
                return (false, $"{message} {defaultSuffix}");
            }
            if (LengthIsEven()) {
                message = "Amount of parameters should be even.";
                return (false, $"{message} {defaultSuffix}");
            }
            if (ContainsRepeatedItems()) {
                message = "Parameters contain repeated items.";
                return (false, $"{message} {defaultSuffix}");
            }
            return (true, string.Empty);
        }
        private bool LengthIsLessThenThree() => _source.Length < 3;
        private bool LengthIsEven() => _source.Length % 2 == 0;
        private bool ContainsRepeatedItems() => _source.Count() != _source.Distinct().Count();
        private bool SourceIsNullOrEmpty() {
            if (_source is null) {
                return true;
            }
            if (!_source.Any()) {
                return true;
            }
            return false;
        }
    }
}
