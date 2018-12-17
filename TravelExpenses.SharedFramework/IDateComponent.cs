using System;

namespace TravelExpenses.SharedFramework
{
    public interface IDateComponent
    {
        DateTime ServerDate { get; }

        DateTime ConvertToServerTimeZone(DateTime date);
    }
}