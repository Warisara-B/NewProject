using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Database
{
    public interface IUnitOfWork
    {
        void BeginTran();
        void RollBackTran();
        void CommitTran();

        void Complete();
        Task CompleteAsync();
    }
}
