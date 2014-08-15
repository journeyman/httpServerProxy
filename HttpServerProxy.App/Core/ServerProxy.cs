using System;
using System.IO;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using HttpServerProxy.App.Utils;
using Utils.Contracts;

namespace HttpServerProxy.App.Core
{
    public class ServerProxy : IServerProxy
    {
        private ServerProxy()
        {
            
        }

        public static async Task<IServerProxy> Connect(string host, int port)
        {
            var proxy = new ServerProxy();
            await proxy.run(host, port).ConfigureAwait(false);
            return proxy;
        }

        private async Task run(string host, int port)
        {
            var listener = new StreamSocketListener();
            listener.ConnectionReceived += listener_ConnectionReceived;

            await listener.BindEndpointAsync(new HostName(host), port.ToString());
        }

        private void listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            this.Log("new Connection received");
            _output = args.Socket.OutputStream;
            try
            {
                using (var reader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
                {
                    while (!reader.EndOfStream)
                    {
                        var nextLine = reader.ReadLine();
                        this.Log(nextLine);
                        _input.OnNext(nextLine);
                    }
                }
                _input.OnCompleted();

            }
            catch (Exception ex)
            {
                this.Log("exception, invalidating socket stuff");
                InvalidateSocketStuff();
                _input.OnError(ex);
            }
        }

        public async Task SendString(string payload)
        {
            _output.NotNull("Output stream is not yet initialized, forgot to await Connect()?");

            var bytes = Encoding.UTF8.GetBytes(payload);
            await _output.AsStreamForWrite().WriteAsync(bytes, 0, bytes.Length);
        }

        private void InvalidateSocketStuff()
        {
            _output = null;
        }

        private readonly ISubject<string> _input = new ReplaySubject<string>();
        private IOutputStream _output;
        public IObservable<string> Input { get { return _input; } }
        
    }
}
