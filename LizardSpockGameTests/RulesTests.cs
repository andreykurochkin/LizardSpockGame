using FluentAssertions;
using LizardSpockGame.Core;
using LizardSpockGame.Core.Validators;
using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;

namespace LizardSpockGameTests {
    public class RulesTests {
        private Rules _sut;
        [Theory]
        [InlineData("A", "B", "C")]
        public void GetValues_ShouldReturnCorrectSetOfData_WhenAmountOfRulesIsThree(params string[] moves) {
            _sut = new Rules(moves);
            var keyAndWinners = new List<(string, string[], string[])>() {
                { ("A", new string[] { "B" }, new string[] { "C" }) },
                { ("B", new string[] { "C" }, new string[] { "A" }) },
                { ("C", new string[] { "A" }, new string[] { "B" }) },
            };

            var result = _sut.GetValues();

            result.Count().Should().Be(moves.Length);
            foreach (var tuple in keyAndWinners) {
                var resultItem = result.Where(x => x.Item1 == tuple.Item1).First();
                resultItem.Item2.Should().Equal(tuple.Item2);
                resultItem.Item3.Should().Equal(tuple.Item3);
            }
        }
        [Theory]
        [InlineData("A", "B", "C", "D", "E")]
        public void GetValues_ShouldReturnCorrectSetOfData_WhenAmountOfRulesIsFive(params string[] moves) {
            _sut = new Rules(moves);
            // <key, winners, loosers>
            var keyAndWinners = new List<(string, string[], string[])>() {
                { ("A", new string[] { "B", "C" }, new string[] { "D", "E" }) },
                { ("B", new string[] { "C", "D" }, new string[] { "E", "A" }) },
                { ("C", new string[] { "D", "E" }, new string[] { "A", "B" }) },
                { ("D", new string[] { "E", "A" }, new string[] { "B", "C" }) },
                { ("E", new string[] { "A", "B" }, new string[] { "C", "D" }) },
            };
            var result = _sut.GetValues();

            result.Count().Should().Be(moves.Length);
            foreach (var tuple in keyAndWinners) {
                var resultItem = result.Where(x => x.Item1 == tuple.Item1).First();
                resultItem.Item2.Should().Equal(tuple.Item2);
                resultItem.Item3.Should().Equal(tuple.Item3);
            }
        }
        [Theory]
        [InlineData(0, 1, "A", "B", "C")]
        [InlineData(1, 2, "A", "B", "C")]
        [InlineData(2, 0, "A", "B", "C")]
        public void GetNextIndex_ShouldReturnValidIndex_WhenDataIsValid(int currentIndex, int expected, params string[] moves) {
            _sut = new Rules(moves);

            var result = _sut.GetNextIndex(currentIndex);

            result.Should().Be(expected);
        }
    }
}
