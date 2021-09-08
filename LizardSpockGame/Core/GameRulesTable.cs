using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardSpockGame.Core {
    public class GameRulesTable {
        private readonly string[] _turns;
        private readonly int _defaultCellLength = 6;
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private int CellLength { get; }
        public GameRulesTable(string[] turns) {
            _turns = turns;
            CellLength = GetCellLength();
        }
        private int GetCellLength() {
            var value = _turns.Select(x => x.Length).Max() + 2;
            return (value > _defaultCellLength) ? value : _defaultCellLength;
        }
        //private void Build() {
        //    _stringBuilder.AppendLi
        //        ne()
        //}
    }
}
