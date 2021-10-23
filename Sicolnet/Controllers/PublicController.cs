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


        public JsonResult TestRecortador(string url)
        {
            
            return Json(UrlShorter.ShortUrl(url));
        }
    }
}
