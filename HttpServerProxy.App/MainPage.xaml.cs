using System;
using System.Reactive.Linq;
using HttpServerProxy.App.Core;
using HttpServerProxy.App.Utils;
using Microsoft.Phone.Controls;

namespace HttpServerProxy.App
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            Run();
        }

        private async void Run()
        {
            var connection = await ServerProxy.Connect("localhost", 3333);
            player.Source = new Uri("http://localhost:3333/myvideo.mp4");

            var input = new ProxyWrapper(connection).Input;
            input.Subscribe(m => m.Log("got request!"));
        }
    }
}