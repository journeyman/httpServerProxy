using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Utils;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BufferWithPeriodOfSilence_Test()
        {
            Func<IObservable<long>> temp = () => Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
            var source = temp().Take(10)
                        .Concat(temp().Delay(TimeSpan.FromSeconds(1)).Take(10));
            var stream = source.BufferWithPeriodOfSilence(TimeSpan.FromMilliseconds(300));
            stream.Subscribe(x => Debug.WriteLine("Buffered: " + x.Count));
            stream.Wait();
        }

        [TestMethod]
        public void HttpRequestMessage_Parsing_Test()
        {
            const string raw = @"
GET /myvideo.mp4 HTTP/1.1
Cache-Control: no-cache
Connection: Keep-Alive
Pragma: getIfoFileURI.dlna.org
Accept: */*
User-Agent: NSPlayer/12.00.9200.16409 WMFSDK/12.00.9200.16409
GetContentFeatures.DLNA.ORG: 1
Host: localhost:3333
";

            var req = raw.Split(new []{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .ToRequest();

            Assert.AreEqual(HttpMethod.Get, req.Method);
            Assert.AreEqual("/myvideo.mp4", req.RequestUri.OriginalString);
            Assert.AreEqual("localhost:3333", req.Headers.Host);
        }
    }
}
