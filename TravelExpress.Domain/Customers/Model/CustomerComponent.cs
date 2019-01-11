namespace TravelExpress.Domain.Customers.Model
{
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Customers.Shared;
    using TravelExpress.Domain.UserHistorization;

    public sealed class CustomerComponent : ICustomerComponent
    {
        #region Private Members

        private readonly Guid _customerId;

        #endregion Private Members

        #region Internal Properties

        internal CustomerState State { get; set; }

        #endregion Internal Properties

        #region Constructor

        public CustomerComponent(
            CustomerStorageComponent customerStorage,
            CustomerHistorization customerHistorization,
            IDateComponent dateComponent,
            UserHistorizationComponent userHistorizationComponent)
        {
            if (customerStorage == null)
            {
                throw new ArgumentNullException(nameof(customerStorage));
            }

            if (customerHistorization == null)
            {
                throw new ArgumentNullException(nameof(customerHistorization));
            }

            if (dateComponent == null)
            {
                throw new ArgumentNullException(nameof(dateComponent));
            }

            if (userHistorizationComponent == null)
            {
                throw new ArgumentNullException(nameof(userHistorizationComponent));
            }

            _customerId = Guid.NewGuid();
            State = new NewCustomerState(this, customerStorage, customerHistorization, dateComponent, userHistorizationComponent);
        }

        public CustomerComponent(
            Guid customerId,
            CustomerStorageComponent customerStorage,
            CustomerHistorization customerHistorization,
            IDateComponent dateComponent,
            UserHistorizationComponent userHistorizationComponent)
        {
            if (customerStorage == null)
            {
                throw new ArgumentNullException(nameof(customerStorage));
            }

            if (customerHistorization == null)
            {
                throw new ArgumentNullException(nameof(customerHistorization));
            }

            if (dateComponent == null)
            {
                throw new ArgumentNullException(nameof(dateComponent));
            }

            if (userHistorizationComponent == null)
            {
                throw new ArgumentNullException(nameof(userHistorizationComponent));
            }

            _customerId = customerId;
            State = new ExistingCustomerState(this, customerStorage, customerHistorization, dateComponent, userHistorizationComponent);
        }

        #endregion Constructor

        #region Product Component Protocol

        public Guid CustomerId => _customerId;

        public async Task<WorkflowResult> AddObservationAsync(string description, Guid userId)
        => await State.AddObservationAsync(description, userId);

        public async Task<WorkflowResult> AddPhoneCallAsync(string description, Guid userId)
        => await State.AddPhoneCallAsync(description, userId);

        public async Task<WorkflowResult> SaveInfoAsync(string name, string familyName1, string familyName2, string telephone, string cellphone, string email, DateTime birthDate, bool phoneCallsEnabled, bool mailEnabled, Guid userId)
        => await State.SaveInfoAsync(name, familyName1, familyName2, telephone, cellphone, email, birthDate, phoneCallsEnabled, mailEnabled, userId);

        #endregion Product Component Protocol

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        private void InternalDispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                State = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~CustomerComponent()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            InternalDispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            InternalDispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
