using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace ImageMatch.Views
{	
	public partial class GamePage : ContentPage
	{
		public List<SelectedItems> SelectedIcons= new List<SelectedItems>();

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
			//clear un matching images
			if (SelectedIcons.Count > 1)
			{
				var unmatchItem = SelectedIcons.Where(x => x.IsMatched==false).FirstOrDefault();
				if(unmatchItem != null)
				{
					unmatchItem.Button.ImageSource = "";
				
						SelectedIcons.Remove(unmatchItem);
				
				}
			}

			SelectedItems selected = new SelectedItems();
			selected.Button = (Button)sender;
			selected.GridRow = Grid.GetRow(selected.Button);
			selected.GridColumn = Grid.GetColumn(selected.Button);
			selected.TypeId = 1;
			selected.Button.ImageSource = "https://www.iconfinder.com/icons/3316547/download/png/128";
			int typeId = 1;

			int id = SelectedIcons.Count() + 1;

			if (SelectedIcons.Count > 0)
			{
				var lastInItem = SelectedIcons.OrderByDescending(x => x.Id).FirstOrDefault();
				if(typeId == lastInItem.TypeId)
                {
					selected.IsMatched = true;
				}
                else
                {
					selected.IsMatched = false;
					selected.TypeId = 2;
				}
			}
            else if (SelectedIcons.Count == 0)
			{
				selected.IsMatched = true;
			}

			SelectedIcons.Add(selected);



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
	}
}

