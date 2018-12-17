namespace TravelExpress.Domain.Customers.Model
{
    using System;
    using System.Threading.Tasks;
    using System.Transactions;
    using TravelExpenses.SharedFramework;

    public sealed class CustomerComponentUow : ICustomerComponentUow
    {
        #region Private members

        private ICustomerComponent _decorated;
        private TransactionScope _transactionScope;
        private bool _changesCommited = false;

        #endregion Private members

        #region Constructor

        public CustomerComponentUow(ICustomerComponent decorated)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        #endregion Constructor

        #region Product Component Protocol

        public Guid CustomerId => _decorated.CustomerId;

        public async Task<WorkflowResult> AddObservationAsync(string description, Guid userId)
         => await _decorated.AddObservationAsync(description, userId);

        public async Task<WorkflowResult> AddPhoneCallAsync(string description, Guid userId)
        => await _decorated.AddPhoneCallAsync(description, userId);

        public async Task<WorkflowResult> SaveInfoAsync(string name, string familyName1, string familyName2, string telephone, string cellphone, string email, DateTime birthDate, bool phoneCallsEnabled, bool mailEnabled, Guid userId)
        => await _decorated.SaveInfoAsync(name, familyName1, familyName2, telephone, cellphone, email, birthDate, phoneCallsEnabled, mailEnabled, userId);

        #endregion Product Component Protocol

        #region Uow Protocol

        public void CommitChanges()
        {
            try
            {
                if (_transactionScope != null)
                {
                    _transactionScope.Complete();
                }
            }
            catch (Exception)
            {
                if (Transaction.Current != null)
                {
                    try
                    {
                        Transaction.Current.Rollback();
                    }
                    finally { }
                }
            }
            finally
            {
                _changesCommited = true;
            }
        }

        public void RollbackChanges()
        {
            if (_changesCommited)
                return;
            if (Transaction.Current != null)
            {
                try
                {
                    Transaction.Current.Rollback();
                }
                finally { }
            }
            _changesCommited = true;
        }

        #endregion Uow Protocol

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_decorated != null)
                    {
                        _decorated.Dispose();
                        _decorated = null;
                    }
                    if(_transactionScope != null)
                    {
                        if (!_changesCommited)
                            CommitChanges();
                        _transactionScope.Dispose();
                        _transactionScope = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~CustomerComponentUow()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}