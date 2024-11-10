using System.Threading;
using System.Threading.Tasks;

namespace btgOrderWorker.Domain.interfaces;

    public interface IOrdersConsumer
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
    }

