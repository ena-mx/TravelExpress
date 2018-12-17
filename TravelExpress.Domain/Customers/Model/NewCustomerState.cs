namespace TravelExpress.Domain.Customers.Model
{
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Customers.Shared;
    using TravelExpress.Enums.Customers;

    internal sealed class NewCustomerState : CustomerState
    {
        #region Constructor

        public NewCustomerState(CustomerComponent customerComponent, CustomerStorageComponent customerStorage, CustomerHistorization customerHistorization, IDateComponent dateComponent) : base(customerComponent, customerStorage, customerHistorization, dateComponent)
        {
        }

        #endregion Constructor

        #region Product State Protocol

        public override async Task<WorkflowResult> SaveInfoAsync(string name, string familyName1, string familyName2, string telephone, string cellphone, string email, DateTime birthDate, bool phoneCallsEnabled, bool mailEnabled, Guid userId)
        {
            if (await _customerStorage.FindDuplicateEmailAsync(_customerComponent.CustomerId, email))
                return _errorDuplicateEmail;

            if (!await _customerStorage.TryAddAsync(_customerComponent.CustomerId, name, familyName1, familyName2, telephone,
                    cellphone, email, birthDate, _dateComponent.ServerDate, phoneCallsEnabled, mailEnabled))
                return _errorSavingCustomerInStorage;

            await _customerHistorization.AddCustomerActivity(_customerComponent.CustomerId, CustomerActivityType.CustomerCreated, _dateComponent.ServerDate, userId);

            _customerComponent.State = new ExistingCustomerState(_customerComponent, _customerStorage, _customerHistorization, _dateComponent);

            return _successWorkflowResult;
        }

        #endregion Product State Protocol
    }
}
