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
            eCCurve.FindOrder();

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
            ECPoint G = eCCurve.G;

            ECPoint aG = eCCurve.Multiply(a, G);
            ECPoint bG = eCCurve.Multiply(b, G);

            ECPoint M1 = eCCurve.Multiply(b, aG);
            ECPoint M2 = eCCurve.Multiply(a, bG);

            dHKeyModule = new DHKeyModule
            {
                KeyLittleA = a,
                KeyLittleB = b,
                AG = aG,
                BG = bG,
                M1 = M1,
                M2 = M2
            };

            return Ok(dHKeyModule);
        }

        [NonAction]
        private ECCurve GetECCurve()
        {
            ECCurve eCCurve = new ECCurve
            {
                A = 5,
                B = 2,
                G = new ECPoint { X = 4, Y = 4, Z = 1 },
                Prime = 7
            };

            return eCCurve;
        }
    }
}
