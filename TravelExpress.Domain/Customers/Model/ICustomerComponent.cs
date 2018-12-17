namespace TravelExpress.Domain.Customers.Model
{
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;

    public interface ICustomerComponent : IDisposable
    {
        Guid CustomerId { get; }

        Task<WorkflowResult> SaveInfoAsync(
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
            );

        Task<WorkflowResult> AddObservationAsync(
                string description,
                Guid userId
            );

        Task<WorkflowResult> AddPhoneCallAsync(
                string description,
                Guid userId
            );
    }
}
