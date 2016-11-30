using System;
using System.Diagnostics;

namespace EdlinSoftware.RationalNumbers
{
    /// <summary>
    /// Represents rational number.
    /// </summary>
    public struct RationalNumber : IEquatable<RationalNumber>, IComparable<RationalNumber>
    {
        /// <summary>
        /// Zero value.
        /// </summary>
        public static readonly RationalNumber Zero = new RationalNumber(0, 1);
        /// <summary>
        /// One value.
        /// </summary>
        public static readonly RationalNumber One = new RationalNumber(1, 1);
        /// <summary>
        /// Maximum value.
        /// </summary>
        public static readonly RationalNumber MaxValue = new RationalNumber(long.MaxValue, 1);
        /// <summary>
        /// Minimum value.
        /// </summary>
        public static readonly RationalNumber MinValue = new RationalNumber(long.MinValue + 1, 1);

        /// <summary>
        /// Numerator of value.
        /// </summary>
        public readonly long Numerator;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long _denominator;

        /// <summary>
        /// Denominator of value.
        /// </summary>
        /// <remarks>This is always positive value.</remarks>
        public long Denominator
        {
            [DebuggerStepThrough]
            get
            {
                // protection from default constructor.
                if (_denominator == 0)
                { _denominator = 1; }

                return _denominator;
            }
        }

        /// <summary>
        /// Initializes instance of <see cref="RationalNumber"/>
        /// </summary>
        /// <param name="numerator">Value of numerator.</param>
        /// <param name="denominator">Value of denominator.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Denominator is zero.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Denominator or numerator is <see cref="long.MinValue"/>.</exception>
        public RationalNumber(long numerator, long denominator)
        {
            if (denominator == 0L) throw new ArgumentOutOfRangeException(nameof(denominator));
            if (denominator == long.MinValue) throw new ArgumentOutOfRangeException(nameof(denominator));
            if (numerator == long.MinValue) throw new ArgumentOutOfRangeException(nameof(numerator));

            if (numerator == 0)
            {
                Numerator = 0;
                _denominator = 1;
            }
            else
            {
                var greatestCommonDenominator = GreatestCommonDenominator(numerator, denominator);

                Numerator = denominator > 0 ? numerator / greatestCommonDenominator : -numerator / greatestCommonDenominator;
                _denominator = Math.Abs(denominator / greatestCommonDenominator);
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

        public static bool operator ==(RationalNumber rn1, RationalNumber rn2)
        {
            return rn1.Equals(rn2);
        }

        public static bool operator !=(RationalNumber rn1, RationalNumber rn2)
        {
            return !(rn1 == rn2);
        }

        public static bool operator <(RationalNumber rn1, RationalNumber rn2)
        {
            if (rn1.Equals(rn2))
            { return false; }
            if (rn1.Numerator < 0 && rn2.Numerator >= 0)
            { return true; }
            if (rn1.Numerator >= 0 && rn2.Numerator < 0)
            { return false; }

            bool inverse = rn1.Numerator < 0 && rn2.Numerator < 0;

            var n1 = rn1.Numerator;
            var d1 = rn1.Denominator;
            var n2 = rn2.Numerator;
            var d2 = rn2.Denominator;
            if (inverse)
            {
                checked
                {
                    n1 = -n1;
                    n2 = -n2;
                }
            }

            return inverse 
                ? !ComparerOfMultiplications.IsLess((ulong)n1, (ulong)d2, (ulong)n2, (ulong)d1) 
                : ComparerOfMultiplications.IsLess((ulong)n1, (ulong)d2, (ulong)n2, (ulong)d1);
        }

        public static bool operator >(RationalNumber rn1, RationalNumber rn2)
        {
            return !(rn1 == rn2) && !(rn1 < rn2);
        }

        public static bool operator <=(RationalNumber rn1, RationalNumber rn2)
        {
            return (rn1 == rn2) || (rn1 < rn2);
        }

        public static bool operator >=(RationalNumber rn1, RationalNumber rn2)
        {
            return (rn1 == rn2) || (rn1 > rn2);
        }

        public int CompareTo(RationalNumber other)
        {
            if (this < other)
                return -1;
            if (this == other)
                return 0;
            return 1;
        }

        public static RationalNumber operator -(RationalNumber rn)
        {
            checked
            {
                return new RationalNumber(-rn.Numerator, rn.Denominator);
            }
        }

        public static RationalNumber operator +(RationalNumber rn1, RationalNumber rn2)
        {
            // (a*b + c*d)/(e*f) = (n1*d2 + n2*d1)/(d1*d2)

            var a = rn1.Numerator;
            var b = rn2.Denominator;
            var c = rn2.Numerator;
            var d = rn1.Denominator;
            var e = rn1.Denominator;
            var f = rn2.Denominator;

            var gcd = GreatestCommonDenominator(e, f);
            if (gcd != 1)
            {
                b /= gcd;
                d /= gcd;
                e /= gcd;
            }

            checked
            {
                return new RationalNumber(a * b + c * d, e * f);
            }
        }

        public static RationalNumber operator -(RationalNumber rn1, RationalNumber rn2)
        {
            return rn1 + (-rn2);
        }

        public static RationalNumber operator *(RationalNumber rn1, RationalNumber rn2)
        {
            var n1 = rn1.Numerator;
            var d1 = rn1.Denominator;
            var n2 = rn2.Numerator;
            var d2 = rn2.Denominator;

            var gcd = GreatestCommonDenominator(n1, d2);
            if (gcd != 1)
            {
                n1 /= gcd;
                d2 /= gcd;
            }

            gcd = GreatestCommonDenominator(n2, d1);
            if (gcd != 1)
            {
                n2 /= gcd;
                d1 /= gcd;
            }

            checked
            {
                return new RationalNumber(n1 * n2, d1 * d2);
            }
        }

        public static RationalNumber operator /(RationalNumber rn1, RationalNumber rn2)
        {
            if(rn2.Numerator == 0)
                throw new DivideByZeroException();

            var n1 = rn1.Numerator;
            var d1 = rn1.Denominator;
            var n2 = rn2.Numerator;
            var d2 = rn2.Denominator;

            var gcd = GreatestCommonDenominator(n1, n2);
            if (gcd != 1)
            {
                n1 /= gcd;
                n2 /= gcd;
            }

            gcd = GreatestCommonDenominator(d1, d2);
            if (gcd != 1)
            {
                d1 /= gcd;
                d2 /= gcd;
            }

            checked
            {
                return new RationalNumber(n1 * d2, d1 * n2);
            }
        }

        public static implicit operator RationalNumber(byte value)
        {
            return new RationalNumber(value, 1);
        }

        public static implicit operator RationalNumber(sbyte value)
        {
            return new RationalNumber(value, 1);
        }

        public static implicit operator RationalNumber(Int16 value)
        {
            return new RationalNumber(value, 1);
        }

        public static implicit operator RationalNumber(UInt16 value)
        {
            return new RationalNumber(value, 1);
        }

        public static implicit operator RationalNumber(Int32 value)
        {
            return new RationalNumber(value, 1);
        }

        public static implicit operator RationalNumber(UInt32 value)
        {
            return new RationalNumber(value, 1);
        }

        public static implicit operator RationalNumber(Int64 value)
        {
            return new RationalNumber(value, 1);
        }

        public static implicit operator RationalNumber(UInt64 value)
        {
            if (value > long.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value));

            return new RationalNumber((long)value, 1);
        }

        public static explicit operator double(RationalNumber value)
        {
            var n = (double)value.Numerator;
            var d = (double)value.Denominator;

            return n / d;
        }

        public static explicit operator decimal(RationalNumber value)
        {
            var n = (decimal)value.Numerator;
            var d = (decimal)value.Denominator;

            return n / d;
        }

        public static explicit operator float(RationalNumber value)
        {
            var n = (float)value.Numerator;
            var d = (float)value.Denominator;

            return n / d;
        }

        /// <summary>
        /// Returns absolute value of this number.
        /// </summary>
        public RationalNumber Abs()
        {
            if (Numerator >= 0)
                return this;

            return -this;
        }
    }
}
