namespace TravelExpress.WebApplication.Pages.Customers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Customers;
    using TravelExpress.Queries.Products;

    public class AddOrderModel : BasePageModel
    {
        private readonly CustomersQueryComponent _customersQueryComponent;
        private readonly ProductQueryComponent _productQueryComponent;

        public Customer Customer { get; set; }
        public ProductExcursionIndex[] ProductExcursions { get; set; }

        [BindProperty]
        public Guid Id { get; set; }

        public AddOrderModel(CustomersQueryComponent customersQueryComponent, ProductQueryComponent productQueryComponent)
        {
            _customersQueryComponent = customersQueryComponent ?? throw new ArgumentNullException(nameof(customersQueryComponent));
            _productQueryComponent = productQueryComponent ?? throw new ArgumentNullException(nameof(productQueryComponent));
        }

        public async Task<ActionResult> OnGetAsync(Guid id)
        {
            this.Id = id;
            Customer = await _customersQueryComponent.CustomerAsync(Id);
            if (Customer == null)
                return RedirectToPage("/NotFound");
            ProductExcursions = await _productQueryComponent.ProductExcursionsAsync();
            return Page();
        }
    }
}