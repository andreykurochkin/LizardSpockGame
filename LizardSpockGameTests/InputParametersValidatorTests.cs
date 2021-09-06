using FluentAssertions;
using LizardSpockGame.Core.Validators;
using System;
using Xunit;

namespace LizardSpockGameTests {
    public class InputParametersValidatorTests {
        private InputParametersValidator _sut;
        [Fact]
        public void Validate_ShouldReturnFalse_WhenSourceIsNull() {
            _sut = new InputParametersValidator(null);

            var result = _sut.Validate();

            result.Item1.Should().BeFalse();
            result.Item2.Should().Contain("No input parameters.");
        }
        [Fact]
        public void Validate_ShouldReturnFalse_WhenSourceIsEmpty() {
            _sut = new InputParametersValidator(Array.Empty<string>());

            var result = _sut.Validate();

            result.Item1.Should().BeFalse();
            result.Item2.Should().Contain("No input parameters.");
        }
        [Theory]
        [InlineData("Scissors")]
        [InlineData("Scissors", "Spock")]
        public void Validate_ShouldReturnFalse_WhenSourceContainsTwoItems(params string[] source) {
            _sut = new InputParametersValidator(source);

            var result = _sut.Validate();

            result.Item1.Should().BeFalse(result.Item2);
            result.Item2.Should().Contain("Input parameters is less then three.");
        }
        [Theory]
        [InlineData("Freddie Mercury", "Brian May", "Roger Taylor", "John Deacon")]
        public void Validate_ShouldReturnFalse_WhenSourceContainsEvenAmountOfInputItems(params string[] source) {
            _sut = new InputParametersValidator(source);

            var result = _sut.Validate();

            result.Item1.Should().BeFalse();
            result.Item2.Should().Contain("Amount of parameters should be odd.");
        }
        [Theory]
        [InlineData("Freddie Mercury", "Brian May", "Roger Taylor", "John Deacon", "Freddie Mercury")]
        public void Validate_ShouldReturnFalse_WhenSourceContainsRepeatedItems(params string[] source) {
            _sut = new InputParametersValidator(source);

            var result = _sut.Validate();

            result.Item1.Should().BeFalse();
            result.Item2.Should().Contain("Parameters should not contain repeated items.");
        }
        [Theory]
        [InlineData("stone", "scissors", "paper", "lizard", "spock")]
        public void Validate_ShouldReturnTrue_WhenAllDataIsValid(params string[] source) {
            _sut = new InputParametersValidator(source);

            var result = _sut.Validate();

            result.Item1.Should().BeTrue();
        }
    }
}
