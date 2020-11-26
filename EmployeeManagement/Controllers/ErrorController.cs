using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "The requested resource could not be found.";
                    _logger.LogWarning($"404 Error Occured. Path = {statusCodeResult.OriginalPath} and QueryString = {statusCodeResult.OriginalQueryString}");
                    // Find the Web Root folder
                    // Create error log directory
                    // If errorLog.txt doesn't exist, create it and add error message with date and time
                    
                    break;
                default:
                    break;
            }
            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error}");

            return View("Error");
        }
    }
}
