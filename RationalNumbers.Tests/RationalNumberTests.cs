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
        [InlineData(long.MaxValue, long.MinValue + 1, "-1")]
        [InlineData(long.MinValue + 1, long.MaxValue, "-1")]
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

        [Theory]
        [InlineData(4, 2, 4, 2, true)]
        [InlineData(4, 2, 2, 1, true)]
        [InlineData(4, 2, -2, -1, true)]
        [InlineData(4, 2, -2, 1, false)]
        [InlineData(5, 3, 7, 2, false)]
        public void EqualityOperator_Values(long numerator1, long denominator1, long numerator2, long denominator2, bool areEqual)
        {
            var rationalNumber1 = new RationalNumber(numerator1, denominator1);
            var rationalNumber2 = new RationalNumber(numerator2, denominator2);

            Assert.Equal(areEqual, rationalNumber1 == rationalNumber2);
        }

        [Theory]
        [InlineData(4, 2, 4, 2, false)]
        [InlineData(-4, 2, -4, 2, false)]
        [InlineData(4, 2, 2, 1, false)]
        [InlineData(4, 2, -2, -1, false)]
        [InlineData(4, 2, -2, 1, false)]
        [InlineData(4, 2, 6, 1, true)]
        [InlineData(5, 3, 7, 2, true)]
        [InlineData(long.MaxValue - 1, long.MaxValue, long.MaxValue - 1, long.MaxValue, false)]
        [InlineData(long.MaxValue - 2, long.MaxValue, long.MaxValue - 1, long.MaxValue, true)]
        [InlineData(long.MinValue + 2, long.MaxValue, long.MinValue + 1, long.MaxValue, false)]
        public void LessOperator_Values(long numerator1, long denominator1, long numerator2, long denominator2, bool isTrue)
        {
            var rationalNumber1 = new RationalNumber(numerator1, denominator1);
            var rationalNumber2 = new RationalNumber(numerator2, denominator2);

            Assert.Equal(isTrue, rationalNumber1 < rationalNumber2);
        }

        [Theory]
        [InlineData(4, 2, 4, 2, false)]
        [InlineData(-4, 2, -4, 2, false)]
        [InlineData(4, 2, 2, 1, false)]
        [InlineData(4, 2, -2, -1, false)]
        [InlineData(4, 2, -2, 1, true)]
        [InlineData(4, 2, 6, 1, false)]
        [InlineData(5, 3, 7, 2, false)]
        [InlineData(7, 2, 5, 3, true)]
        [InlineData(long.MaxValue - 1, long.MaxValue, long.MaxValue - 1, long.MaxValue, false)]
        [InlineData(long.MaxValue - 2, long.MaxValue, long.MaxValue - 1, long.MaxValue, false)]
        [InlineData(long.MinValue + 2, long.MaxValue, long.MinValue + 1, long.MaxValue, true)]
        public void GreaterOperator_Values(long numerator1, long denominator1, long numerator2, long denominator2, bool isTrue)
        {
            var rationalNumber1 = new RationalNumber(numerator1, denominator1);
            var rationalNumber2 = new RationalNumber(numerator2, denominator2);

            Assert.Equal(isTrue, rationalNumber1 > rationalNumber2);
        }

        [Theory]
        [InlineData(1, 2, 4, 2)]
        [InlineData(-2, 1, 4, 2)]
        [InlineData(long.MaxValue - 2, long.MaxValue, long.MaxValue - 1, long.MaxValue)]
        [InlineData(long.MinValue + 1, long.MaxValue, long.MinValue + 2, long.MaxValue)]
        public void CompareTo_FirstIsLess_Values(long numerator1, long denominator1, long numerator2, long denominator2)
        {
            var rationalNumber1 = new RationalNumber(numerator1, denominator1);
            var rationalNumber2 = new RationalNumber(numerator2, denominator2);

            Assert.True(rationalNumber1.CompareTo(rationalNumber2) < 0);
        }

        [Theory]
        [InlineData(4, 2, 1, 2)]
        [InlineData(2, 1, -2, 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, long.MaxValue - 2, long.MaxValue)]
        [InlineData(long.MinValue + 2, long.MaxValue, long.MinValue + 1, long.MaxValue)]
        public void CompareTo_SecondIsLess_Values(long numerator1, long denominator1, long numerator2, long denominator2)
        {
            var rationalNumber1 = new RationalNumber(numerator1, denominator1);
            var rationalNumber2 = new RationalNumber(numerator2, denominator2);

            Assert.True(rationalNumber1.CompareTo(rationalNumber2) > 0);
        }

        [Theory]
        [InlineData(4, 2, 2, 1)]
        [InlineData(-2, 1, -2, 1)]
        [InlineData(long.MaxValue - 1, long.MaxValue, long.MaxValue - 1, long.MaxValue)]
        [InlineData(long.MinValue + 1, long.MaxValue, long.MinValue + 1, long.MaxValue)]
        public void CompareTo_BothEqual_Values(long numerator1, long denominator1, long numerator2, long denominator2)
        {
            var rationalNumber1 = new RationalNumber(numerator1, denominator1);
            var rationalNumber2 = new RationalNumber(numerator2, denominator2);

            Assert.True(rationalNumber1.CompareTo(rationalNumber2) == 0);
        }

        [Theory]
        [InlineData(2, 1, 2, 1, 4, 1)]
        [InlineData(2, 1, -2, 1, 0, 1)]
        [InlineData(2, 3, 3, 4, 17, 12)]
        [InlineData(5, 6, 7, 8, 41, 24)]
        public void AddOperator_Values(long numerator1, long denominator1, long numerator2, long denominator2,
            long resultNumerator, long resultDenominator)
        {
            Assert.Equal(new RationalNumber(resultNumerator, resultDenominator), new RationalNumber(numerator1, denominator1) + new RationalNumber(numerator2, denominator2));
        }

        [Theory]
        [InlineData(2, 1, 2, 1, 0, 1)]
        [InlineData(2, 1, -2, 1, 4, 1)]
        [InlineData(2, 3, 3, 4, -1, 12)]
        [InlineData(5, 6, 7, 8, -1, 24)]
        public void SubOperator_Values(long numerator1, long denominator1, long numerator2, long denominator2,
            long resultNumerator, long resultDenominator)
        {
            Assert.Equal(new RationalNumber(resultNumerator, resultDenominator), new RationalNumber(numerator1, denominator1) - new RationalNumber(numerator2, denominator2));
        }

        [Theory]
        [InlineData(2, 1, 2, 1, 4, 1)]
        [InlineData(2, 1, -2, 1, -4, 1)]
        [InlineData(2, 3, 3, 4, 1, 2)]
        [InlineData(5, 6, 7, 8, 35, 48)]
        public void MulOperator_Values(long numerator1, long denominator1, long numerator2, long denominator2,
            long resultNumerator, long resultDenominator)
        {
            Assert.Equal(new RationalNumber(resultNumerator, resultDenominator), new RationalNumber(numerator1, denominator1) * new RationalNumber(numerator2, denominator2));
        }

        [Theory]
        [InlineData(2, 1, 2, 1, 1, 1)]
        [InlineData(2, 1, -2, 1, -1, 1)]
        [InlineData(2, 3, 3, 4, 8, 9)]
        [InlineData(5, 6, 7, 8, 20, 21)]
        public void DivOperator_Values(long numerator1, long denominator1, long numerator2, long denominator2,
            long resultNumerator, long resultDenominator)
        {
            Assert.Equal(new RationalNumber(resultNumerator, resultDenominator), new RationalNumber(numerator1, denominator1) / new RationalNumber(numerator2, denominator2));
        }

        [Fact]
        public void DivOperator_DivisionByZero()
        {
            Assert.Throws<DivideByZeroException>(() => new RationalNumber(1, 1) / new RationalNumber(0, 1));
        }

        [Theory]
        [InlineData(2, 1, true)]
        [InlineData(-2, 1, false)]
        [InlineData(long.MaxValue, 1, true)]
        [InlineData(long.MinValue + 1, 1, false)]
        public void Abs_Values(long numerator1, long denominator1, bool isSame)
        {
            if (isSame)
            {
                Assert.Equal(new RationalNumber(numerator1, denominator1), new RationalNumber(numerator1, denominator1).Abs());
            }
            else
            {
                Assert.Equal(new RationalNumber(-numerator1, denominator1), new RationalNumber(numerator1, denominator1).Abs());

            }
        }
    }
}
