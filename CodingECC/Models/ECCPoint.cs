using System;
using System.Collections.Generic;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodingECC.Models
{
    public class ECCPoint
    {
        [Required]
        public long X { get; set; }
        [Required]
        public long Y { get; set; }
        [Required]
        public long Z { get; set; }
        public static long Prime = ECCModule.PrimeP;
        public static long A = ECCModule.CurveA;

        public ECCPoint()
        {
            X = 0;
            Y = 0;
            Z = 1;
        }

        public ECCPoint(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public ECCPoint Summand(ECCPoint P, ECCPoint Q)
        {
            ECCPoint O = new ECCPoint(0, 1, 0);

            if (P.Equals(O))
            {
                return Q;
            }
            if (Q.Equals(O))
            {
                return P;
            }
            if (P.HasInverse(Q))
            {
                return O;
            }

            long m;

            if (P.X == Q.X)
            {
                m = P.FindSlope();
            }
            else
            {
                m = P.FindSlope(Q);
            }

            if (m == -1)
            {
                return O;
            }

            long M = (long)Math.Pow(m, 2);
            long x = M - P.X - Q.X;
            x = Modulo(x, Prime);

            long y = -m * (x - P.X) - P.Y;
            y = Modulo(y, Prime);

            return new ECCPoint(x, y, 1);
        }

        public long FindSlope()
        {
            long x = (long)Math.Pow(this.X, 2);
            long top = (3 * x) + A;
            long bottom = (2 * this.Y);
            return CheckSlope(top, bottom, Prime);
        }

        public long FindSlope(ECCPoint Q)
        {
            long top = Q.Y - this.Y;
            long bottom = Q.X - this.X;
            return CheckSlope(top, bottom, Prime);
        }

        public long CheckSlope(long top, long bottom, long prime)
        {
            top = Modulo(top, prime);
            if (BigInteger.GreatestCommonDivisor(bottom, prime) != 1)
            {
                return -1;
            }
            long bottomInverse = (long)BigInteger.ModPow(bottom, prime - 2, prime);
            long slope = top * bottomInverse;
            slope = Modulo(slope, prime);
            return slope;
        }

        public ECCPoint Multiply(long x, ECCPoint P, ECCPoint Q)
        {
            if (x == 1)
            {
                return Q;
            }
            ECCPoint R = Summand(P, Q);
            return Multiply(x-1, P, R);
        }

        public bool Equals(ECCPoint Q)
        {
            return (this.X == Q.X && this.Y == Q.Y && this.Z == Q.Z);
        }

        public bool HasInverse(ECCPoint Q)
        {
            long y = -this.Y;
            y = Modulo(y, Prime);
            return (this.X == Q.X && y == Q.Y && this.Z == Q.Z);
        }

        public static long Modulo(long x, long m)
        {
            return (x % m + m) % m;
        }
    }
}