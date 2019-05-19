using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MotorcycleNewb.Models.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext ApplicationDb { get; set; }
        public IRepository<Profile> Profiles { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            ApplicationDb = db;
            Profiles = new Repository<Profile>(ApplicationDb);
        }
        public void Dispose()
        {
            ApplicationDb.Dispose();

        }

        public void Commit()
        {
            ApplicationDb.SaveChanges();
        }

        public void Update(Profile element)
        {
            ApplicationDb.Entry(element).State = EntityState.Modified;
        }

        public void Delete(Profile element)
        {
            ApplicationDb.Entry(element).State = EntityState.Deleted;
            Profiles.Remove(element);
        }
    }
}