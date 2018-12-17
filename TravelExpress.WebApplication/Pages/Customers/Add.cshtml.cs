namespace TravelExpress.WebApplication.Pages.Customers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Customers.Model;
    using TravelExpress.Entities.Customers;

    [BindProperties]
    public class AddModel : BasePageModel
    {
        public AddUpdateCustomerRequest Customer { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync([FromServices]CustomerComponentUowFactory customerComponentUowFactory)
        {
            if (customerComponentUowFactory == null)
            {
                throw new System.ArgumentNullException(nameof(customerComponentUowFactory));
            }

            if (!ModelState.IsValid)
                return Page();

            using (ICustomerComponentUow customerComponent = customerComponentUowFactory.NewCustomerUow())
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
                    customerComponent.RollbackChanges();
                    ModelState.AddModelError("", ex.Message);
                    return Page();
                }
            }
        }
    }
}