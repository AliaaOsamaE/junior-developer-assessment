namespace AxiomSysDL.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    T? GetById(object id);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
