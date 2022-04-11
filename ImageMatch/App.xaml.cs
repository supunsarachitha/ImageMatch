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

            bool firstTimeUser = Xamarin.Essentials.Preferences.Get("FIRST_TIME_USER", true);
            if (firstTimeUser)
            {
                MainPage = new NavigationPage(new IntroductionPage());
            }
            else
            {
                MainPage = new GamePage();
            }
            
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
