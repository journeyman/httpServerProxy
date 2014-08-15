using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using HttpServerProxy.App.Core;
using HttpServerProxy.App.Utils;
using Microsoft.Phone.Controls;
using Utils;

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
            var newHost = "dl.dropboxusercontent.com";
            var client = new HttpClient() {BaseAddress = new Uri("https://" + newHost)};

            connection.Input
                .BufferWithPeriodOfSilence(TimeSpan.FromSeconds(2d))
                .Select(x => x.ToRequest().RerouteToHost(newHost))
                .SelectMany(req => client.SendAsync(req).ToObservable())
                .Catch<HttpResponseMessage, Exception>(ex => Observable.Throw<HttpResponseMessage>(new InvalidOperationException("request failed")))
                .SelectMany(resp => resp.ToRaw().ToObservable())
                .Do(payload => this.Log(payload, "Response: "))
                .Subscribe(payload => connection.SendString(payload));

            player.Source = new Uri("http://localhost:3333/u/14357101/test.mp4");
        }
    }
}