

using btgOrderWorker.Domain.models;

namespace btgOrderWorker.Domain.interfaces;

public interface IOrderService{
     Task ProcessMessageAsync(Order order);
}