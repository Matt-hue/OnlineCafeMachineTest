using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OnlineCafe.HostedServices
{
    public interface ICafeDrinksQueue
    {
        ValueTask<Func<CancellationToken, Task<string>>> DequeueAsync(CancellationToken cancellationToken);
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, Task<string>> workItem);
    }

    public class CafeWorkQueue : ICafeDrinksQueue
    {
        private readonly Channel<Func<CancellationToken, Task<string>>> _queue;

        public CafeWorkQueue(int capacity)
        {
            // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
            // which completes only when space became available. This leads to backpressure,
            // in case too many publishers/calls start accumulating.
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<Func<CancellationToken, Task<string>>>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(
            Func<CancellationToken, Task<string>> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<CancellationToken, Task<string>>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
