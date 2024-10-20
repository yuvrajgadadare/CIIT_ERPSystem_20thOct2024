using CIIT_NEW_WithCore.Models;
//using CIIT_NEW_WithCore.Services.Implementations;
//using CIIT_NEW_WithCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CIIT_NEW_WithCore.Controllers
{
    public class LoginController : Controller
    {
        //IAdminService adminService;
        //RSACryptoServiceProvider rsaCryptoServiceProvider;
        //IExtraService extraService;
        //public LoginController(IAdminService adminService, IExtraService extraService)
        //{
        //    this.adminService = adminService;
        //    rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048);
        //    this.extraService = extraService;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Index(TbladminDetail ad)
        //{
        //    Task<TbladminDetail> d = adminService.CheckLogin(ad.UserName, ad.Password);
        //    if (d != null)
        //    {
        //        var encryptid = extraService.Encrypt(d.Id.ToString(), rsaCryptoServiceProvider.ExportParameters(false));
        //        SetCookie("admin", encryptid.ToString(), 30);

        //        return Redirect("/Admin/Dashboard");
        //    }
        //    else
        //    {
        //        ViewBag.msg = "Invalid user name or password";
        //        return View();
        //    }
        //}

        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Remove("admin");
        //    return Redirect("/");
        //}



        //public string GetCookie(string key)
        //{
        //    return Request.Cookies[key];
        //}

        //public void SetCookie(string key, string value, int? expireTime)
        //{
        //    CookieOptions option = new CookieOptions();
        //    if (expireTime.HasValue)
        //        option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
        //    else
        //        option.Expires = DateTime.Now.AddMilliseconds(10);
        //    Response.Cookies.Append(key, value, option);
        //}
        //public void Remove(string key)
        //{
        //    Response.Cookies.Delete(key);
        //}


    }
}
