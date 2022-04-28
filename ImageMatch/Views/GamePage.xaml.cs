using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using ImageMatch.Helpers;
using ImageMatch.Models;
using MediaManager;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImageMatch.Views
{


	public partial class GamePage : ContentPage
	{
		public List<ModelItem> SelectedIcons= new List<ModelItem>();
		List<RemoteImages> remoteImageList = new List<RemoteImages>();
		List<ModelItem> hiddenImageList = new List<ModelItem>();
		int SimilarSets = 1; //decide how many similar images should be selected to earn points
		int mainImageId = 1; //decide which image should be selected to earn points
		int lifeLostImageId = 2; //reduce points
		int score = 0;
		int totalMatchingImagesCount = 0;
		int lifeCount = Common.MaxFailAttemptCount;
		int GameLevel = 0;

		public GamePage ()
		{
			InitializeComponent ();
			InitGame();
		}

	

		private void InitGame()
        {
			
			GetRemoteImages();

			var random = new Random();

			//level
			GameLevel = Preferences.Get("LEVEL", 1);
			lblLevelCount.Text = Convert.ToString(GameLevel);
			Common.GridColumnCount = Math.Min(5, (Math.Max(2, GameLevel) + 1));
			Common.GridRowCount = Math.Max(2,GameLevel+1) ;

			totalMatchingImagesCount = 0;
			gamegrid.Children.Clear();
			int hiddenImageListId = 0;
			lifeCount = lifeCount = Common.MaxFailAttemptCount;
			score = 0;
			totalMatchingImagesCount = 0;
			SelectedIcons.Clear();
			hiddenImageList.Clear();
			lblScore.Text = Convert.ToString(score);
			lblLifeCount.Text = Convert.ToString(lifeCount);

			for (int column = 0; column <= Common.GridColumnCount; column++)
			{
				for (int row = 0; row < Common.GridRowCount; row++)
				{
					Button button = new Button()
					{
						CornerRadius = 25,
						BackgroundColor = Color.WhiteSmoke,
						WidthRequest = 50,
						HeightRequest = 50,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						BorderColor = Color.WhiteSmoke,
						BorderWidth = 1,
						Margin=1,
						ImageSource = Common.GamePageBackGroundImageURL,
						VerticalOptions = LayoutOptions.CenterAndExpand,
					};
					button.Clicked += Button_Clicked;
					gamegrid.Children.Add(button, column, row); //view , column, row


					//creating a map and assign images to a list randomly
					var nextImageIndex = random.Next(remoteImageList.Count);
					var nextImage = remoteImageList[nextImageIndex];

					hiddenImageList.Add(
					new ModelItem
					{
						ImageUrl = nextImage.URL,
						Id = hiddenImageListId,
						GridColumn = column,
						GridRow = row,
						IsMatched = false,
						TypeId = nextImage.Id
					});

					hiddenImageListId++;
				}
			}

			//getting total matching images
			totalMatchingImagesCount = hiddenImageList.Where(x => x.TypeId == mainImageId).Count();
			lblTargetCount.Text = totalMatchingImagesCount.ToString();
			gamegrid.IsEnabled = true;
		}

        private void GetRemoteImages()
        {

			using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(Common.RemoteImageUrl);
				remoteImageList = JsonConvert.DeserializeObject<List<RemoteImages>>(json);
            }

			//remoteImageList.Add(new RemoteImages()
			//{
			//    Id = 1,
			//    URL = "https://www.iconfinder.com/icons/3316547/download/png/128"
			//});

			//remoteImageList.Add(new RemoteImages()
			//{
			//    Id = 2,
			//    URL = "https://www.iconfinder.com/icons/3316551/download/png/128"
			//});

			//remoteImageList.Add(new RemoteImages()
			//{
			//    Id = 3,
			//    URL = "https://www.iconfinder.com/icons/3316544/download/png/128"
			//});

			//remoteImageList.Add(new RemoteImages()
			//{
			//    Id = 4,
			//    URL = "https://www.iconfinder.com/icons/3316536/download/png/128"
			//});

			//remoteImageList.Add(new RemoteImages()
			//{
			//    Id = 5,
			//    URL = "https://www.iconfinder.com/icons/3316538/download/png/128"
			//});

			//remoteImageList.Add(new RemoteImages()
			//{
			//    Id = 6,
			//    URL = "https://www.iconfinder.com/icons/3316546/download/png/128"
			//});


			//remoteImageList.Add(new RemoteImages()
			//{
			//    Id = 7,
			//    URL = "https://www.iconfinder.com/icons/3316540/download/png/128"
			//});


		}


		ModelItem selected;
        private void Button_Clicked(object sender, EventArgs e)
        {
			//create an object by selected button and add to a list
			selected  = new ModelItem();
			selected.Button = (Button)sender;
			selected.GridRow = Grid.GetRow(selected.Button);
			selected.GridColumn = Grid.GetColumn(selected.Button);
			selected.Id = SelectedIcons.Count() + 1;

			var selectedHiddenItem = hiddenImageList.Where(x => x.GridRow == selected.GridRow && x.GridColumn == selected.GridColumn).FirstOrDefault();
			
			selected.TypeId = selectedHiddenItem.TypeId;
			selected.Button.ImageSource = selectedHiddenItem.ImageUrl;

			if (SelectedIcons.Count > 0)
			{
				//selected item from list
				var selectedIcon = SelectedIcons.Where(x => x.GridColumn == selected.GridColumn && x.GridRow == selected.GridRow).FirstOrDefault();


				//todo - disable this by a setting
				var invalid = SelectedIcons.Where(x => x.IsMatched == false).FirstOrDefault();
				if (invalid != null)
				{
					//clear image from selected item list and 
					invalid.Button.ImageSource = Common.GamePageBackGroundImageURL;
					Task.WhenAll(
						invalid.Button.RotateYTo(-251 * 180, 250)
					);
					SelectedIcons.Remove(invalid);
					
				}

				//previous item
				var similarItems = SelectedIcons.Where(x => x.TypeId == mainImageId && x.TypeId == selected.TypeId);

				
				if (selected.TypeId == mainImageId)
                {
					Common.PlaySound("SmallWin.mp3");
					Matched(selected);
				}
                else if(selected.TypeId  == lifeLostImageId)
                {
					//if life lost image we reduce life
					lifeCount = lifeCount - 1;
					lblLifeCount.Text = Convert.ToString(lifeCount);
					selected.IsMatched = false;
					Common.PlaySound("614006__aarontheonly__roar3.mp3");
				}
                else
                {
					selected.IsMatched = false;
				}
			}
            else if (SelectedIcons.Count == 0 && selected.TypeId == mainImageId)
			{
				Common.PlaySound("SmallWin.mp3");
				Matched(selected);
			}

			SelectedIcons.Add(selected);

			Task.WhenAll(
			  selected.Button.RotateYTo(251 * 180, 250)
			);

			//lets take final decisions

			//if total images count achived then show animation as winner
			if (totalMatchingImagesCount == SelectedIcons.Where(x => x.TypeId == mainImageId).Count())
            {

               //todo enable all images after win

				gamegrid.IsEnabled = false;
				animationView_win.IsVisible = true;
				animationView_win.PlayAnimation();
				PlayWinSound();

				//next level


				Device.BeginInvokeOnMainThread(async () =>
				{
					await Task.Delay(5000);
					bool res = await DisplayAlert("", "Go to next level?", "Yes go to next level", "Replay this level");
					if (res)
					{
						GameLevel = GameLevel + 1;
						lblLevelCount.Text = Convert.ToString(GameLevel);
						Preferences.Set("LEVEL", GameLevel);
						InitGame();
					}
					else
					{
						InitGame();
					}
				});
				

			}

			//check fail count
			if(lifeCount == 0)
            {
				Common.PlaySound("LooseSound.mp3");

				Device.BeginInvokeOnMainThread(async () =>
				{
					bool res=false;
					if (Device.RuntimePlatform == Device.iOS)
					{
						res = await DisplayAlert("", "You loose!", "Restart", "Cancel");
					}
					else if (Device.RuntimePlatform == Device.Android)
					{
						res = await DisplayAlert("", "You loose!", "Restart", "Exit");
					}

					
					if (res)
					{
						InitGame();
					}
					else
					{
						if (Device.RuntimePlatform == Device.Android)
						{
							Environment.Exit(0);
						}
					}
				});

			}

		}

        private void PlayWinSound()
        {
			Common.PlaySound("518305__mrthenoronha__stage-clear-8-bit.mp3");
		}

        private void Matched(ModelItem selected)
        {
			score = score + 1;
			lblScore.Text = Convert.ToString(score);
			lifeCount = lifeCount + 1;
			lblLifeCount.Text = Convert.ToString(lifeCount);
			selected.IsMatched = true;

			lblTargetCount.Text = (totalMatchingImagesCount - score).ToString();
		}

        void animationView_win_OnFinishedAnimation(System.Object sender, System.EventArgs e)
        {
			animationView_win.IsVisible = false;
			animationView_win.StopAnimation();
		}

		async void btnResetGame_Clicked(System.Object sender, System.EventArgs e)
        {
			bool res = await DisplayAlert("", "Restart again?", "Restart", "Cancel");
            if (res)
            {
				InitGame();
			}

		}
    }

}

