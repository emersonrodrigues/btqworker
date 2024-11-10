namespace btgOrderWorker.Domain.interfaces;

public interface IBaseRepository<T> where T:class{
  Task<T> GetByIdAsync(int id) ;
  Task<List<T>> List() ;
  Task<T> AddAsync(T entity) ;
  void Update(T entity);
  void Delete(T entity) ;
}