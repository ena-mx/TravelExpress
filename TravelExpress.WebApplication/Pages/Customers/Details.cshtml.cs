namespace TravelExpress.WebApplication.Pages.Customers
{
    using EnaBricks.Generics;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Customers.Model;
    using TravelExpress.Entities.Customers;
    using TravelExpress.Queries.Customers;

    public class DetailsModel : BasePageModel
    {
        private readonly CustomersQueryComponent _customersQueryComponent;
        public Customer Customer { get; set; }

        [BindProperty]
        public Guid Id { get; set; }

        [BindProperty]
        public AddPhoneCallRequest AddPhoneCall { get; set; }

        [BindProperty]
        public AddObservationRequest AddObservation { get; set; }

        public DetailsModel(CustomersQueryComponent customersQueryComponent)
        {
            _customersQueryComponent = customersQueryComponent;
        }

        public async Task<ActionResult> OnGetAsync(Guid id)
        {
            this.Id = id;
            Customer = await _customersQueryComponent.CustomerAsync(Id);
            if (Customer == null)
                return RedirectToPage("/NotFound");
            return Page();
        }

        public async Task<IActionResult> OnPostAddPhoneCallAsync([FromServices]CustomerComponentUowFactory customerComponentUowFactory)
        {
            if (customerComponentUowFactory == null)
            {
                throw new ArgumentNullException(nameof(customerComponentUowFactory));
            }

            ModelState.Clear();
            if (!TryValidateModel(AddPhoneCall))
            {
                ModelState.TryAddModelError("", "Parametros incorrectos");
                return Page();
            }

            Option<ICustomerComponentUow> customerComponentOption = await customerComponentUowFactory.ExistingCustomerUowAsync(Id);

            if (!customerComponentOption.Any())
            {
                ModelState.TryAddModelError("", "El registro ya no se encuentra en BD");
                return Page();
            }

            using (ICustomerComponentUow customerComponent = customerComponentOption.Single())
            {
                try
                {
                    WorkflowResult result = await customerComponent.AddPhoneCallAsync(AddPhoneCall.Description, UserId());

                    if (!result.Success)
                    {
                        customerComponent.RollbackChanges();

                        for (int i = 0; i < result.Errors.Length; i++)
                        {
                            ModelState.TryAddModelError("", result.Errors[i]);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    customerComponent.RollbackChanges();
                    ModelState.AddModelError("", ex.Message);
                }
            }
            Customer = await _customersQueryComponent.CustomerAsync(Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAddObservationCallAsync([FromServices]CustomerComponentUowFactory customerComponentUowFactory)
        {
            if (customerComponentUowFactory == null)
            {
                throw new ArgumentNullException(nameof(customerComponentUowFactory));
            }

            ModelState.Clear();
            if (!TryValidateModel(AddObservation))
            {
                ModelState.TryAddModelError("", "Parametros incorrectos");
                return Page();
            }

            Option<ICustomerComponentUow> customerComponentOption = await customerComponentUowFactory.ExistingCustomerUowAsync(Id);

            if (!customerComponentOption.Any())
            {
                ModelState.TryAddModelError("", "El registro ya no se encuentra en BD");
                return Page();
            }

            using (ICustomerComponentUow customerComponent = customerComponentOption.Single())
            {
                try
                {
                    WorkflowResult result = await customerComponent.AddObservationAsync(AddObservation.Description, UserId());

                    if (!result.Success)
                    {
                        customerComponent.RollbackChanges();

                        for (int i = 0; i < result.Errors.Length; i++)
                        {
                            ModelState.TryAddModelError("", result.Errors[i]);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    customerComponent.RollbackChanges();
                    ModelState.AddModelError("", ex.Message);
                }
            }
            Customer = await _customersQueryComponent.CustomerAsync(Id);
            return Page();
        }
    }
}