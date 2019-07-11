using System;
using System.Collections.Generic;

namespace Fodder.Cnsl
{
    public interface IDeliveryService
    {
        IEnumerable<DateTime> PossibleDeliveryDates(Product product, DateTime? date = null);
        IEnumerable<DateTime> PossibleDeliveryDates(IEnumerable<Product> products, DateTime? date = null);

        string CalculateDeliveryJson(Product product, int postalCode, DateTime? date = null);
        IEnumerable<ProductDeliveryResponse> CalculateDelivery(Product product, int postalCode, DateTime? date = null);
        string CalculateDeliveryJson(IEnumerable<Product> products, int postalCode, DateTime? date = null);
        IEnumerable<ProductDeliveryResponse> CalculateDelivery(IEnumerable<Product> products, int postalCode, DateTime? date = null);
    }
}