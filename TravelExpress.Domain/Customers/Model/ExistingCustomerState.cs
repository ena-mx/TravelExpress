namespace TravelExpress.Domain.Customers.Model
{
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Customers.Shared;
    using TravelExpress.Domain.UserHistorization;
    using TravelExpress.Enums.Customers;

    internal sealed class ExistingCustomerState : CustomerState
    {
        #region Constructor

        public ExistingCustomerState(CustomerComponent customerComponent, CustomerStorageComponent customerStorage, CustomerHistorization customerHistorization, IDateComponent dateComponent, UserHistorizationComponent userHistorizationComponent) : base(customerComponent, customerStorage, customerHistorization, dateComponent, userHistorizationComponent)
        {
        }

        #endregion Constructor

        #region Product State Protocol

        public override async Task<WorkflowResult> AddObservationAsync(string description, Guid userId)
        {
            await _customerStorage.TryAddObservationAsync(_customerComponent.CustomerId, description, userId, _dateComponent.ServerDate);

            await _customerHistorization.AddCustomerActivity(_customerComponent.CustomerId, CustomerActivityType.ObservationAdded, _dateComponent.ServerDate, userId);

            await _userHistorizationComponent.AddUserActivity(_customerComponent.CustomerId, Enums.UserHistorization.UserActivityType.CustomerObservationAdded, _dateComponent.ServerDate, userId);

            return _successWorkflowResult;
        }

        public override async Task<WorkflowResult> AddPhoneCallAsync(string description, Guid userId)
        {
            await _customerStorage.TryAddPhoneCallAsync(_customerComponent.CustomerId, description, userId, _dateComponent.ServerDate);

            await _customerHistorization.AddCustomerActivity(_customerComponent.CustomerId, CustomerActivityType.PhonecallAdded, _dateComponent.ServerDate, userId);

            await _userHistorizationComponent.AddUserActivity(_customerComponent.CustomerId, Enums.UserHistorization.UserActivityType.CustomerPhoneCallAdded, _dateComponent.ServerDate, userId);

            return _successWorkflowResult;
        }

        public override async Task<WorkflowResult> SaveInfoAsync(string name, string familyName1, string familyName2, string telephone, string cellphone, string email, DateTime birthDate, bool phoneCallsEnabled, bool mailEnabled, Guid userId)
        {
            if (await _customerStorage.FindDuplicateEmailAsync(_customerComponent.CustomerId, email))
                return _errorDuplicateEmail;

            if (!await _customerStorage.TryUpdateAsync(_customerComponent.CustomerId, name, familyName1, familyName2, telephone,
                    cellphone, email, birthDate, _dateComponent.ServerDate, phoneCallsEnabled, mailEnabled))
                return _errorSavingCustomerInStorage;

            await _customerHistorization.AddCustomerActivity(_customerComponent.CustomerId, CustomerActivityType.CustomerInfoUpdated, _dateComponent.ServerDate, userId);

            await _userHistorizationComponent.AddUserActivity(_customerComponent.CustomerId, Enums.UserHistorization.UserActivityType.CustomerInfoUpdated, _dateComponent.ServerDate, userId);

            return _successWorkflowResult;
        }

        #endregion Product State Protocol
    }
}
