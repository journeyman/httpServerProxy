using System;
using System.Reactive.Linq;

namespace HttpServerProxy.App.Core
{
    public class ProxyWrapper
    {
        private readonly IServerProxy _proxy;

        public ProxyWrapper(IServerProxy proxy)
        {
            _proxy = proxy;

            var request = proxy.Input
                .Timeout(TimeSpan.FromSeconds(2))
                .Catch(Observable.Empty<string>())
                .Aggregate("", (s1, s2) => string.Concat(s1, s2, Environment.NewLine));
        }
    }
}