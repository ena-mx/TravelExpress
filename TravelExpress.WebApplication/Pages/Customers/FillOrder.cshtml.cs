namespace TravelExpress.WebApplication.Pages.Customers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Customers;
    using TravelExpress.Queries.Products;


    public class AditionalServicesRequest
    {
        [Required]
        public int AditionalServiceId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }

    public class PriceDetailRequest
    {
        [Required]
        public int PriceDetailId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }

    public class FillOrderRequest
    {
        public List<AditionalServicesRequest> AditionalServices { get; set; }
        public List<PriceDetailRequest> PriceDetails { get; set; }
    }

    public class FillOrderModel : BasePageModel
    {
        private readonly CustomersQueryComponent _customersQueryComponent;
        private readonly ProductQueryComponent _productQueryComponent;

        [BindProperty]
        public Guid CustomerId { get; set; }
        [BindProperty]
        public Guid ExcursionId { get; set; }
        [BindProperty]
        public FillOrderRequest FillOrderRequest { get; set; }

        public Customer Customer { get; set; }
        public ExcursionDetail ExcursionDetail { get; set; }

        public FillOrderModel(CustomersQueryComponent customersQueryComponent, ProductQueryComponent productQueryComponent)
        {
            _customersQueryComponent = customersQueryComponent ?? throw new ArgumentNullException(nameof(customersQueryComponent));
            _productQueryComponent = productQueryComponent ?? throw new ArgumentNullException(nameof(productQueryComponent));
        }


        public async Task<ActionResult> OnGetAsync(Guid customerId, Guid excursionId)
        {
            if(customerId == Guid.Empty || excursionId == Guid.Empty)
                return RedirectToPage("/NotFound");

            CustomerId = customerId;
            ExcursionId = excursionId;

            Customer = await _customersQueryComponent.CustomerAsync(CustomerId);

            ExcursionDetail = await _productQueryComponent.ExcursionItemAsync(ExcursionId);

            if(Customer == null || ExcursionDetail == null)
                return RedirectToPage("/NotFound");

            return Page();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (CustomerId == Guid.Empty || ExcursionId == Guid.Empty)
                return RedirectToPage("/NotFound");

            Customer = await _customersQueryComponent.CustomerAsync(CustomerId);

            ExcursionDetail = await _productQueryComponent.ExcursionItemAsync(ExcursionId);

            if (!ModelState.IsValid)
                return Page();

            if (Customer == null || ExcursionDetail == null)
                return RedirectToPage("/NotFound");

            return Page();
        }

    }
}