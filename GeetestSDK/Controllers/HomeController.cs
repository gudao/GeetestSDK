using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeetestSDK.Models;
using GeetestSDK.Lib;
using Microsoft.AspNetCore.Http;

namespace GeetestSDK.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getCaptcha()
        {
            var r= GeetestHelper.registerChallenge("h5");
            //服务器端
            // r.success
            return Json(r);
        }

        [HttpPost]
        public IActionResult Index(string challenge,string validate,string seccode)
        {
            var r = GeetestHelper.enhencedValidateRequest(challenge, validate, seccode, true);
            return Json(new { result=r });
        }



        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
