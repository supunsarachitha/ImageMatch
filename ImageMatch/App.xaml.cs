using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageMatch.Services;
using ImageMatch.Views;
using MediaManager;
using ImageMatch.Helpers;

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
            if (Common.AudioPlayer.IsPlaying)
            {
                Common.AudioPlayer.Pause();
            }
        }

        protected override void OnResume()
        {
        }
    }
}
