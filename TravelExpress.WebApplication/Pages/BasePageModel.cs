namespace TravelExpress.WebApplication.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;

    public abstract class BasePageModel : PageModel
    {
        private static readonly string _objectIdentifierKey = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        internal Guid UserId()
        {
            string oib = User.FindFirst(c => c.Type.Equals(_objectIdentifierKey, StringComparison.OrdinalIgnoreCase))?.Value;
            Guid guid = Guid.Empty;
            Guid.TryParse(oib, out guid);
            return guid;
        }

        public string UserName()
        {
            return User.FindFirst(c => c.Type.Equals("name", StringComparison.OrdinalIgnoreCase))?.Value ?? User.Identity?.Name ?? "Unknown";
        }
    }
}
