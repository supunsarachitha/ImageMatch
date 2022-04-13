using System;
using Xamarin.Forms;

namespace ImageMatch.Models
{
	public class ModelItem
	{
		public int Id { get; set; }

		public Button Button { get; set; }

		public bool IsMatched { get; set; }

		public int TypeId { get; set; }

		public int GridRow { get; set; }

		public int GridColumn { get; set; }

		public string ImageUrl { get; set; }
	}
}

