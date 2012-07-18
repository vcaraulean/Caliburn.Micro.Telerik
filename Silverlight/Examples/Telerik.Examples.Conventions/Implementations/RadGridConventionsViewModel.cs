using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Telerik.Examples.Conventions.Implementations
{
	public class DataItem : PropertyChangedBase
	{
		private string name;
		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				NotifyOfPropertyChange(() => Name);
			}
		}

		private bool isValid;
		public bool IsValid
		{
			get { return isValid; }
			set
			{
				isValid = value;
				NotifyOfPropertyChange(() => IsValid);
			}
		}
	}

	[Export(typeof(IScreen))]
	public class RadGridConventionsViewModel : Screen
	{
		public RadGridConventionsViewModel()
		{
			DisplayName = "RadGridView & RadTreeListView";

			DataItems = new List<DataItem>
			{
				new DataItem {Name = "First", IsValid = false},
				new DataItem {Name = "Second", IsValid = true}
			};
		}

		public List<DataItem> DataItems { get; set; }
		
		private DataItem selectedDataItem;
		public DataItem SelectedDataItem
		{
			get { return selectedDataItem; }
			set
			{
				selectedDataItem = value;
				NotifyOfPropertyChange(() => SelectedDataItem);
			}
		}
	}
}