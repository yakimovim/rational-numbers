using System;

namespace EdlinSoftware.RationalNumbers
{
    /// <summary>
    /// Contains logic for comparison of two multiplications.
    /// </summary>
    internal static class ComparerOfMultiplications
    {
        private class Triple
        {
            public ulong Hi { get; set; }
            public ulong Middle { get; set; }
            public ulong Lo { get; set; }
        }

        private class Halves
        {
            public ulong Hi { get; set; }
            public ulong Lo { get; set; }
        }

        /// <summary>
        /// Checks if <paramref name="a"/>*<paramref name="b"/> is less than <paramref name="c"/>*<paramref name="d"/>.
        /// </summary>
        public static bool IsLess(ulong a, ulong b, ulong c, ulong d)
        {
            var ab = Get32BitTriple(a, b);
            var cd = Get32BitTriple(c, d);

            if (ab.Hi < cd.Hi)
                return true;
            if (ab.Hi > cd.Hi)
                return false;

            if (ab.Middle < cd.Middle)
                return true;
            if (ab.Middle > cd.Middle)
                return false;

            if (ab.Lo < cd.Lo)
                return true;

            return false;
        }

        private static Triple Get32BitTriple(ulong a, ulong b)
        {
            // a = a0 + n*a1
            // b = b0 + n*b1

            var aHalves32 = Get32Halves(a);
            var bHalves32 = Get32Halves(b);

            // a*b = a0*b0 + (a0*b1 + a1*b0)*n + a1*b1*n*n

            var a0b0Halves32 = Get32Halves(aHalves32.Lo * bHalves32.Lo);

            var a0b1Halves32 = Get32Halves(aHalves32.Lo * bHalves32.Hi);
            var a1b0Halves32 = Get32Halves(aHalves32.Hi * bHalves32.Lo);

            checked
            {
                var sumLo = a0b1Halves32.Lo + a1b0Halves32.Lo + a0b0Halves32.Hi;
                var sumLoHalves32 = Get32Halves(sumLo);
                var sumHi = a0b1Halves32.Hi + a1b0Halves32.Hi + sumLoHalves32.Hi;

                var hi = aHalves32.Hi * bHalves32.Hi + sumHi;

                return new Triple
                {
                    Hi = hi,
                    Middle = sumLoHalves32.Lo,
                    Lo = a0b0Halves32.Lo
                };
            }

        }

        private static Halves Get32Halves(ulong v)
        {
            return new Halves
            {
                Hi = v >> 32,
                Lo = v & 0xFFFFFFFF
            };
        }
    }
}