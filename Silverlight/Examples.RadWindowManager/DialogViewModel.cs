using Caliburn.Micro;

namespace Examples.TelerikWindowManager
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