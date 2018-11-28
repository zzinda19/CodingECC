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
            var viewModel = new MainViewModel();
            return View(viewModel);
        }

        // POST: Main/Calculate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Calculate(ECCModule eCCModule)
        {
            long a = eCCModule.LittleA;
            long b = eCCModule.LittleB;

            ECCPoint basePoint = eCCModule.BasePoint;

            ECCPoint A = basePoint.Multiply(a, basePoint, basePoint);
            ECCPoint B = basePoint.Multiply(b, basePoint, basePoint);
            ECCPoint M1 = A.Multiply(b, A, A);
            ECCPoint M2 = B.Multiply(a, B, B);

            var viewModel = new MainViewModel
            {
                LittleA = a,
                LittleB = b
            };

            viewModel.BigA = A;
            viewModel.BigB = B;
            viewModel.BigM1 = M1;
            viewModel.BigM2 = M2;

            return View("Index", viewModel);
        }
    }
}