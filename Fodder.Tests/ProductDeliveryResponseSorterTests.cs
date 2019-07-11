using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fodder.Cnsl;

namespace Fodder.Tests
{
    [TestClass]
    public class ProductDeliveryResponseSorterTests
    {
        [TestMethod]
        public void DeliverySorterPrioritizeTests()
        {
            var items = new ProductDeliveryResponse[]
            {
                new ProductDeliveryResponse { PostalCode = 1, DeliveryDate = DateTime.Now, IsGreenDelivery = false },
                new ProductDeliveryResponse { PostalCode = 2, DeliveryDate = DateTime.Now.AddDays(5), IsGreenDelivery = true },
                new ProductDeliveryResponse { PostalCode = 3, DeliveryDate = DateTime.Now.AddDays(2), IsGreenDelivery = true }
            };

            var greenCargoPrioritySorter = new ProductDeliveryResponseSorter(DateTime.Now, 3);

            Assert.IsFalse(greenCargoPrioritySorter.IsPrioritized(items[0]), "Isn't green enough.");
            Assert.IsFalse(greenCargoPrioritySorter.IsPrioritized(items[1]), "Too slow.");
            Assert.IsTrue(greenCargoPrioritySorter.IsPrioritized(items[2]), "Waow waow!");
        }

        [TestMethod]
        public void DeliverySorterLimitTests()
        {
            var greenCargoPrioritySorter = new ProductDeliveryResponseSorter(DateTime.Now, 3);

            Assert.IsTrue(greenCargoPrioritySorter.IsWithinLimit(DateTime.Now.AddDays(1)), "One...!");
            Assert.IsTrue(greenCargoPrioritySorter.IsWithinLimit(DateTime.Now.AddDays(2)), "Two...!");
            Assert.IsTrue(greenCargoPrioritySorter.IsWithinLimit(DateTime.Now.AddDays(3)), "This should be the maximum!!");
            Assert.IsFalse(greenCargoPrioritySorter.IsWithinLimit(DateTime.Now.AddDays(4)), "Fail fail fail.");
        }

        [TestMethod]
        public void SortingOfSameTypeUseObjectDefaultSort()
        {
            var items = new ProductDeliveryResponse[]
            {
                new ProductDeliveryResponse { PostalCode = 1, DeliveryDate = DateTime.Now, IsGreenDelivery = true },
                new ProductDeliveryResponse { PostalCode = 2, DeliveryDate = DateTime.Now.AddDays(5), IsGreenDelivery = true },
                new ProductDeliveryResponse { PostalCode = 3, DeliveryDate = DateTime.Now.AddDays(2), IsGreenDelivery = true }
            };

            var greenCargoPrioritySorter = new ProductDeliveryResponseSorter(DateTime.Now, 3);
            Array.Sort(items, greenCargoPrioritySorter);

            Assert.IsTrue(items[0].PostalCode == 1, "First date first.");
            Assert.IsTrue(items[1].PostalCode == 3, "Next is next?");
            Assert.IsTrue(items[2].PostalCode == 2, "And... last is last!");
        }

        [TestMethod]
        public void SortingWithGreeniesUseObjectDefaultSort()
        {
            var items = new ProductDeliveryResponse[]
            {
                new ProductDeliveryResponse { PostalCode = 1, DeliveryDate = DateTime.Now, IsGreenDelivery = false },
                new ProductDeliveryResponse { PostalCode = 2, DeliveryDate = DateTime.Now.AddDays(5), IsGreenDelivery = true },
                new ProductDeliveryResponse { PostalCode = 3, DeliveryDate = DateTime.Now.AddDays(1), IsGreenDelivery = true },
                new ProductDeliveryResponse { PostalCode = 4, DeliveryDate = DateTime.Now.AddDays(2), IsGreenDelivery = true },
                new ProductDeliveryResponse { PostalCode = 5, DeliveryDate = DateTime.Now.AddDays(3), IsGreenDelivery = true },
            };

            var greenCargoPrioritySorter = new ProductDeliveryResponseSorter(DateTime.Now, 3);
            Array.Sort(items, greenCargoPrioritySorter);

            Assert.IsTrue(items[0].PostalCode == 3, "Third item is within three days and green!");
            Assert.IsTrue(items[1].PostalCode == 4);
            Assert.IsTrue(items[2].PostalCode == 5);
            Assert.IsTrue(items[3].PostalCode == 1);
            Assert.IsTrue(items[4].PostalCode == 2);
        }
    }
}
