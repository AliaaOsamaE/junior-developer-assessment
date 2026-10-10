using Microsoft.EntityFrameworkCore;

namespace AxiomSysDL.Repositories;

public interface IUnitOfWork<TContext> where TContext : DbContext
{
    IRepository<T> GetRepository<T>() where T : class;
    int Commit();
}
