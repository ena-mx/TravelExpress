namespace TravelExpress.Entities.Products
{
    using System.ComponentModel.DataAnnotations;

    public class AddUpdatePriceCategoryRequest
    {
        [Display(Name = nameof(Resources.Resource.AddUpdatePriceCategoryRequest_Description), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AddUpdatePriceCategoryRequest_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "AddUpdatePriceCategoryRequest_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }
    }
}
