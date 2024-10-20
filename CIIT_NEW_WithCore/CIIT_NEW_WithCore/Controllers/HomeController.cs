//using CIIT_NEW_WithCore.Models;
using CIIT_NEW_WithCore.Models;
//using CIIT_NEW_WithCore.Services.Implementations;
//using CIIT_NEW_WithCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Diagnostics;

namespace CIIT_NEW_WithCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private IEmailService extraService;

        //public HomeController(ILogger<HomeController> logger,IEmailService extraService)
        //{
        //    _logger = logger;
        //    this.extraService = extraService;
        //}
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //public IActionResult SendEmail()
        //{
        //    EmailModel em = new EmailModel() 
        //    {
        //     UserName="Yuvraj Gadadare",
        //      EmailAddress="yuvraj.gadadare@gmail.com",
        //       Subject="Test Email",
        //        Message="This is Test Email"
        //    };
        //    extraService.SendEmail(em);
        //    return View();
        //}
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
