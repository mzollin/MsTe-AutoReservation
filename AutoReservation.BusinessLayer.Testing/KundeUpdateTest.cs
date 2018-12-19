using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class KundeUpdateTest
        : TestBase
    {
        private KundeManager target;
        private KundeManager Target => target ?? (target = new KundeManager());

        [Fact]
        public void CreateKundeTest()
        {
            var newCustomer = new Kunde
            {
                FirstName = "Testian",
                Surname = "Beispielsfelder",
                Birthday = new DateTime(1993, 7, 9)
            };
            var customers = Target.GetAll();

            Target.Create(newCustomer);

            Assert.ProperSuperset(new HashSet<Kunde>(customers), new HashSet<Kunde>(Target.GetAll()));
            var newCustomerFromDb = Target.GetById(customers.OrderBy(c => c.Id).Last().Id + 1);
            Assert.NotNull(newCustomerFromDb);
            Assert.Equal(newCustomer.FirstName, newCustomerFromDb.FirstName);
            Assert.Equal(newCustomer.Surname, newCustomerFromDb.Surname);
            Assert.Equal(newCustomer.Birthday, newCustomerFromDb.Birthday);
        }

        [Fact]
        public void UpdateKundeTest()
        {
            var customer = Target.GetById(1);
            Assert.NotEqual("Beispielshofer", customer.Surname);

            customer.Surname = "Beispielshofer";
            Target.Update(customer);

            var modifiedCustomer = Target.GetById(customer.Id);
            Assert.Equal("Beispielshofer", modifiedCustomer.Surname);
        }

        [Fact]
        public void DeleteKundeTest()
        {
            var customer = Target.GetById(1);
            Assert.NotNull(customer);

            Target.Delete(customer);

            var deletedCustomer = Target.GetById(1);
            Assert.Null(deletedCustomer);
        }
    }
}
