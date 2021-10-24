using Microsoft.AspNetCore.Mvc;
using Sicolnet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Controllers
{
    public class PublicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> TestShorter()
        {
            try
            {

                UrlShorterResponse response = await UrlShorter.ShortUrl("https://localhost:44319/Registro/Index?Id=Uk1DaUN4WXIzSEU4NnBjQ3dkZkRDQT09");
                return Json(response);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
       
        }
    }
}
