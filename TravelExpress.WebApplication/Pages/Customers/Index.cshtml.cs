namespace TravelExpress.WebApplication.Pages.Customers
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Common;
    using TravelExpress.Queries.Customers;

    public class IndexModel : BasePageModel
    {
        private readonly CustomersQueryComponent _customersQueryComponent;
        public GenericPage<CustomerIndex> CustomersPage { get; set; }
        public List<SelectListItem> PageLimitItems { get; set; }
        public int Limit { get; set; }
        public int PageIndex { get; set; }
        public int Offset { get; set; }
        public string SearchValue { get; set; } = string.Empty;
        public int PageLimit { get; set; }

        public IndexModel(CustomersQueryComponent customersQueryComponent)
        {
            _customersQueryComponent = customersQueryComponent ?? throw new ArgumentNullException(nameof(customersQueryComponent));
        }

        public async Task OnGetAsync(int pageIndex = 1, int pageLimit = 10, string search = "")
        {
            if(!string.IsNullOrWhiteSpace(search))
                SearchValue = search;
            Limit = pageLimit;
            PageIndex = pageIndex;
            Offset = (PageIndex - 1) * Limit;
            BindPageLimitSelectItems();
            CustomersPage = await _customersQueryComponent.CustomersAsync(Offset, Limit, SearchValue);
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