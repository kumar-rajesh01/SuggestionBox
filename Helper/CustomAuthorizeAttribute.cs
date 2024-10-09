using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuggestionBox.Helper;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (string.IsNullOrEmpty(CredentialManager.ValidateCredential()))
        {
            context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
        }
    }
}