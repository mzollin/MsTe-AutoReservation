using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager : ManagerBase
    {
        public IEnumerable<Kunde> ReadAll()
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Kunden.ToList();
            }
        }

        public Kunde Read(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Kunden.FirstOrDefault(a => a.Id == id);
            }
        }

        public void Create(Kunde kunde)
        {
            CheckCustomerAge(kunde);

            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Attach(kunde);
                context.Entry(kunde).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Update(Kunde kunde)
        {
            CheckCustomerAge(kunde);

            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Attach(kunde);
                    context.Entry(kunde).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw CreateOptimisticConcurrencyException(context, kunde);
                }
            }
        }

        public void Delete(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Attach(kunde);
                    context.Entry(kunde).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw CreateOptimisticConcurrencyException(context, kunde);
                }
            }
        }
        private void CheckCustomerAge(Kunde customer)
        {
            if (customer.Birthday > DateTime.Now.AddYears(-18))
            {
                throw new CustomerNotOfAgeException(customer.Birthday);
            }
        }
    }
}