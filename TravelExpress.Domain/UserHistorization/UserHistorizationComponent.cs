namespace TravelExpress.Domain.UserHistorization
{
    using System;
    using System.Threading.Tasks;
    using TravelExpress.Enums.UserHistorization;

    public abstract class UserHistorizationComponent
    {
        public abstract Task AddUserActivity(Guid Id, UserActivityType userActivityType, DateTime date, Guid userId);
    }
}