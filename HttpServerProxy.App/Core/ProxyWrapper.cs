using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using HttpServerProxy.App.Utils;
using Utils;

namespace HttpServerProxy.App.Core
{
    public class ProxyWrapper
    {
        public ProxyWrapper(IServerProxy proxy)
        {
            Input = proxy.Input
                .BufferWithPeriodOfSilence(TimeSpan.FromSeconds(2d))
                .Select(x => x.ToRequest())
                //TODO:SS: figure out this stuff at last!
                .Publish().RefCount();
        }

        public IObservable<HttpRequestMessage> Input { get; private set; }
    }

}