

using btgOrderWorker.Domain.interfaces;
using btgOrderWorker.Domain.models;

namespace btgOrderWorker.services;
public class OrderService : IOrderService
{

    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    public OrderService(IOrderRepository orderRepository,IProductRepository productRepository)
    {
        _orderRepository=orderRepository;
        _productRepository=productRepository;
    }

    public async  Task ProcessMessageAsync(Order order)
    {
        order.products.ForEach( (item)=>{if(item.Id ==0) item.Id =  _productRepository.AddIfNotExists(item).Result.Id;});

        if(await _orderRepository.GetByIdAsync(order.Id) ==null)
            await _orderRepository.AddAsync(order);
        else
             _orderRepository.Update(order);
    }
}

