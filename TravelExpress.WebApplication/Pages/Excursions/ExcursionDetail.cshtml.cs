namespace TravelExpress.WebApplication.Pages.Excursions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using TravelExpress.Queries.Excursions;

    public class ExcursionDetailModel : BasePageModel
    {
        private readonly ExcursionQueryComponent _excursionQueryComponent;

        public Guid Id { get; set; }
        public ExcursionDataModel ExcursionModel { get; set; }

        public ExcursionDetailModel(ExcursionQueryComponent excursionQueryComponent)
        {
            _excursionQueryComponent = excursionQueryComponent ?? throw new ArgumentNullException(nameof(excursionQueryComponent));
        }

        public async Task<ActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToPage("/NotFound");
            Id = id;

            ExcursionModel = await _excursionQueryComponent.FindAsync(Id);

            if (ExcursionModel == null)
                return RedirectToPage("/NotFound");

            return Page();
        }
    }
}