using Microsoft.EntityFrameworkCore;

namespace AxiomSysDL.Repositories;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context;

    public UnitOfWork(TContext context)
    {
        _context = context;
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        return new Repository<T>(_context);
    }

    public int Commit()
    {
        return _context.SaveChanges();
    }
}
