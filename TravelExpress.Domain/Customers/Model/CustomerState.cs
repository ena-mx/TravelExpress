namespace TravelExpress.Domain.Customers.Model
{
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Customers.Shared;

    internal abstract class CustomerState
    {
        #region Protected Members

        protected readonly CustomerComponent _customerComponent;
        protected readonly CustomerStorageComponent _customerStorage;
        protected readonly CustomerHistorization _customerHistorization;
        protected readonly IDateComponent _dateComponent;
        protected static readonly WorkflowResult _invalidOperationError = new WorkflowResult(Resources.Resource.InvalidOperation);
        protected readonly WorkflowResult _successWorkflowResult = new WorkflowResult();
        protected readonly WorkflowResult _errorSavingCustomerInStorage = new WorkflowResult(Resources.Resource.NewCustomerComponent_ErrorSavingCustomerInStorage);
        protected readonly WorkflowResult _errorDuplicateEmail = new WorkflowResult(Resources.Resource.NewCustomerComponent_ErrorDuplicateEmail);

        #endregion Protected Members

        #region Constructor

        protected CustomerState(
            CustomerComponent customerComponent,
            CustomerStorageComponent customerStorage,
            CustomerHistorization customerHistorization,
            IDateComponent dateComponent)
        {
            _customerComponent = customerComponent ?? throw new ArgumentNullException(nameof(customerComponent));
            _customerStorage = customerStorage ?? throw new ArgumentNullException(nameof(customerStorage));
            _customerHistorization = customerHistorization ?? throw new ArgumentNullException(nameof(customerHistorization));
            _dateComponent = dateComponent ?? throw new ArgumentNullException(nameof(dateComponent));
        }

        #endregion Constructor

        #region Product State Protocol

        public virtual Task<WorkflowResult> SaveInfoAsync(
                string name,
                string familyName1,
                string familyName2,
                string telephone,
                string cellphone,
                string email,
                DateTime birthDate,
                bool phoneCallsEnabled,
                bool mailEnabled,
                Guid userId
            ) => Task.FromResult(_invalidOperationError);

        public virtual Task<WorkflowResult> AddObservationAsync(
                string description,
                Guid userId
            ) => Task.FromResult(_invalidOperationError);

        public virtual Task<WorkflowResult> AddPhoneCallAsync(
                string description,
                Guid userId
            ) => Task.FromResult(_invalidOperationError);

        #endregion Product State Protocol
    }
}
