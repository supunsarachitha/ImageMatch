using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ImageMatch.Helpers;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;

namespace ImageMatch.Views
{	
	public partial class IntroductionPage : ContentPage
	{
		public IntroductionPage ()
		{
			InitializeComponent ();
			PlaySound("bensound-jazzcomedy.mp3");

			List<string> descriptionTexts = new List<string>();
			descriptionTexts.Add("This is Jim, \n Our friendly farmer");
			descriptionTexts.Add("He lost his ducks, \n he is going to search the jungle");
			descriptionTexts.Add("He needs our help to find them");
			descriptionTexts.Add("Lets help him...");
			descriptionTexts.Add("you will meet many animals in the jungle");
			descriptionTexts.Add("But, \n be careful..!");
			descriptionTexts.Add("specially from the tiger!");
			descriptionTexts.Add("you will loose life points if tiger catches you");
			descriptionTexts.Add("Dont worry \n you will gain it back when you find a duck");
			descriptionTexts.Add("Find all the ducks \n and bring them back to the farm");
			descriptionTexts.Add("Good luck!!");

			Device.BeginInvokeOnMainThread(async () =>
			{
				await imgJim.FadeTo(1, 2000);

				int i  = 1;
				foreach (var item in descriptionTexts)
                {
					
					if(lblDescriptionTxt.Text.Length > 1)
                    {
						await lblDescriptionTxt.FadeTo(0, 1000);
					}
					
					lblDescriptionTxt.Text = item;
					await lblDescriptionTxt.FadeTo(1, 500);

                    if (i == 3)
                    {
						imgDescriptionImage.Source = "https://www.iconfinder.com/icons/3316547/download/png/128";
						await imgDescriptionImage.FadeTo(1, 500);
                    }
					else if (i == 7)
					{
						imgDescriptionImage.Source = "https://www.iconfinder.com/icons/3316551/download/png/128";
						await imgDescriptionImage.FadeTo(1, 500);
					}
					else if (i == 8)
					{
						imgDescriptionImage.Source = "https://www.iconfinder.com/icons/285639/download/png/128";
						await imgDescriptionImage.FadeTo(1, 500);
					}
					else if (i == 11)
					{

						imgDescriptionImage.IsVisible = false;
						btnAnimationView.IsVisible = true;
					}
					else
                    {
						imgDescriptionImage.Source = "";
						await imgDescriptionImage.FadeTo(0, 500);
					}



					await Task.Delay(5000);

					i++;
				}
				
			});
		}

		private async void PlaySound(string name)
		{
			var stream = GetStreamFromFile(name);
			
			Common.AudioPlayer.Load(stream);
			Common.AudioPlayer.Play();
		}

		Stream GetStreamFromFile(string filename)
		{
			var assembly = typeof(App).GetTypeInfo().Assembly;

			var stream = assembly.GetManifestResourceStream("ImageMatch.Audio." + filename);

			return stream;
		}

		void btnAnimationView_Clicked(System.Object sender, System.EventArgs e)
        {
			Common.AudioPlayer.Stop();
			Xamarin.Essentials.Preferences.Set("FIRST_TIME_USER", false);
			App.Current.MainPage = new GamePage();
        }
    }
}

