namespace TravelExpress.WebApplication.Pages.Excursions
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Queries.Common;
    using TravelExpress.Queries.Excursions;

    public class IndexModel : BasePageModel
    {
        private readonly ExcursionQueryComponent _excursionQueryComponent;

        public GenericPage<ExcursionIndexModel> ExcursionsPage { get; set; }
        public List<SelectListItem> PageLimitItems { get; set; }
        public int Limit { get; set; }
        public int PageIndex { get; set; }
        public int Offset { get; set; }
        public string SearchValue { get; set; } = string.Empty;
        public int PageLimit { get; set; }
        public DateTime ServerDate { get; set; }

        public IndexModel(ExcursionQueryComponent excursionQueryComponent, IDateComponent dateComponent)
        {
            if (dateComponent == null)
            {
                throw new ArgumentNullException(nameof(dateComponent));
            }

            _excursionQueryComponent = excursionQueryComponent ?? throw new ArgumentNullException(nameof(excursionQueryComponent));
            ServerDate = dateComponent.ServerDate;
        }

        public async Task OnGetAsync(int pageIndex = 1, int pageLimit = 10, string search = "")
        {
            if (!string.IsNullOrWhiteSpace(search))
                SearchValue = search;
            Limit = pageLimit;
            PageIndex = pageIndex;
            Offset = (PageIndex - 1) * Limit;
            BindPageLimitSelectItems();
            ExcursionsPage = await _excursionQueryComponent.PageAsync(Offset, Limit, SearchValue);
        }

        private void BindPageLimitSelectItems()
        {
            PageLimitItems = new List<SelectListItem>(4) {
                new SelectListItem { Value = "10", Text = Resources.Resource.Grid_TenRows, Selected = (10 == Limit) },
                new SelectListItem { Value = "20", Text = Resources.Resource.Grid_TwentyRows, Selected = (20 == Limit) },
                new SelectListItem { Value = "30", Text = Resources.Resource.Grid_ThirtyRows, Selected = (30 == Limit) },
                new SelectListItem { Value = "50", Text = Resources.Resource.Grid_FiftyRows, Selected = (50 == Limit) }
            };
        }
    }
}