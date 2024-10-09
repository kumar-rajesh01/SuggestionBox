using GCrypt;
using log4net;
using Microsoft.AspNetCore.Mvc;
using SuggestionBox.Interface;
using SuggestionBox.Models;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SuggestionBox.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Suggestion()
        {
            log.Info("Index page is called.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Suggestion([FromServices] IUserService userService, SuggestionModel suggestion)
        {
            try
            {
                if (!string.IsNullOrEmpty(suggestion.Text))
                {
                    if (suggestion?.File != null)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx" };
                        var fileExtension = Path.GetExtension(suggestion.File.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("ProfilePicture", "Invalid file type. Allowed types: jpg, jpeg, png, pdf, doc, docx.");
                            TempData["ErrorMessage"] = "Invalid file type!";
                            return View(suggestion);
                        }
                    }

                    var isSuccess = await userService.SaveSuggestion(suggestion);
                    if (isSuccess)
                    {
                        TempData["SuccessMessage"] = "Suggestion submitted successfully!";
                        return RedirectToAction("Suggestion");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in saving the suggestion", ex);
                throw;
            }
            return View();
        }

        public IActionResult Encrypt(string input)
        {
            return new ContentResult
            {
                Content = GCrypter.Encrypt(input),
                ContentType = "application/json"
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
