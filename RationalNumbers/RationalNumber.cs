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
        /// <exception cref="System.ArgumentOutOfRangeException">Denominator or numerator is <see cref="long.MinValue"/>.</exception>
        public RationalNumber(long numerator, long denominator)
        {
            if (denominator == 0L) throw new ArgumentOutOfRangeException(nameof(denominator));
            if (denominator == long.MinValue) throw new ArgumentOutOfRangeException(nameof(denominator));
            if (numerator == long.MinValue) throw new ArgumentOutOfRangeException(nameof(numerator));

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
                n1 = -n1;
                n2 = -n2;
            }

            return inverse ? !IsLess(n1, d2, n2, d1) : IsLess(n1, d2, n2, d1);
        }

        /// <summary>
        /// Checks if <paramref name="a"/>*<paramref name="b"/> is less than <paramref name="c"/>*<paramref name="d"/>.
        /// </summary>
        private static bool IsLess(long a, long b, long c, long d)
        {
            // Solve: (a*b < c*d) 

            // a = a0 + n*a1
            // b = b0 + n*b1
            // c = c0 + n*c1
            // d = d0 + n*d1
            // n = 2^32

            ulong a0 = (ulong)a & 0xFFFFFFFF;
            ulong a1 = (ulong)a >> 32;
            ulong b0 = (ulong)b & 0xFFFFFFFF;
            ulong b1 = (ulong)b >> 32;
            ulong c0 = (ulong)c & 0xFFFFFFFF;
            ulong c1 = (ulong)c >> 32;
            ulong d0 = (ulong)d & 0xFFFFFFFF;
            ulong d1 = (ulong)d >> 32;

            // Solve: a0*b0 + (a0*b1 + a1*b0)*n + a1*b1*n*n < c0*d0 +(c0*d1 + c1*d0)*n + c1*d1*n*n
            UInt64 x00 = a0 * b0;
            UInt64 x01 = c0 * d0;

            // this one may overflow, so split them up even more:
            ulong x10 = (a0 * b1) & 1 + (a1 * b0) & 1; // lower bit
            ulong x11 = (c0 * d1) & 1 + (c1 * d0) & 1; // lower bit
            ulong x20 = a0 * b1 / 2 + a1 * b0 / 2; // top 63 bits
            ulong x21 = c0 * d1 / 2 + c1 * d0 / 2; // top 63 bits
            if (x10 == 2)
            {
                x20++;
                x10 = 0;
            }
            if (x11 == 2)
            {
                x21++;
                x11 = 0;
            }

            UInt64 x30 = a1 * b1;
            UInt64 x31 = c1 * d1;

            if (x30 < x31)
                return true;
            else if (x30 > x31)
                return false;
            else if (x20 < x21)
                return true;
            else if (x20 > x21)
                return false;
            else if (x10 < x11)
                return true;
            else if (x10 > x11)
                return false;
            else if (x00 < x01)
                return true;
            else
                return false;
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

        public static RationalNumber operator -(RationalNumber rn)
        {
            return new RationalNumber(-rn.Numerator, rn.Denominator);
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

            return new RationalNumber(a * b + c * d, e * f);
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

            return new RationalNumber(n1 * n2, d1 * d2);
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

            return new RationalNumber(n1 * d2, d1 * n2);
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
    }
}
