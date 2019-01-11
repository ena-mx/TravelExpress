namespace TravelExpress.WebApplication.Pages.Customers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Orders.Component;
    using TravelExpress.Entities.Orders;
    using TravelExpress.Queries.Customers;
    using TravelExpress.Queries.Products;

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

        private async Task<ActionResult> LoadViewModelAsync(RedirectToPageResult redirectToPageResult = null)
        {
            if (CustomerId == Guid.Empty || ExcursionId == Guid.Empty)
                return RedirectToPage("/NotFound");

            Customer = await _customersQueryComponent.CustomerAsync(CustomerId);

            ExcursionDetail = await _productQueryComponent.ExcursionItemAsync(ExcursionId);

            if (Customer == null || ExcursionDetail == null)
                return RedirectToPage("/NotFound");

            if (redirectToPageResult != null)
                return redirectToPageResult;

            return Page();
        }

        public async Task<ActionResult> OnGetAsync(Guid customerId, Guid excursionId)
        {
            CustomerId = customerId;
            ExcursionId = excursionId;

            return await LoadViewModelAsync();
        }

        public async Task<ActionResult> OnPostAsync([FromServices]OrderFactory orderFactory)
        {
            if (orderFactory == null)
            {
                throw new ArgumentNullException(nameof(orderFactory));
            }

            ModelState.Clear();

            if (FillOrderRequest == null)
            {
                ModelState.TryAddModelError("", Resources.Resource.FillOrderMissingDetailsError);
                return await LoadViewModelAsync();
            }

            IOrder order = orderFactory.NewOrder(CustomerId);

            WorkflowResult result = await order.CreateAsync(
                    ExcursionId,
                    FillOrderRequest.ValidAditionalServices,
                    FillOrderRequest.ValidPriceDetails,
                    UserId()
                );

            if (!result.Success)
            {
                for (int i = 0; i < result.Errors.Length; i++)
                {
                    ModelState.TryAddModelError("", result.Errors[i]);
                }

                return await LoadViewModelAsync();
            }

            return await LoadViewModelAsync(RedirectToPage("/Orders/OrderDetail", new { id = order.OrderId }));
        }
    }
}