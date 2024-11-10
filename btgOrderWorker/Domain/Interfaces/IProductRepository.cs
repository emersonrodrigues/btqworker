using btgOrderWorker.Domain.models;

namespace btgOrderWorker.Domain.interfaces;

public interface IProductRepository:IBaseRepository<Product>{
     Task<Product> GetByDescriptionAsync(string description) ;
     Task<Product> AddIfNotExists(Product product);
}