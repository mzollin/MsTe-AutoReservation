using System;
using System.Collections.Generic;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private AutoManager target;
        private AutoManager Target => target ?? (target = new AutoManager());

        [Fact]
        public void CreateAutoTest()
        {
            var newCar = new StandardAuto
            {
                Brand = "TestBrand",
                DailyRate = 99
            };
            var cars = Target.GetAll();

            Target.Create(newCar);

            Assert.ProperSuperset(new HashSet<Auto>(cars), new HashSet<Auto>(Target.GetAll()));
            var newCarFromDb = Target.GetById(cars.OrderBy(c => c.Id).Last().Id + 1);
            Assert.NotNull(newCarFromDb);
            Assert.Equal(newCar.Brand, newCarFromDb.Brand);
            Assert.Equal(newCar.DailyRate, newCarFromDb.DailyRate);
        }

        [Fact]
        public void UpdateAutoTest()
        {
            var car = Target.GetById(1);
            Assert.NotEqual("ModifiedBrand", car.Brand);

            car.Brand = "ModifiedBrand";
            Target.Update(car);

            var modifiedCar = Target.GetById(car.Id);
            Assert.Equal("ModifiedBrand", modifiedCar.Brand);
        }

        [Fact]
        public void DeleteAutoTest()
        {
            var car = Target.GetById(1);
            Assert.NotNull(car);

            Target.Delete(car);

            var deletedCar = Target.GetById(1);
            Assert.Null(deletedCar);
        }
    }
}
