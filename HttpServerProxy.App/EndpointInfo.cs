using Windows.Networking;

namespace HttpServerProxy.App
{
    public class EndpointInfo
    {
        public EndpointInfo(HostName host, int port)
        {
            Host = host;
            Port = port;
        }
        public HostName Host { get; private set; }
        public int Port { get; private set; }
    }
}