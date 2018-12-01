using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodingECC.Models;
using CodingECC.ViewModels;

namespace CodingECC.Controllers
{
    public class MainController : Controller
    {

        // GET: Main
        public ActionResult Index()
        {
            Console.Write("Printing");
            return View();
        }
    }
}