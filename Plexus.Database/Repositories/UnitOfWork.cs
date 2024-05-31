using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Database.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public void BeginTran()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTran()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBackTran()
        {
            _context.Database.RollbackTransaction();
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
