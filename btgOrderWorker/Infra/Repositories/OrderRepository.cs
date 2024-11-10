using btgOrderWorker.Domain.interfaces;
using btgOrderWorker.Domain.models;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace btgOrderWorker.infra.Repositories;

public class OrderRepository : BaseRepository, IOrderRepository
{    
 public OrderRepository(IConfiguration configuration):base(configuration)
 {
    
 }

    public async  Task<Order> AddAsync(Order entity)
  {
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        using var transaction = connection.BeginTransaction();

        try
        {

            var insertOrderQuery = "INSERT INTO orders(id, customer_id) VALUES(@id, @customer_id)";
            await connection.ExecuteAsync(insertOrderQuery, new { id=entity.Id, customer_id=entity.CustomerId }, transaction);

     
            var insertItemQuery = "INSERT INTO orderitems ( order_id, product_id, amount,price) VALUES (@OrderId, @ProductId, @Amount, @Price)";
            foreach (var item in entity.products)
            {
                var parammeters = new {orderId=entity.Id, ProductId=item.Id,Amount=item.Amount,Price=item.Price};
                await connection.ExecuteAsync(insertItemQuery, parammeters, transaction);
            }


            transaction.Commit();
            return entity;
        }
        catch(Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }
    public void Delete(Order entity)
    {
        throw new NotImplementedException();
    }

    public async  Task<Order> GetByIdAsync(int id)
    {
              await using (var connection = new NpgsqlConnection(connectionString))
       {

        
           Order entity = await connection.QueryFirstOrDefaultAsync<Order>("SELECT Id, customer_id FROM orders WHERE Id=@id",
                                    new { id = id });
                                    
             return entity;
       }
    }

    public Task<List<Order>> List()
    {
        throw new NotImplementedException();
    }

    public async void Update(Order entity)
    {
    //            await using (var connection = new NpgsqlConnection(connectionString))
    //    {
    //         await connection.ExecuteScalarAsync<int>("UPDATE orders  set customer_id=@customer_id",
    //              entity);
    
             
    //    }

           using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
        using var transaction = connection.BeginTransaction();

        try
        {

           await connection.ExecuteScalarAsync<int>("UPDATE orders  set customer_id=@customer_id",     new{customer_id=entity.CustomerId});
           await connection.ExecuteScalarAsync<int>("delete from OrderItens where order_id=@order_id",     new{order_id=entity.Id});

     
            var insertItemQuery = "INSERT INTO OrderItens ( order_id, product_id, amount,price) VALUES (@OrderId, @ProductId, @Amount, @Price)";
            foreach (var item in entity.products)
            {
                var parammeters = new {orderId=entity.Id, ProductId=item.Id,Amount=item.Amount,Price=item.Price};
                await connection.ExecuteAsync(insertItemQuery, parammeters, transaction);
            }


            transaction.Commit();
        }
        catch(Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }

    public Task updateItensAsync(Order order)
    {
        throw new NotImplementedException();
    }
}