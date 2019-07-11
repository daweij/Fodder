using System;
using System.Collections.Generic;
using System.Text;

namespace Fodder.Cnsl
{
    public class ProductDeliveryResponse : IComparable, IComparable<ProductDeliveryResponse>
    {
        public int PostalCode { get; set; }
        public bool IsGreenDelivery { get; set; }

        private DateTime deliveryDate;
        public DateTime DeliveryDate
        {
            get
            {
                return deliveryDate;
            }
            set
            {
                deliveryDate = value.Date;
            }
        }

        public int CompareTo(object obj)
        {
            if ((obj is ProductDeliveryResponse) == false)
            {
                throw new ArgumentException("App app, not a ProductDeliveryResponse.");
            }
            return CompareTo((ProductDeliveryResponse)obj);
        }

        public int CompareTo(ProductDeliveryResponse other)
        {
            if (DeliveryDate.CompareTo(other.deliveryDate) == 0)
            {
                // invert the sorting for boleean
                return IsGreenDelivery.CompareTo(other.IsGreenDelivery) * -1;
            }

            return DeliveryDate.CompareTo(other.DeliveryDate);
        }

        public string AsJson()
        {
            return $@"{{ ""postalCode"": {PostalCode}, ""deliveryDate"": ""{DeliveryDate.ToString("yyyy-MM-ddTHH:mm:sszzz")}"", ""isGreenDelivery"": ""{IsGreenDelivery.ToString().ToLower()}"" }}";
        }
    }
}
