using btgOrderWorker.Domain.interfaces;
using btgOrderWorker.Domain.models;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace btgOrderWorker.infra.Repositories;

public class ProductRepository : BaseRepository, IProductRepository
{    
 public ProductRepository(IConfiguration configuration):base(configuration)
 {
    
 }

    public async Task<Product> AddAsync(Product entity)
    {
       {
       await using (var connection = new NpgsqlConnection(connectionString))
       {
        entity.Id=   await connection.ExecuteScalarAsync<int>("INSERT INTO products(description,price) VALUES(@description, @price); SELECT currval(pg_get_serial_sequence('products','id'));",
                 entity);
    
             return entity;
       }
    }
    }

    public async Task<Product> AddIfNotExists(Product product)
    {
          var productbase=await GetByDescriptionAsync(product.Description);
          if(productbase==null)
          return await AddAsync(product);
          else 
            return productbase;

         }

    public void Delete(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> GetByDescriptionAsync(string description)
    {
                          await using (var connection = new NpgsqlConnection(connectionString))
       {

        
           Product entity = await connection.QueryFirstOrDefaultAsync<Product>("SELECT Id, description, stock, price FROM products WHERE description=@description",
                                    new { description = description });
                                    
             return entity;
       }
    }

    public async   Task<Product> GetByIdAsync(int id)
    {
                    await using (var connection = new SqlConnection(connectionString))
       {

        
           Product entity = await connection.QueryFirstOrDefaultAsync<Product>("SELECT [Id], [description], [stock], [price] FROM [orders] WHERE [Id]=@id",
                                    new { id = id });
                                    
             return entity;
       }
    }

    public Task<List<Product>> List()
    {
        throw new NotImplementedException();
    }

    public void Update(Product entity)
    {
        throw new NotImplementedException();
    }
}