using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Utils.Contracts;

namespace Utils
{
    public static class HttpExt
    {
        public static HttpRequestMessage RerouteToHost(this HttpRequestMessage request, string host)
        {
            request.Headers.Host = host;
            return request;
        }

        [NotNull]
        public static HttpRequestMessage ToRequest(this IEnumerable<string> raw)
        {
            raw = raw.ArgNotNull("raw");

            var firstLine = raw.First();
            var splitted = firstLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Contract.Assert<InvalidOperationException>(splitted.Length >= 2, "cannot parse first line: " + firstLine);

            var method = new HttpMethod(splitted[0]);
            var url = new Uri(splitted[1], UriKind.RelativeOrAbsolute);
            var req = new HttpRequestMessage(method, url);
            foreach (var str in raw.Skip(1).Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var i = str.IndexOf(':');
                Contract.Assert<InvalidOperationException>(i >= 0, "cannot parse header: " + str);

                var key = str.Substring(0, i).Trim();
                var val = str.Substring(i + 1).Trim();

                req.Headers.Add(key, val);
            }
            return req;
        }


        public static async Task<string> ToRaw(this HttpResponseMessage This)
        {
            var sb = new StringBuilder();

            //status line
            sb.AppendFormat("HTTP/1.1 {0} {1}", This.ReasonPhrase, This.StatusCode.ToString());
            sb.AppendLine();

            //headers
            sb.AppendLine(This.Headers.ToString());

            //empty line
            sb.AppendLine();

            //content
            var content = await This.Content.ReadAsStringAsync();
            sb.Append(content);

            var response = sb.ToString();
            return response;
        }
    }
}