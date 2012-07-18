using Caliburn.Micro;

namespace Telerik.Examples.RadWindowManager
{
	public class DialogViewModel : Screen
	{
		public DialogViewModel()
		{
			DisplayName = "Sample dialog view";
		}

		public void Close()
		{
			TryClose();
		}
	}
}