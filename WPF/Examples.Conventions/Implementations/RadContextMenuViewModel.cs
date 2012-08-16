using System.ComponentModel.Composition;
using Caliburn.Micro;
using Telerik.Windows.Controls;

namespace Examples.Conventions.Implementations
{
	[Export(typeof (IScreen))]
	public class RadContextMenuViewModel : Screen
	{
		public RadContextMenuViewModel()
		{
			DisplayName = "RadContextMenu";
		}

		private static void Alert(string message)
		{
			RadWindow.Alert(new DialogParameters {Header = "It works", Content = message});
		}

		public void ExecuteAction1()
		{
			Alert("Action 1 executed");
		}

		public void ExecuteAction2()
		{
			Alert("Action 2 executed");
		}

		public void ExecuteAction3()
		{
			Alert("Action 3 executed");
		}
	}
}