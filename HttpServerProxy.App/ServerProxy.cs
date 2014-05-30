using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Microsoft.Phone.Logging;

namespace HttpServerProxy.App
{
    public static class Ext
    {
        public static void Log(this object This, string message, [CallerMemberName] string source = "unknown method")
        {
            Debug.WriteLine(source + " >>> " + message);
        }
    }

    public class ServerProxy
    {
        private ServerProxy()
        {
            
        }

        public static async Task Start(string host, int port)
        {
            await new ServerProxy().run(host, port);
        }

        private async Task run(string host, int port)
        {
            var listener = new StreamSocketListener();
            listener.ConnectionReceived += listener_ConnectionReceived;
            await listener.BindEndpointAsync(new HostName(host), port.ToString());
        }

        async void listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            var sb = new StringBuilder();
            string requestLine = null;
            //TODO: exceptions handling
            using (var reader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                do
                {
                    requestLine = await reader.ReadLineAsync();
                    sb.AppendLine(requestLine);
                } while (requestLine != null);
                this.Log(sb.ToString());
            }
        }
    }
}
