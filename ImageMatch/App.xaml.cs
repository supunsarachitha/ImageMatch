using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageMatch.Services;
using ImageMatch.Views;
using MediaManager;

namespace ImageMatch
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            CrossMediaManager.Current.Init();
            DependencyService.Register<MockDataStore>();
            MainPage = new GamePage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
