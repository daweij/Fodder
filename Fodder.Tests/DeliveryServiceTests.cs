using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fodder.Cnsl;
using System.Collections.Generic;

namespace Fodder.Tests
{
    [TestClass]
    public class DeliveryServiceTests
    {
        [TestMethod]
        public void NormalProductReturnsEveryDayInPeriod()
        {
            var service = new DeliveryService();
            var product = new Product { Type = ProductType.Normal };
            var possibleDeliveryDates = service.PossibleDeliveryDates(product, new DateTime(2019, 1, 1)).ToList();
            Assert.IsTrue(possibleDeliveryDates.Count() == 14, "Should return all fourteen days.");
        }

        [TestMethod]
        public void NormalProductReturnsTwoMondays()
        {
            var service = new DeliveryService();
            var product = new Product
            {
                Type = ProductType.Normal,
                AvailableDeliveryDays = new List<DayOfWeek> { DayOfWeek.Monday }
            };
            var possibleDeliveryDates = service.PossibleDeliveryDates(product, new DateTime(2019, 1, 1)).ToList();
            Assert.IsTrue(possibleDeliveryDates.Count() == 2, "Should return two mondays.");
        }

        [TestMethod]
        public void OrderInAdvanceOver14ReturnsZeroDates()
        {
            var service = new DeliveryService();
            var product = new Product { OrderDaysInAdvance = 14 };
            var possibleDeliveryDates = service.PossibleDeliveryDates(product, new DateTime(2019, 1, 1)).ToList();
            Assert.IsTrue(possibleDeliveryDates.Count() == 1, "Returns one day, as we're counting orderday.");
        }

        [TestMethod]
        public void ExternalProductReturn10Days()
        {
            var service = new DeliveryService();
            var product = new Product { Type = ProductType.External };
            var possibleDeliveryDates = service.PossibleDeliveryDates(product, new DateTime(2019, 1, 1)).ToList();
            Assert.IsTrue(possibleDeliveryDates.Count() == 10, "Can be delivered 10 upcoming days.");
        }

        [TestMethod]
        public void TemporaryProductReturnsASingleDayOnSunday()
        {
            var service = new DeliveryService();
            var product = new Product { Type = ProductType.Temporary };
            var possibleDeliveryDates = service.PossibleDeliveryDates(product, new DateTime(2019, 1, 6)).ToList();
            Assert.IsTrue(possibleDeliveryDates.Count() == 0, "Temporary products ordered on a sunday cant be delivered...");
        }

        [TestMethod]
        public void MultipleNormalProductsReturnFourteenDays()
        {
            var service = new DeliveryService();
            var products = new Product[] 
            {
                new Product { Type = ProductType.Normal },
                new Product { Type = ProductType.Normal },
                new Product { Type = ProductType.Normal }
            };
            var possibleDeliveryDates = service.PossibleDeliveryDates(products, new DateTime(2019, 1, 1)).ToList();
            Assert.IsTrue(possibleDeliveryDates.Count() == 14, "Should return all fourteen days.");
        }

        [TestMethod]
        public void MultipleNormalProductsReturnsJustTheDaysTheTemporaryItemReturns()
        {
            var service = new DeliveryService();
            var products = new Product[]
            {
                new Product { Type = ProductType.Normal },
                new Product { Type = ProductType.Normal },
                new Product { Type = ProductType.Temporary }
            };
            var possibleDeliveryDates = service.PossibleDeliveryDates(products, new DateTime(2019, 1, 1)).ToList();
            Assert.IsTrue(possibleDeliveryDates.Count() == 5, "Should return the five days after the tuesday 1/1.");
        }

        [TestMethod]
        public void TemporaryAndExternalCanBeDelivered()
        {
            var service = new DeliveryService();
            var products = new Product[]
            {
                new Product { Type = ProductType.External },
                new Product { Type = ProductType.Temporary }
            };
            var possibleDeliveryDates = service.PossibleDeliveryDates(products, new DateTime(2019, 1, 1)).ToList();
            Assert.IsTrue(possibleDeliveryDates.Count() == 1, "Should return the that one sunday...!");
        }
    }
}