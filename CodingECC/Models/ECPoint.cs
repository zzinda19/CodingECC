using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodingECC.Models
{
    public class ECPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public static ECPoint O = new ECPoint { X = 0, Y = 1, Z = 0 };

        public bool Equals(ECCPoint P)
        {
            return (X == P.X && Y == P.Y && Z == P.Z);
        }
    }
}