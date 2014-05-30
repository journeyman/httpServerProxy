using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HttpServerProxy.App.Resources;

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
            await ServerProxy.Start("localhost", 3333);
            player.Source = new Uri("http://localhost:3333");
        }
    }
}