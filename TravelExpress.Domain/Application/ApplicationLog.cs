namespace TravelExpress.Domain.Application
{
    using System;
    using System.Threading.Tasks;

    public abstract class ApplicationLog
    {
        public abstract Task<int> LogExceptionAsync(Exception exception);
    }
}
