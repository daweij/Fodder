using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fodder.Cnsl;

namespace Fodder.Tests
{
    [TestClass]
    public class ProductDeliveryResponseTests
    {
        [TestMethod]
        public void SortWithEqualDateReturnGreensFirst()
        {
            var items = new ProductDeliveryResponse[]
            {
                new ProductDeliveryResponse { PostalCode = 1, DeliveryDate = DateTime.Now, IsGreenDelivery = false },
                new ProductDeliveryResponse { PostalCode = 2, DeliveryDate = DateTime.Now, IsGreenDelivery = true },
            };

            Array.Sort(items);
            Assert.IsTrue(items[0].PostalCode == 2, "Same same, but different.");
        }
    }
}
