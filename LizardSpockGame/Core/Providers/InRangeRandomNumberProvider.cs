using ChanceNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardSpockGame.Core.Providers {
    public class InRangeRandomNumberProvider {
        private readonly Chance _chance;
        private readonly int _min;
        private readonly int _max;
        public InRangeRandomNumberProvider(int min, int max) {
            _chance = new Chance();
            _min = min;
            _max = max;
        }
        public int GenerateValue() => _chance.Integer(_min, _max);
    }
}
