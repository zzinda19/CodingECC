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

    }
}