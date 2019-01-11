namespace TravelExpress.Entities.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class AditionalServicesRequest
    {
        [Required]
        public int AditionalServiceId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
