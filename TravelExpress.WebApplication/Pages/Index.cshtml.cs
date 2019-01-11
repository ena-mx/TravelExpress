namespace TravelExpress.WebApplication.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Queries.Analytics;

    public class IndexModel : PageModel
    {
        private readonly AnalyticsQueryComponent _analyticsQueryComponent;
        private readonly IDateComponent _dateComponent;

        public AnalyticsModel AnalyticsModel { get; set; }

        public IndexModel(
            AnalyticsQueryComponent analyticsQueryComponent,
            IDateComponent dateComponent)
        {
            _analyticsQueryComponent = analyticsQueryComponent ?? throw new ArgumentNullException(nameof(analyticsQueryComponent));
            _dateComponent = dateComponent ?? throw new ArgumentNullException(nameof(dateComponent));
        }

        public async Task OnGetAsync()
        {
            DateTime dateActiveExcursions = _dateComponent.ServerDate.Date;
            DateTime userActivityDate = dateActiveExcursions.AddDays(-(dateActiveExcursions.Day - 1));
            AnalyticsModel = await _analyticsQueryComponent.AnalyticsModelAsync(userActivityDate, dateActiveExcursions);
        }
    }
}
