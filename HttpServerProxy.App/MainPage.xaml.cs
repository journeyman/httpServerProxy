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

            var request = await connection.Input
                .Timeout(TimeSpan.FromSeconds(2))
                .Catch(Observable.Empty<string>())
                .Aggregate("", string.Concat);

            this.Log("Thre request iiiiiissssss:");
            this.Log(request);
        }
    }
}