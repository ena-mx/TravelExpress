namespace TravelExpress.Entities.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class AddOrderPaymentRequest
    {
        [Display(Name = nameof(Resources.Resource.UnitPrice_Display), ResourceType = typeof(Resources.Resource))]
        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}