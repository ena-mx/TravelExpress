namespace TravelExpress.Entities.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FillOrderRequest
    {
        private static readonly List<AditionalServicesRequest> _emptyAditionalServices = new List<AditionalServicesRequest>(0);
        private static readonly List<PriceDetailRequest> _emptyPriceDetails = new List<PriceDetailRequest>(0);

        public List<AditionalServicesRequest> AditionalServices { get; set; }
        public List<PriceDetailRequest> PriceDetails { get; set; }

        public KeyValuePair<int, int>[] ValidAditionalServices => (AditionalServices ?? _emptyAditionalServices)
                        .Where(item => item.AditionalServiceId > 0 && item.Quantity > 0)
                        .Select(item => new KeyValuePair<int, int>(item.AditionalServiceId, item.Quantity))
                        .ToArray() ?? Array.Empty<KeyValuePair<int, int>>();

        public KeyValuePair<int, int>[] ValidPriceDetails => (PriceDetails ?? _emptyPriceDetails)
                        .Where(item => item.PriceDetailId > 0 && item.Quantity > 0)
                        .Select(item => new KeyValuePair<int, int>(item.PriceDetailId, item.Quantity))
                        .ToArray() ?? Array.Empty<KeyValuePair<int, int>>();
    }
}
