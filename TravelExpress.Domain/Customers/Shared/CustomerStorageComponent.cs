namespace TravelExpress.Domain.Customers.Shared
{
    using System;
    using System.Threading.Tasks;

    public abstract class CustomerStorageComponent
    {
        public abstract Task<bool> TryAddAsync(
            Guid customerId,
            string name,
            string familyName1,
            string familyName2,
            string telephone,
            string cellphone,
            string email,
            DateTime birthDate,
            DateTime lastUpdatedDate,
            bool phoneCallsEnabled,
            bool mailEnabled);

        public abstract Task<bool> TryUpdateAsync(
            Guid customerId,
            string name,
            string familyName1,
            string familyName2,
            string telephone,
            string cellphone,
            string email,
            DateTime birthDate,
            DateTime lastUpdatedDate,
            bool phoneCallsEnabled,
            bool mailEnabled);

        public abstract Task<bool> FindDuplicateEmailAsync(Guid customerId, string email);

        public abstract Task<bool> TryAddObservationAsync(Guid customerId, string description, Guid userId, DateTime date);

        public abstract Task<bool> TryAddPhoneCallAsync(Guid customerId, string description, Guid userId, DateTime date);
    }
}
