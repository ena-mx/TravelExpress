namespace TravelExpress.WebApplication.Pages.Customers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Queries.Customers;
    using TravelExpress.Queries.Products;

    public class AddOrderModel : BasePageModel
    {
        private readonly CustomersQueryComponent _customersQueryComponent;
        private readonly ProductQueryComponent _productQueryComponent;
        private readonly IDateComponent _dateComponent;

        public Customer Customer { get; set; }
        public ProductExcursionIndex[] ProductExcursions { get; set; }

        [BindProperty]
        public Guid Id { get; set; }

        public AddOrderModel(
            CustomersQueryComponent customersQueryComponent, 
            ProductQueryComponent productQueryComponent,
            IDateComponent dateComponent)
        {
            _customersQueryComponent = customersQueryComponent ?? throw new ArgumentNullException(nameof(customersQueryComponent));
            _productQueryComponent = productQueryComponent ?? throw new ArgumentNullException(nameof(productQueryComponent));
            _dateComponent = dateComponent ?? throw new ArgumentNullException(nameof(dateComponent));
        }

        public async Task<ActionResult> OnGetAsync(Guid id)
        {
            this.Id = id;
            Customer = await _customersQueryComponent.CustomerAsync(Id);
            if (Customer == null)
                return RedirectToPage("/NotFound");
            ProductExcursions = await _productQueryComponent.ProductExcursionsAsync(_dateComponent.ServerDate.Date);
            return Page();
        }
    }
}