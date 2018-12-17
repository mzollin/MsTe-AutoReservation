using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class AutoManager : ManagerBase
    {
        public IEnumerable<Auto> ReadAll()
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Autos.ToList();
            }
        }

        public Auto Read(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Autos.FirstOrDefault(a => a.Id == id);
            }
        }

        public void Create(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Attach(auto);
                context.Entry(auto).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Update(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Attach(auto);
                    context.Entry(auto).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw CreateOptimisticConcurrencyException(context, auto);
                }
            }
        }

        public void Delete(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Attach(auto);
                    context.Entry(auto).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw CreateOptimisticConcurrencyException(context, auto);
                }
            }
        }
    }
}