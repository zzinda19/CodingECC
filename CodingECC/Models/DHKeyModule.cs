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
        [Required]
        public ECCurve ECCurve { get; set; }

        public int Order { get; set; }

        public string AG { get; set; }
        public string BG { get; set; }
        public string M1 { get; set; }
        public string M2 { get; set; }
    }
}