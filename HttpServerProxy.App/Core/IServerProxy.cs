using System;
using System.Threading.Tasks;

namespace HttpServerProxy.App.Core
{
    public interface IServerProxy
    {
        IObservable<string> Input { get; }
        Task SendString(string payload);
    }
}