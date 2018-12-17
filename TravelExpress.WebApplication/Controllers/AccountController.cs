namespace TravelExpress.WebApplication.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            var redirectUrl = Url.Page("/Index");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                //OpenIdConnectDefaults.AuthenticationScheme
                AzureADDefaults.AuthenticationScheme
            );
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Page("/Account/SignedOut", pageHandler: null, values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                //CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme
                AzureADDefaults.AuthenticationScheme, AzureADDefaults.OpenIdScheme, AzureADDefaults.CookieScheme
            );
        }
    }
}