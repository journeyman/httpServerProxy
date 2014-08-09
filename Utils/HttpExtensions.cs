using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using JetBrains.Annotations;
using Utils.Contracts;

namespace Utils
{
    

    public static class HttpExtensions
    {
        [NotNull]
        public static HttpRequestMessage ToRequest(this IEnumerable<string> raw)
        {
            raw = raw.ArgNotNull("raw");

            var firstLine = raw.First();
            var splitted = firstLine.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
            Contract.Assert<InvalidOperationException>(splitted.Length >= 2, "cannot parse first line: " + firstLine);

            var method = new HttpMethod(splitted[0]);
            var url = new Uri(splitted[1], UriKind.RelativeOrAbsolute);
            var req = new HttpRequestMessage(method, url);
            foreach (var str in raw.Skip(1))
            {
                var i = str.IndexOf(':');
                Contract.Assert<InvalidOperationException>(i >= 0, "cannot parse header: " + str);

                var key = str.Substring(0, i).Trim();
                var val = str.Substring(i + 1).Trim();

                req.Headers.Add(key, val);
            }
            return req;
        }
    }
}