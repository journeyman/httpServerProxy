using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
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
    }
}
