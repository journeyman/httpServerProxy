using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace HttpServerProxy.App.Utils
{
    public static class Ext
    {
        public static void Log(this object This, string message, [CallerMemberName] string source = "unknown method")
        {
            Debug.WriteLine(source + " >>> " + message);
        }

        //public static Task<EndpointInfo> GetConnectedRemoteHostName(this ServerProxy proxy)
        //{
        //    if (proxy.ConnectedRemoteEndpoint != null)
        //    {
        //        proxy.Dispose();
        //        return Task.FromResult(proxy.ConnectedRemoteEndpoint);
        //    }
        //    else
        //    {
        //        Action handler = null;
        //        var completion = new TaskCompletionSource<EndpointInfo>();
        //        handler = () =>
        //        {
        //            proxy.ConnectionRecieved -= handler;
        //            proxy.Dispose();
        //            completion.SetResult(proxy.ConnectedRemoteEndpoint);
        //        };
        //        proxy.ConnectionRecieved += handler;
        //        return completion.Task;
        //    }
        //}

        //public static Task<StreamSocket> GetConnectedSocket(this ServerProxy proxy)
        //{
        //    if (proxy.ConnectedSocket != null)
        //    {
        //        return Task.FromResult(proxy.ConnectedSocket);
        //    }
        //    else
        //    {
        //        Action handler = null;
        //        var completion = new TaskCompletionSource<StreamSocket>();
        //        handler = () =>
        //        {
        //            proxy.ConnectionRecieved -= handler;
        //            completion.SetResult(proxy.ConnectedSocket);
        //        };
        //        proxy.ConnectionRecieved += handler;
        //        return completion.Task;
        //    }
        //}

        public static async Task<string> ReadInputAsText(this IInputStream input)
        {
            const int BufferSize = 8192;
            var request = new StringBuilder();
            using (input)
            {
                var data = new byte[BufferSize];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }
            return request.ToString();
        }
    }
}