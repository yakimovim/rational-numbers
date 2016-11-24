using System;

namespace EdlinSoftware.RationalNumbers
{
    /// <summary>
    /// Represents rational number.
    /// </summary>
    public struct RationalNumber : IEquatable<RationalNumber>
    {
        /// <summary>
        /// Numerator of value.
        /// </summary>
        public readonly long Numerator;
        /// <summary>
        /// Denominator of value.
        /// </summary>
        /// <remarks>This is always positive value.</remarks>
        public readonly long Denominator;

        /// <summary>
        /// Initializes instance of <see cref="RationalNumber"/>
        /// </summary>
        /// <param name="numerator">Value of numerator.</param>
        /// <param name="denominator">Value of denominator.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Denominator is zero.</exception>
        public RationalNumber(long numerator, long denominator)
        {
            if (denominator == 0L) throw new ArgumentOutOfRangeException(nameof(denominator));

            if (numerator == 0)
            {
                Numerator = 0;
                Denominator = 1;
            }
            else
            {
                var greatestCommonDenominator = GreatestCommonDenominator(numerator, denominator);

                Numerator = denominator > 0 ? numerator / greatestCommonDenominator : -numerator / greatestCommonDenominator;
                Denominator = Math.Abs(denominator / greatestCommonDenominator);
            }
        }

        /// <summary>
        /// Returns greatest common denominator of two integers.
        /// </summary>
        /// <param name="a">First intreger.</param>
        /// <param name="b">Second integer.</param>
        private static long GreatestCommonDenominator(long a, long b)
        {
            if (a < 0)
            { a = -a; }
            if (b < 0)
            { b = -b; }

            if (b > a)
            {
                var temp = b;
                b = a;
                a = temp;
            }

            while (b != 0)
            {
                var temp = a % b;
                a = b;
                b = temp;
            }

            return a;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is RationalNumber))
                return false;

            return Equals((RationalNumber)obj);
        }

        /// <inheritdoc />
        public bool Equals(RationalNumber other)
        {
            return Numerator == other.Numerator && Denominator == other.Denominator;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Numerator.GetHashCode() * 397) ^ Denominator.GetHashCode();
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (Denominator == 1)
            { return Numerator.ToString(); }

            return Numerator + "/" + Denominator;
        }
    }
}
