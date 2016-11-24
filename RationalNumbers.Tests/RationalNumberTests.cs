using System;
using Xunit;

namespace EdlinSoftware.RationalNumbers.Tests
{
    public class RationalNumberTests
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentOutOfRangeException_IfDenominatorIsNull()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new RationalNumber(1, 0));
            Assert.Equal("denominator", exception.ParamName);
        }

        [Theory]
        [InlineData(4, 2, 2, 1)]
        [InlineData(8, 6, 4, 3)]
        [InlineData(5, 3, 5, 3)]
        public void Constructor_ShouldSimplifyValue(long inputNumerator, long inputDenominator, long expectedNumerator, long expectedDenominator)
        {
            var rationalNumber = new RationalNumber(inputNumerator, inputDenominator);

            Assert.Equal(expectedNumerator, rationalNumber.Numerator);
            Assert.Equal(expectedDenominator, rationalNumber.Denominator);
        }

        [Fact]
        public void Denominator_IsAlwaysPositive()
        {
            var rationalNumber = new RationalNumber(1, -1);

            Assert.True(rationalNumber.Denominator > 0);
        }

        [Fact]
        public void Denominator_ShouldBeOne_ForZero()
        {
            var rationalNumber = new RationalNumber(0, -10);

            Assert.Equal(1, rationalNumber.Denominator);
        }

        [Theory]
        [InlineData(4, 2, "2")]
        [InlineData(8, 6, "4/3")]
        [InlineData(5, 3, "5/3")]
        [InlineData(0, 3, "0")]
        [InlineData(-1, 3, "-1/3")]
        [InlineData(-4, -3, "4/3")]
        [InlineData(long.MaxValue, long.MaxValue, "1")]
        [InlineData(long.MinValue, long.MinValue, "1")]
        public void ToString_Values(long inputNumerator, long inputDenominator, string expectedString)
        {
            var rationalNumber = new RationalNumber(inputNumerator, inputDenominator);

            Assert.Equal(expectedString, rationalNumber.ToString());
        }

        [Theory]
        [InlineData(4, 2, 4, 2, true)]
        [InlineData(4, 2, 2, 1, true)]
        [InlineData(4, 2, -2, -1, true)]
        [InlineData(4, 2, -2, 1, false)]
        [InlineData(5, 3, 7, 2, false)]
        public void Equal_Values(long numerator1, long denominator1, long numerator2, long denominator2, bool areEqual)
        {
            var rationalNumber1 = new RationalNumber(numerator1, denominator1);
            var rationalNumber2 = new RationalNumber(numerator2, denominator2);

            Assert.Equal(areEqual, rationalNumber1.Equals(rationalNumber2));
        }

    }
}
