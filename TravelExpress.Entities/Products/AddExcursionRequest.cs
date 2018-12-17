namespace TravelExpress.Entities.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddExcursionRequest
    {
        [Display(Name = nameof(Resources.Resource.BeginDate_Display), ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "BeginDate_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        public DateTime BeginDate { get; set; }

        [Display(Name = nameof(Resources.Resource.EndDate_Display), ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessageResourceName = "EndDate_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        public DateTime EndDate { get; set; }
    }
}
