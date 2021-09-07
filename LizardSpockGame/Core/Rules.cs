using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardSpockGame.Core {
    public class Rules {
        private readonly string[] _moves;
        private readonly int _amountOfWinners;
        public Rules(string[] moves) {
            _moves = moves;
            _amountOfWinners = (_moves.Length - 1) / 2;
        }
        public int GetNextIndex(int currentIndex) => (currentIndex < _moves.Length - 1) ? ++currentIndex : 0;
        /// <summary>
        /// tuple -> <key, winners: ones who win the key, loosers: ones who are weaker then the key>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(string, string[], string[])> GetValues() {
            var result = new List<(string, string[], string[])>();
            for (int i = 0; i < _moves.Length; i++) {
                var key = _moves[i];
                var winners = new string[_amountOfWinners];
                var loosers = new string[_amountOfWinners];
                var nextIndex = GetNextIndex(i);
                for (int j = 0; j < _amountOfWinners; j++) {
                    winners[j] = _moves[nextIndex];
                    nextIndex = GetNextIndex(nextIndex);
                }
                for (int j = 0; j < _amountOfWinners; j++) {
                    loosers[j] = _moves[nextIndex];
                    nextIndex = GetNextIndex(nextIndex);
                }
                result.Add((key, winners, loosers));
            }
            return result;
        }
    }
}
