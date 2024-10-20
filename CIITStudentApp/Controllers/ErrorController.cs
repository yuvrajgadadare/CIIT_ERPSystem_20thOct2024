using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CIITStudentApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statuscoderesult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    {
                        ViewBag.errorMessage = "Sorry, the resource you requested colud not found";
                        ViewBag.path = statuscoderesult.OriginalPath;
                        ViewBag.QS = statuscoderesult.OriginalQueryString;
                        break;
                    }
            }
            return View("NotFound");
        }
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.StackTrace = exceptionDetails.Error.StackTrace;
            return View("Error");
        }
    }
}
