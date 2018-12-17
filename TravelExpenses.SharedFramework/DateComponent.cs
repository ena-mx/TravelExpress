namespace TravelExpenses.SharedFramework
{
    using System;

    public sealed class CstMexDateComponent : IDateComponent
    {
        private static readonly TimeZoneInfo _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
        public DateTime ServerDate => TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, _timeZoneInfo);
        public DateTime ConvertToServerTimeZone(DateTime date) => TimeZoneInfo.ConvertTime(date.ToUniversalTime(), TimeZoneInfo.Utc, _timeZoneInfo);
    }
}
