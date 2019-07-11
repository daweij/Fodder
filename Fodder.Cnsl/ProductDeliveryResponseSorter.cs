using System;
using System.Collections.Generic;
using System.Text;

namespace Fodder.Cnsl
{
    public class ProductDeliveryResponseSorter : IComparer<ProductDeliveryResponse>
    {
        private readonly DateTime date;
        private readonly int greenDayLimit;

        public ProductDeliveryResponseSorter(DateTime? date = null, int greenDayLimit = 3)
        {
            this.date = date?.Date ?? DateTime.Now.Date;
            this.greenDayLimit = greenDayLimit;
        }

        public int Compare(ProductDeliveryResponse x, ProductDeliveryResponse y)
        {
            // both prioritized, object check
            if (IsPrioritized(x) && IsPrioritized(y))
                return x.CompareTo(y);
            else if (IsPrioritized(x))
                return -1;
            else if (IsPrioritized(y))
                return 1;

            // none prioritized, regular check
            return x.CompareTo(y);
        }

        public bool IsPrioritized(ProductDeliveryResponse response)
        {
            return response.IsGreenDelivery && IsWithinLimit(response.DeliveryDate);
        }

        public bool IsWithinLimit(DateTime date)
        {
            return (date - this.date).Days <= greenDayLimit;
        }
    }
}
