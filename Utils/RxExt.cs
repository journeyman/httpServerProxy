using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Utils
{
    public static class RxExt
    {
        public static IObservable<IList<T>> BufferWithPeriodOfSilence<T>(this IObservable<T> This, TimeSpan timespan, IScheduler scheduler = null)
        {
            var pub = This.Publish().RefCount();

            return Observable.Create<IList<T>>(subj =>
            {
                return pub.Buffer(() => pub.Throttle(timespan, scheduler ?? Scheduler.Default))
                    .Subscribe(subj);
            });
        }
    }
}