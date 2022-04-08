using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediaManager;
using Xamarin.Forms;

namespace ImageMatch.Views
{	
	public partial class GamePage : ContentPage
	{
		public List<SelectedItems> SelectedIcons= new List<SelectedItems>();
		List<RemoteImageList> remoteImageList = new List<RemoteImageList>();
		List<SelectedItems> hiddenImageList = new List<SelectedItems>();
		int SimilarSets = 1; //decide how many similar images should be selected to earn points
		int colCount = 5;
		int rowCount = 8;
		int mainImageId = 1; //decide which image should be selected to earn points
		int score = 0;
		int totalMatchingImagesCount = 0;

		public GamePage ()
		{
			InitializeComponent ();
			GetRemoteImages();

			var random = new Random();

			int hiddenImageListId = 0;

			for (int column = 0; column <= colCount; column++)
            {
                for (int row = 0; row < rowCount; row++)
                {
					

					Button button = new Button()
					{
						CornerRadius = 25,
						BackgroundColor= Color.WhiteSmoke,
						WidthRequest= 50,
						HeightRequest=50,
						HorizontalOptions=LayoutOptions.FillAndExpand,
						BorderColor=Color.WhiteSmoke,
						BorderWidth = 1,
						ImageSource="",
						VerticalOptions = LayoutOptions.FillAndExpand,
					};
                    button.Clicked += Button_Clicked;
					gamegrid.Children.Add(button, column, row); //view , column, row


					//creating a map and assign images to a list randomly
					var nextImageIndex = random.Next(remoteImageList.Count);
					var nextImage = remoteImageList[nextImageIndex];
					
					hiddenImageList.Add(
					new SelectedItems
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

		}

        private void GetRemoteImages()
        {
			remoteImageList.Add(new RemoteImageList()
			{
				Id = 1,
				URL= "https://www.iconfinder.com/icons/3316547/download/png/128"
			});

			remoteImageList.Add(new RemoteImageList()
			{
				Id = 2,
				URL = "https://www.iconfinder.com/icons/3316551/download/png/128"
			});

			remoteImageList.Add(new RemoteImageList()
			{
				Id = 3,
				URL = "https://www.iconfinder.com/icons/3316544/download/png/128"
			});

		}


		SelectedItems selected;
        private void Button_Clicked(object sender, EventArgs e)
        {
			//create an object by selected button and add to a list
			selected  = new SelectedItems();
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

				if(selectedIcon != null)
				{
					//clear image from selected item list and 
					selected.Button.ImageSource = "";

					SelectedIcons.Remove(selectedIcon);
					return;
				}

				//previous item
				var similarItems = SelectedIcons.Where(x => x.TypeId == mainImageId && x.TypeId == selected.TypeId);

				
				if (selected.TypeId == mainImageId)
                {
					Matched(selected);
				}
                else
                {
					selected.IsMatched = false;
				}
			}
            else if (SelectedIcons.Count == 0 && selected.TypeId == mainImageId)
			{
				Matched(selected);
			}

			SelectedIcons.Add(selected);


			//if total images count achived then show animation as winner
			if (totalMatchingImagesCount == SelectedIcons.Where(x => x.TypeId == mainImageId).Count())
            {
				animationView_win.IsVisible = true;
				animationView_win.PlayAnimation();
				PlayWinSound();
				
			}

		}

        private async void PlayWinSound()
        {
			await CrossMediaManager.Current.Play("https://files.freemusicarchive.org/storage-freemusicarchive-org/music/no_curator/Yung_Kartz/July_2019/Yung_Kartz_-_02_-_Levels.mp3");
		}

        private void Matched(SelectedItems selected)
        {
			score = score + 1;
			lblScore.Text = Convert.ToString(score);
			selected.IsMatched = true;
		}

        void animationView_win_OnFinishedAnimation(System.Object sender, System.EventArgs e)
        {
			animationView_win.IsVisible = false;
			animationView_win.StopAnimation();
		}
    }

    public class SelectedItems
    {
		public int Id { get; set; }

		public Button Button { get; set; }

		public bool IsMatched { get; set; }

		public int TypeId { get; set; }

		public int GridRow { get; set; }

		public int GridColumn { get; set; }

		public string ImageUrl { get; set; }
	}

	public class RemoteImageList
    {
		public int Id { get; set; }

		public string URL { get; set; }

	}
}

