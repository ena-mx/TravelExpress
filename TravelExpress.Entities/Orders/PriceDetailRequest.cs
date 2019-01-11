namespace TravelExpress.Entities.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class PriceDetailRequest
    {
        [Required]
        public int PriceDetailId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}