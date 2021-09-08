using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardSpockGame.Core {
    public class RulesInfo {
        private readonly string[] _turns;
        private readonly int _amountOfWinners;
        public RulesInfo(string[] turns) {
            _turns = turns;
            _amountOfWinners = (_turns.Length - 1) / 2;
        }
        public int GetNextIndex(int currentIndex) => (currentIndex < _turns.Length - 1) ? ++currentIndex : 0;
        /// <summary>
        /// tuple -> <key, winners: ones who win the key, loosers: ones who are weaker then the key>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(string, string[])> GetValues() {
            var result = new List<(string, string[])>();
            for (int i = 0; i < _turns.Length; i++) {
                // from point of view of a key all items that can beat him are inside the winners
                var key = _turns[i];
                var winners = new string[_amountOfWinners];
                var nextIndex = GetNextIndex(i);
                for (int j = 0; j < _amountOfWinners; j++) {
                    winners[j] = _turns[nextIndex];
                    nextIndex = GetNextIndex(nextIndex);
                }
                result.Add((key, winners));
            }
            return result;
        }
    }
}
