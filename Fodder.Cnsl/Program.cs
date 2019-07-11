using System;
using System.Collections.Generic;
using System.Linq;

namespace Fodder.Cnsl
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new DeliveryServiceSettings();
            settings.IsEnvironmentFriendly = (date) =>
            {
                // wednesdays are good
                if ((int)date.DayOfWeek == 3)
                {
                    return true;
                }
                return false;
            };
            var service = new DeliveryService(settings);



            var products1 = new Product[]
            {
                new Product { OrderDaysInAdvance = 10 },
                new Product { Type = ProductType.External }
            };

            var products2 = new Product[]
            {
                new Product { Type = ProductType.Temporary }, 
                new Product
                {
                    AvailableDeliveryDays = new List<DayOfWeek>
                    {
                        DayOfWeek.Tuesday, 
                        DayOfWeek.Wednesday, 
                        DayOfWeek.Friday, 
                        DayOfWeek.Sunday
                    }
                }
            };




            var response1 = service.CalculateDelivery(products1, 12677);
            var response2 = service.CalculateDelivery(products2, 12677, new DateTime(2019, 7, 8));

            

            Console.WriteLine("Hello World!");
        }
    }
}
