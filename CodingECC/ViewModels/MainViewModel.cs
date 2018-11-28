using System;
using System.Collections.Generic;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CodingECC.Models;

namespace CodingECC.ViewModels
{
    public class MainViewModel
    {
        [Required]
        [Display(Name = "Choose a:")]
        public long LittleA { get; set; }
        [Required]
        [Display(Name = "Choose b:")]
        public long LittleB { get; set; }

        public ECCPoint BigA { get; set; }
        public ECCPoint BigB { get; set; }
        public ECCPoint BigM1 { get; set; }
        public ECCPoint BigM2 { get; set; }

        public MainViewModel()
        {
            BigA = new ECCPoint();
            BigB = new ECCPoint();
            BigM1 = new ECCPoint();
            BigM2 = new ECCPoint();
        }
    }
}