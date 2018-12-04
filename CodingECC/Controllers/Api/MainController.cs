using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodingECC.Models;

namespace CodingECC.Controllers.Api
{
    [RoutePrefix("api/main")]
    public class MainController : ApiController
    {
        [Route("index")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            ECCurve eCCurve = new ECCurve
            {
                A = 5,
                B = 2,
                G = new ECPoint { X = 4, Y = 4, Z = 1 },
                Prime = 7
            };
            eCCurve.FindOrder();
            return Ok(eCCurve);
        }

        [Route("calculate")]
        [HttpPost]
        public IHttpActionResult CalculateSharedKey(DHKeyModule dHKeyModule)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("The form data was corrupted.");
            }

            ECCurve eCCurve = dHKeyModule.ECCurve;

            int a = dHKeyModule.KeyLittleA;
            int b = dHKeyModule.KeyLittleB;
            int o = eCCurve.Order;
            ECPoint G = eCCurve.G;

            ECPoint aG = eCCurve.Multiply(a - 1, G);
            ECPoint bG = eCCurve.Multiply(b - 1, G);

            ECPoint M1 = eCCurve.Multiply(b, aG);
            ECPoint M2 = eCCurve.Multiply(a, bG);

            dHKeyModule = new DHKeyModule
            {
                KeyLittleA = a,
                KeyLittleB = b,
                Order = o,
                AG = aG.ToString(),
                BG = bG.ToString(),
                M1 = M1.ToString(),
                M2 = M2.ToString()
            };

            return Ok(dHKeyModule);
        }

        [Route("update")]
        [HttpPost]
        public IHttpActionResult UpdateMasterCurve(ECCurve newCurve)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The form data was corrupted.");
            }

            if (!newCurve.HasPrime())
            {
                return BadRequest(newCurve.Prime.ToString() + " is not prime.");
            }

            if (!newCurve.HasValidCoefficients())
            {
                return BadRequest("The discriminant of this curve is zero. Please pick different coefficients.");
            }

            if (!newCurve.HasBasepointOnCurve())
            {
                return BadRequest("The point you chose is not on your selected curve.");
            }

            newCurve.FindOrder();
            return Ok(newCurve);
        }
    }
}
