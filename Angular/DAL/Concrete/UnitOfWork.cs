using System.Data.Entity;
using DAL.Interfaces.Repository;

namespace DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            context?.Dispose();
        }

        public void Commit()
        {
            context?.SaveChanges();
        }
    }
}