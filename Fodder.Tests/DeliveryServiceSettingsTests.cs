using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fodder.Cnsl;

namespace Fodder.Tests
{
    [TestClass]
    public class DeliveryServiceSettingsTests
    {
        DeliveryServiceSettings settings = new DeliveryServiceSettings();

        [TestMethod]
        public void ShiftMethodReturns0ForRegularItems()
        {
            var product = new Product { Type = ProductType.Normal };
            var shift = settings.DeliveryDayShift(product);
            Assert.IsTrue(shift == 0, "Default settings for Normal products are zero days.");
        }

        [TestMethod]
        public void ShiftMethodReturns5ForExternal()
        {
            var product = new Product { Type = ProductType.External };
            var shift = settings.DeliveryDayShift(product);
            Assert.IsTrue(shift == 5, "Default settings for External products are five days.");
        }

        [TestMethod]
        public void ShiftMethodTakesHighestValueOfExternalAndDeliveryDaysInAdvance()
        {
            var product1 = new Product { Type = ProductType.External, OrderDaysInAdvance = 10 };
            var shift1 = settings.DeliveryDayShift(product1);
            Assert.IsTrue(shift1 == 10, "External product for 5 days is overridden by the higher value of DeliveryDaysInAdvance.");

            var product2 = new Product { Type = ProductType.External, OrderDaysInAdvance = 2 };
            var shift2 = settings.DeliveryDayShift(product2);
            Assert.IsTrue(shift2 == 5, "DeliveryDaysInAdvance is overridden by rule for External product.");
        }


    }
}