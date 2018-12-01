using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Web;

namespace CodingECC.Models
{
    public class ECCurve
    {
        public int A { get; set; }
        public int B { get; set; }
        public ECPoint G { get; set; }
        public int Prime { get; set; }
        public int Order { get; set; }

        public void FindOrder()
        {
            int counter = 1;
            ECPoint P = G;
            while (!P.Equals(ECPoint.O))
            {
                P = Summand(G, P);
                counter++;
            }
            Order = counter;
        }

        public ECPoint Multiply(int numTimes, ECPoint P)
        {
            if (numTimes == 1)
            {
                return P;
            }
            P = Summand(G, P);
            return Multiply(numTimes - 1, P);
        }

        public ECPoint Summand(ECPoint P, ECPoint Q)
        {
            if (P.Equals(ECPoint.O))
            {
                return Q;
            }
            if (Q.Equals(ECPoint.O))
            {
                return P;
            }
            if (Inverses(P, Q))
            {
                return ECPoint.O;
            }

            int m = CalculateSlope(P, Q);

            if (m == -1)
            {
                return ECPoint.O;
            }

            int x, y;
            int m2 = (int)Math.Pow(m, 2);
            x = m2 - Q.X - P.X;
            x = Modulo(x, Prime);
            y = -m * (x - P.X) - P.Y;
            y = Modulo(y, Prime);

            return new ECPoint { X = x, Y = y, Z = 1 };
        }

        public int CalculateSlope(ECPoint P, ECPoint Q)
        {
            int top, bottom;

            if (P.X == Q.X)
            {
                int x = (int)Math.Pow(P.X, 2);
                top = (3 * x) + A;
                top = Modulo(top, Prime);
                bottom = 2 * P.Y;
                bottom = Modulo(bottom, Prime);
            }
            else
            {
                top = Q.Y - P.Y;
                top = Modulo(top, Prime);
                bottom = Q.X - P.X;
                bottom = Modulo(bottom, Prime);
            }

            if (BigInteger.GreatestCommonDivisor(bottom, Prime) != 1)
            {
                return -1;
            }

            int bottomInverse = (int)BigInteger.ModPow(bottom, Prime - 2, Prime);
            int slope = top * bottomInverse;
            slope = Modulo(slope, Prime);

            return slope;
        }

        public bool Inverses(ECPoint P, ECPoint Q)
        {
            return (P.X == Q.X && Modulo(-P.Y, Prime) == Q.Y && P.Z == Q.Z);
        }

        public static int Modulo(int a, int n)
        {
            return (a % n + n) % n;
        }
    }
}