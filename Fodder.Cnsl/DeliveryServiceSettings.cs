using System;

namespace Fodder.Cnsl
{
    public class DeliveryServiceSettings
    {
        // default method to calculate green delivery 
        // with possibility to replace
        private Func<DateTime, bool> isEnvironmentFriendly = (date) => false; // default, trucks are the devil.
        public Func<DateTime, bool> IsEnvironmentFriendly
        {
            get
            {
                return isEnvironmentFriendly;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("A method to identify green cargo is needed.");
                }
                isEnvironmentFriendly = value;
            }
        }

        private int findNumberOfDays = 14;
        public int FindNumberOfDays
        {
            get
            {
                return findNumberOfDays;
            }
            set
            {
                if (value < 1 || value > 31)
                {
                    throw new ArgumentOutOfRangeException("Must be given a value between 1 and 31.");
                }
                findNumberOfDays = value;
            }
        }

        // default method to calculate delivery start
        // with possibility to replace
        private Func<Product, int> deliveryDayShift = (product) => (product.Type == ProductType.External && product.OrderDaysInAdvance < 5) ? 5 : product.OrderDaysInAdvance;
        public Func<Product, int> DeliveryDayShift
        {
            get
            {
                return deliveryDayShift;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("A method to identify shift of deliverydays can't be null.");
                }
                deliveryDayShift = value;
            }
        }
    }
}