using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
namespace LizardSpockGame.Core {
    public class Table {
        public static ConsoleTable Create(string[] headers, IEnumerable<(string, string[])> values) {
            var table = new ConsoleTable(CreateHeaderWithFirstCell("  \\  ", headers).ToArray());
            foreach (var row in GetRows(headers, values)) {
                table.AddRow(row.ToArray());
            }
            return table;
        }

        private static IEnumerable<List<string>> GetRows(string[] headers, IEnumerable<(string, string[])> values) =>
            headers.Select(h => CreateRow(h, headers, values.First(t => t.Item1 == h)));

        private static List<string> CreateHeaderWithFirstCell(string firtsCell, IEnumerable<string> keys) {
            var header = new List<string> { firtsCell };
            header.AddRange(keys);
            return header;
        }

        private static List<string> CreateRow(string firstCell, IEnumerable<string> headers, (string, string[]) tuple) {
            var row = new List<string> { firstCell };
            foreach (var header in headers) {
                if (header == tuple.Item1) {
                    row.Add("draw");
                    continue;
                }
                if (tuple.Item2.Contains(header)) {
                    row.Add("lose");
                    continue;
                }
                row.Add("win");
            }
            return row;
        }
    }
}
