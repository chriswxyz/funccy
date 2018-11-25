using System;
using System.Threading;
using System.Threading.Tasks;

namespace Funccy
{
    public interface ILimiter
    {
        Task<T> Run<T>(Func<T> action);
        Task<T> Run<T>(Func<T> action, CancellationToken cancel);
        Task<T> Run<T>(Func<Task<T>> action);
        Task<T> Run<T>(Func<Task<T>> action, CancellationToken cancel);
    }
}