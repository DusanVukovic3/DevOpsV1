namespace Movie_Collection.Generic
{
    public interface ICRUDRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        T Create(T entity);
        T Update(T entity);
        void Delete(Guid id);
    }
}
