using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using HttpServerProxy.App.Utils;
using Utils;

namespace HttpServerProxy.App.Core
{
    public class ReroutingWrapper
    {
        private readonly IServerProxy _proxy;

        public ReroutingWrapper(IServerProxy proxy, string newHost)
        {
            _proxy = proxy;
            //test video: https://dl.dropboxusercontent.com/u/14357101/test.mp4
            Input = proxy.Input
                .BufferWithPeriodOfSilence(TimeSpan.FromSeconds(2d))
                .Select(x => x.ToRequest().RerouteToHost(newHost))
                .Publish().RefCount();
        }

        public IObservable<HttpRequestMessage> Input { get; private set; }
        public async Task SendString(string payload)
        {
            await _proxy.SendString(payload);
        }
    }

    public class SendingWrapper
    {
        public SendingWrapper(HttpClient client, ReroutingWrapper proxy)
        {
            proxy.Input
                .SelectMany(req => client.SendAsync(req).ToObservable())
                .SelectMany(resp => resp.ToRaw().ToObservable())
                .Subscribe(payload => proxy.SendString(payload));
        }
    }
}