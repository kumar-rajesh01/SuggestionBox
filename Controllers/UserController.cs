using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using SuggestionBox.Data;
using SuggestionBox.Interface;
using SuggestionBox.Models;
using SuggestionBox.Service;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SuggestionBox.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private static readonly ILog log = LogManager.GetLogger(typeof(UserController));

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login([FromServices] IUserService userService, LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var data = userService.Login(loginViewModel);
                if (data != null)
                {
                    var claims = new Claim[]
                    {
                        new Claim("UserId", data.UserId.ToString()),
                        new Claim(ClaimTypes.Name, data.Name),
                        new Claim("Email", data.Email),
                        new Claim("Role", data.Role),
                    };
                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                    return RedirectToAction("Admin");
                }
                else
                    TempData["ErrorMessage"] =  "Unable to login. " + string.Join(",", ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList());
            }
            else
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
            }

            //var data = Results.SignIn(claimsPrincipal);
            return View(loginViewModel);
        }

        /// <summary>
        /// Get Suggestion List
        /// </summary>
        /// <param name="userService"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Admin([FromServices] IUserService userService)
        {
            try
            {
                var user = HttpContext.User;
                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    return View(userService.GetSuggestionList());
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(new List<Suggestion>());
            }
        }

        /// <summary>
        /// Admin Logout
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login"); // Change to your desired action and controller
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> DownloadFile([FromServices] IUserService userService, int id)
        {
            var suggestion = userService.GetSuggestionById(id);
            if (suggestion == null || string.IsNullOrEmpty(suggestion.File))
            {
                TempData["ErrorMessage"] = "File not found!";
                //return NotFound();
                return RedirectToAction("Admin");
            }
            else
            {
                var fileBytes = Convert.FromBase64String(suggestion.File);
                var contentType = GetContentType(suggestion.File);

                TempData["SuccessMessage"] = "File downloaded successfully!";
                return File(fileBytes, contentType, $"{suggestion.FileName}{Path.GetExtension(suggestion.File)}");
            }
        }

        /// <summary>
        /// Save admin comment
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveComment([FromServices] IUserService userService, SaveCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUpdated = userService.UpdateSuggestion(model);
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Admin action saved successfully!";
                    return RedirectToAction("Admin");
                }
                else
                    TempData["ErrorMessage"] = "Unable to save admin action. " + string.Join(",", ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList());
            }
            else
            {
                TempData["ErrorMessage"] = "Error occured in saving admin action!";
            }
            return View(userService.GetSuggestionList());
        }

        private string GetContentType(string base64String)
        {
            if (base64String.StartsWith("data:image/jpeg;base64,"))
                return "image/jpeg";
            if (base64String.StartsWith("data:image/png;base64,"))
                return "image/png";
            if (base64String.StartsWith("data:application/pdf;base64,"))
                return "application/pdf";
            if (base64String.StartsWith("data:application/msword;base64,"))
                return "application/msword";
            if (base64String.StartsWith("data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,"))
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            return "application/octet-stream";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
