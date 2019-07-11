using System;
using System.Collections.Generic;
using System.Text;

namespace Fodder.Cnsl
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductType Type { get; set; }

        public IEnumerable<DayOfWeek> AvailableDeliveryDays { get; set; }

        private int orderDaysInAdvance;
        public int OrderDaysInAdvance
        {
            get
            {
                return orderDaysInAdvance;
            }
            set
            {
                if (value < 0 || value > 31)
                {
                    throw new ArgumentOutOfRangeException("Must be given a value between 0 and 31.");
                }
                orderDaysInAdvance = value;
            }
        }

        public Product()
        {
            Type = ProductType.Normal;
            AvailableDeliveryDays = new List<DayOfWeek>()
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };
        }
    }
}
