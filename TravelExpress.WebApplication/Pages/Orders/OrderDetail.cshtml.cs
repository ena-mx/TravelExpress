namespace TravelExpress.WebApplication.Pages.Orders
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Orders.Component;
    using TravelExpress.Entities.Orders;
    using TravelExpress.Queries.Orders;

    public class OrderDetailModel : BasePageModel
    {
        private readonly OrderModelQueryComponent _orderModelQueryComponent;

        [BindProperty]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public OrderModel OrderModel { get; set; }

        [BindProperty]
        public AddOrderPaymentRequest AddPaymentRequest { get; set; }

        public OrderDetailModel(
            OrderModelQueryComponent orderModelQueryComponent)
        {
            _orderModelQueryComponent = orderModelQueryComponent ?? throw new ArgumentNullException(nameof(orderModelQueryComponent));
        }

        public async Task<ActionResult> OnGet(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToPage("/NotFound");

            Id = id;

            return await TryLoadViewModelAsync();
        }

        public async Task<ActionResult> TryLoadViewModelAsync()
        {
            OrderModel = await _orderModelQueryComponent.OrderAsync(Id);

            if (OrderModel == null)
                return RedirectToPage("/NotFound");

            CustomerId = OrderModel.CustomerInfo.CustomerId;

            return Page();
        }

        public async Task<ActionResult> OnPostAsync([FromServices]OrderFactory orderFactory)
        {
            if (orderFactory == null)
            {
                throw new ArgumentNullException(nameof(orderFactory));
            }

            ModelState.Clear();

            if (!TryValidateModel(AddPaymentRequest))
            {
                ModelState.TryAddModelError("", "Parametros incorrectos");
                return await TryLoadViewModelAsync();
            }

            EnaBricks.Generics.Option<IOrder> orderOption = await orderFactory.OrderAsync(Id);

            if (!orderOption.Any())
                return Redirect("/Error");

            IOrder order = orderOption.Single();

            WorkflowResult result = await order.AddPaymentAsync(AddPaymentRequest.Amount, UserId());

            if (!result.Success)
            {
                for (int i = 0; i < result.Errors.Length; i++)
                {
                    ModelState.TryAddModelError("", result.Errors[i]);
                }
            }

            return await TryLoadViewModelAsync();
        }

        public async Task<ActionResult> OnPostDeleteOrderAsync([FromServices]OrderFactory orderFactory)
        {
            if (orderFactory == null)
            {
                throw new ArgumentNullException(nameof(orderFactory));
            }

            ModelState.Clear();

            EnaBricks.Generics.Option<IOrder> orderOption = await orderFactory.OrderAsync(Id);

            if (!orderOption.Any())
                return Redirect("/Error");

            IOrder order = orderOption.Single();

            WorkflowResult result = await order.CancelAsync(UserId());

            if (!result.Success)
            {
                for (int i = 0; i < result.Errors.Length; i++)
                {
                    ModelState.TryAddModelError("", result.Errors[i]);
                }
            }

            return await TryLoadViewModelAsync();
        }
    }
}