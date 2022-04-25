using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSIT133Site.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MSIT133Site.Controllers
{
    public class HomeController : Controller
    {
        //dependency injection
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FirstAjax()
        {
            return View();
        }
        public IActionResult AjaxPost()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AjaxPost(User user)
        {
            ViewBag.name = user.UserName;
            return View();
        }
        public IActionResult Promise()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Adderss()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult AjaxFormData()
        {
            return View();
        }
    }
}
