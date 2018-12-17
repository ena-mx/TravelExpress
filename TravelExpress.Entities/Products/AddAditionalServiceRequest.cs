namespace TravelExpress.Entities.Products
{
    using System.ComponentModel.DataAnnotations;

    public class AddAditionalServiceRequest
    {
        [Display(Name = nameof(Resources.Resource.Name_Display), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductDescription_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "ProductDescription_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }

        [Display(Name = nameof(Resources.Resource.UnitPrice_Display), ResourceType = typeof(Resources.Resource))]
        [Range(0, double.MaxValue)]
        [Required]
        public decimal UnitPrice { get; set; }
    }

    public class AddPriceDetailRequest
    {

        public int PriceCategoryId { get; set; }

        [Display(Name = nameof(Resources.Resource.Name_Display), ResourceType = typeof(Resources.Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductDescription_Required", ErrorMessageResourceType = typeof(Resources.Resource))]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "ProductDescription_StringLength_Error", ErrorMessageResourceType = typeof(Resources.Resource))]
        public string Description { get; set; }

        [Display(Name = nameof(Resources.Resource.UnitPrice_Display), ResourceType = typeof(Resources.Resource))]
        [Range(0, double.MaxValue)]
        [Required]
        public decimal UnitPrice { get; set; }
    }
}
