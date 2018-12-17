namespace TravelExpress.WebApplication.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        public string Message { get; set; } = "Initial Request";

        public void OnGet()
        {

        }

        public void OnPost()
        {
            Message = "Form Posted";
        }

        public void OnPostDelete()
        {
            Message = "Delete handler fired";
        }

        public void OnPostEdit(int id)
        {
            Message = "Edit handler fired";
        }

        public void OnPostView(int id)
        {
            Message = $"View handler fired for {id}";
        }
    }
}
