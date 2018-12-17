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

    public class EditModel : BasePageModel
    {
        [BindProperty]
        public AddUpdateCustomerRequest Customer { get; set; }

        [BindProperty]
        public Guid Id { get; set; }

        public async Task<ActionResult> OnGetAsync([FromServices]CustomersQueryComponent customersQueryComponent, Guid id)
        {
            if (customersQueryComponent == null)
            {
                throw new ArgumentNullException(nameof(customersQueryComponent));
            }

            this.Id = id;

            Customer customer = await customersQueryComponent.CustomerAsync(Id);

            if (customer == null)
                return RedirectToPage("/NotFound");

            Customer = new AddUpdateCustomerRequest
            {
                Name = customer.CustomerInfo.Name,
                FamilyName1 = customer.CustomerInfo.FamilyName1,
                FamilyName2 = customer.CustomerInfo.FamilyName2,
                BirthDate = customer.CustomerInfo.BirthDate,
                Cellphone = customer.CustomerInfo.Cellphone,
                Email = customer.CustomerInfo.Email,
                Telephone = customer.CustomerInfo.Telephone,
                MailEnabled = customer.CustomerOptions.MailEnabled,
                PhoneCallsEnabled = customer.CustomerOptions.PhoneCallsEnabled
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices]CustomerComponentUowFactory customerComponentUowFactory,
            [FromServices]CustomersQueryComponent customersQueryComponent)
        {
            if (customerComponentUowFactory == null)
            {
                throw new System.ArgumentNullException(nameof(customerComponentUowFactory));
            }

            if (customersQueryComponent == null)
            {
                throw new ArgumentNullException(nameof(customersQueryComponent));
            }

            if (!ModelState.IsValid)
                return Page();

            Option<ICustomerComponentUow> customerComponentOption = await customerComponentUowFactory.ExistingCustomerUowAsync(Id);

            if (!customerComponentOption.Any())
                return RedirectToPage("/NotFound");

            using (ICustomerComponentUow customerComponent = customerComponentOption.Single())
            {
                try
                {
                    WorkflowResult result = await customerComponent.SaveInfoAsync(Customer.Name,
                                                                Customer.FamilyName1,
                                                                Customer.FamilyName2,
                                                                Customer.Telephone,
                                                                Customer.Cellphone,
                                                                Customer.Email,
                                                                Customer.BirthDate,
                                                                Customer.PhoneCallsEnabled,
                                                                Customer.MailEnabled,
                                                                UserId());

                    if (!result.Success)
                    {
                        customerComponent.RollbackChanges();

                        for (int i = 0; i < result.Errors.Length; i++)
                        {
                            ModelState.TryAddModelError("", result.Errors[i]);
                        }

                        return Page();
                    }

                    return RedirectToPage("./Details", new { id = customerComponent.CustomerId });
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    customerComponent.RollbackChanges();
                }
            }

            return Page();


        }
    }
}