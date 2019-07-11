using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fodder.Cnsl
{
    public class DeliveryService : IDeliveryService
    {
        private readonly DeliveryServiceSettings settings;
        private ProductDeliveryResponseSorter sorter;

        public DeliveryService()
        {
            this.settings = new DeliveryServiceSettings();
            this.sorter = new ProductDeliveryResponseSorter();
        }

        public DeliveryService(DeliveryServiceSettings settings) : this()
        {
            this.settings = settings ?? throw new ArgumentNullException("Settings cannot be null.");
        }

        public string CalculateDeliveryJson(Product product, int postalCode, DateTime? date = null)
        {
            return "[" + string.Join(",", CalculateDelivery(product, postalCode, date).Select(r => r.AsJson())) + "]";
        }

        public IEnumerable<ProductDeliveryResponse> CalculateDelivery(Product product, int postalCode, DateTime? date = null)
        {
            var deliveryDays = PossibleDeliveryDates(product, date);
            var response = new List<ProductDeliveryResponse>();
            foreach (var day in deliveryDays)
            {
                response.Add(new ProductDeliveryResponse
                {
                    PostalCode = postalCode, 
                    DeliveryDate = day, 
                    IsGreenDelivery = settings.IsEnvironmentFriendly(day)
                });
            }

            response.Sort(this.sorter);
            return response;
        }

        public string CalculateDeliveryJson(IEnumerable<Product> products, int postalCode, DateTime? date = null)
        {
            return "[" + string.Join(",", CalculateDelivery(products, postalCode, date).Select(r => r.AsJson())) + "]";
        }


        public IEnumerable<ProductDeliveryResponse> CalculateDelivery(IEnumerable<Product> products, int postalCode, DateTime? date = null)
        {
            var deliveryDays = PossibleDeliveryDates(products, date);
            var response = new List<ProductDeliveryResponse>();
            foreach (var day in deliveryDays)
            {
                response.Add(new ProductDeliveryResponse
                {
                    PostalCode = postalCode,
                    DeliveryDate = day,
                    IsGreenDelivery = settings.IsEnvironmentFriendly(day)
                });
            }

            response.Sort(this.sorter);
            return response;
        }



        // - a delivery date is not valid if a product must be ordered more days in advance than possible
        // dw: (...)

        // - all external products need to be ordered 5 days in in advance
        // dw: i.e., can't be deliveded until after 5 days have passed...
        //     so, check dates from now + (5 || product.DeliveryDaysInAdvance if DelieryDaysInAdvance > 5)
        //     until settings.FindNumberOfDays.


        // - temporary products can only be ordered within the current week(Mon-Sun)
        // dw: (makes no sense, is this a req. that they must be delivered within that week?)

        public IEnumerable<DateTime> PossibleDeliveryDates(Product product, DateTime? date = null)
        {
            var orderDay = date ?? DateTime.Now;
            var firstDeliveryDay = orderDay.AddDays(1);
            var lastDeliveryDay = orderDay.AddDays(settings.FindNumberOfDays);

            // temporary items, calculate new stop date 
            // = upcoming sunday
            if (product.Type == ProductType.Temporary)
            {
                // cant deliver on next day...
                if (orderDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    yield break;
                }

                if (firstDeliveryDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    lastDeliveryDay = firstDeliveryDay;
                }
                else
                {
                    lastDeliveryDay = firstDeliveryDay.AddDays(7 - (int)firstDeliveryDay.DayOfWeek);
                }
            }

            // shift the start based on setting method to calculate 
            // how many days in advance the item must be ordered
            if (settings.DeliveryDayShift(product) > 0)
            {
                // withdraw one, as we're not delivering the orderday
                firstDeliveryDay = firstDeliveryDay.AddDays(settings.DeliveryDayShift(product) - 1);
            }

            var days = GetDays(firstDeliveryDay, lastDeliveryDay);
            foreach (var day in days)
            {
                if (product.AvailableDeliveryDays.Contains(day.DayOfWeek))
                {
                    yield return day;
                }
            }
        }

        public IEnumerable<DateTime> PossibleDeliveryDates(IEnumerable<Product> products, DateTime? date = null)
        {
            IEnumerable<DateTime> possibleDeliveryDates = new List<DateTime>();

            foreach (var product in products)
            {
                var productDeliveryDates = PossibleDeliveryDates(product, date);

                if (possibleDeliveryDates.Any())
                {
                    possibleDeliveryDates = possibleDeliveryDates.Intersect(productDeliveryDates);
                }
                else
                {
                    possibleDeliveryDates = productDeliveryDates;
                }
            }
            return possibleDeliveryDates;
        }


        public IEnumerable<DateTime> GetDays(DateTime from, DateTime until)
        {
            if (from > until) return null;

            var days = new List<DateTime>();
            for(var day = from.Date; day.Date <= until.Date; day = day.AddDays(1))
            {
                days.Add(day);
            }

            return days;
        }
    }
}
