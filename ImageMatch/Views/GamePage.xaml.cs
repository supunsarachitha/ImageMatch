using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace ImageMatch.Views
{	
	public partial class GamePage : ContentPage
	{	
		public GamePage ()
		{
			InitializeComponent ();

			int colCount = 6;
			int rowCount = 6;

			
			for (int column = 0; column <= colCount; column++)
            {
                for (int row = 0; row < rowCount; row++)
                {
					Button button = new Button()
					{
						CornerRadius = 25,
						BackgroundColor= Color.Transparent,
						ImageSource = "https://www.iconfinder.com/icons/3316547/download/png/128",
						WidthRequest= 50,
						HeightRequest=50,
						HorizontalOptions=LayoutOptions.Fill,
						BorderColor=Color.WhiteSmoke,
						BorderWidth = 1
					};
                    button.Clicked += Button_Clicked;

					gamegrid.Children.Add(button, column, row); //view , column, row
				}
			}
             
			
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
			var btn = (Button)sender;

			int row = Grid.GetRow(btn);
			int column = Grid.GetColumn(btn);
		}
    }
}

