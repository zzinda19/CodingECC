using System;
using System.Collections.Generic;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CodingECC.Models;

namespace CodingECC.Models
{
    public class ECCModule
    {
        [Required]
        public long LittleA { get; set; }
        [Required]
        public long LittleB { get; set; }

        public const long CurveA = 5;
        public const long CurveB = 2;
        public const long PrimeP = 7;
        public ECCPoint BasePoint = new ECCPoint(4, 4, 1);
    }
}