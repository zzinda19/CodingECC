using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodingECC.Models
{
    public class DHKeyModule
    {
        [Required]
        public int KeyLittleA { get; set; }
        [Required]
        public int KeyLittleB { get; set; }

        public ECPoint AG { get; set; }
        public ECPoint BG { get; set; }
        public ECPoint M1 { get; set; }
        public ECPoint M2 { get; set; }
    }
}