namespace AV_test.DAL.Interfaces;

public interface IGenericRepository<T>
{
    bool Create(T entity);
    T? Get(T entity);
    bool Edit(T entity);
}