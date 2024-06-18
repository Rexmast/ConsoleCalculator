using System;
using Xunit;
using CalculatorLibrary;

namespace CalculatorTests
{
    public class CalculatorTests
    {
        [Fact]
        public void Evaluate_Addition_ReturnsCorrectResult()
        {
            var calculator = new Calculator();
            var result = calculator.Evaluate("2+3");
            Assert.Equal(5, result);
        }

        [Fact]
        public void Evaluate_InvalidExpression_ThrowsException()
        {
            var calculator = new Calculator();
            Assert.Throws<ArgumentException>(() => calculator.Evaluate("2++3"));
        }
    }
}