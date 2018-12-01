using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodingECC.Models;

namespace CodingECC.Controllers.Api
{
    public class MainController : ApiController
    {

        [HttpGet]
        public IHttpActionResult Index()
        {

            ECCurve eCCurve = GetECCurve();
            return Ok(eCCurve);
        }

        [HttpPost]
        public IHttpActionResult CalculateSharedKey(DHKeyModule dHKeyModule)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("The form data was not received correctly.");
            }

            ECCurve eCCurve = GetECCurve();

            int a = dHKeyModule.KeyLittleA;
            int b = dHKeyModule.KeyLittleB;
            int o = eCCurve.Order;
            ECPoint G = eCCurve.G;

            ECPoint aG = eCCurve.Multiply(a-1, G);
            ECPoint bG = eCCurve.Multiply(b-1, G);

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

        [NonAction]
        private ECCurve GetECCurve()
        {
            ECCurve eCCurve = new ECCurve
            {
                A = 13,
                B = -13,
                G = new ECPoint { X = 1, Y = 1, Z = 1 },
                Prime = 10007
            };

            eCCurve.FindOrder();

            return eCCurve;
        }
    }
}
