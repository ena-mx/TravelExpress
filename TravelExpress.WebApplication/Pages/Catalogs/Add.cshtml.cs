namespace TravelExpress.WebApplication.Pages.Catalogs
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Products;
    using TravelExpress.Entities.Products;

    public class AddModel : BasePageModel
    {
        [BindProperty]
        public AddUpdateProductRequest Product { get; set; }

        public void OnGet()
        {

        }

        public async Task<ActionResult> OnPostAsync([FromServices]IProductComponentFactory productComponentFactory)
        {
            if (!ModelState.IsValid)
                return Page();
            IProductComponent product = productComponentFactory.New();

            WorkflowResult result = await product.SaveAsync(Product.Description);

            if (!result.Success)
            {
                for (int i = 0; i < result.Errors.Length; i++)
                {
                    ModelState.TryAddModelError("", result.Errors[i]);
                }
                return Page();
            }
            
            return RedirectToPage("./Details", new { id = product.ProductId });
        }
    }
}