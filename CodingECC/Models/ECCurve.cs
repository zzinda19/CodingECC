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
            for (int i = 0; i < numTimes; i++)
            {
                P = Summand(G, P);
            }
            return P;
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
            int m2 = (int)BigInteger.ModPow(m, 2, Prime);
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

        //////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////

        // Validation Methods For User Input //

        //////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////

        public bool HasValidCoefficients()
        {
            int a3 = (int)Math.Pow(A, 3);
            int b2 = (int)Math.Pow(B, 2);
            return ((4 * a3) + (27 * b2)) != 0;
        }

        public bool HasPrime()
        {
            if (Prime < 5)
            {
                switch (Prime)
                {
                    case 2:
                    case 3:
                        return true;
                    default:
                        return false;
                }
            }
            if (Prime % 2 == 0 || Prime % 3 == 0)
            {
                return false;
            }

            int boundary = (int)Math.Floor(Math.Sqrt(Prime));

            for (int i = 5; i <= boundary; i += 2)
            {
                if (Prime % i == 0)
                {
                    return false;
                } 
            }
            return true;
        }

        public bool HasBasepointOnCurve()
        {
            int y2 = (int)BigInteger.ModPow(G.Y, 2, Prime);
            int x3 = (int)BigInteger.ModPow(G.X, 3, Prime);
            x3 += (A * G.X) + B;
            x3 = Modulo(x3, Prime);
            return y2 == x3;
        }
    }
}