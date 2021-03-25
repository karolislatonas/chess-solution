using System;
using System.Threading.Tasks;

namespace Chess.Api.Notification.Notifiers
{
    public interface INotifier : IDisposable
    {
        Task StartAsync();
    }
}
