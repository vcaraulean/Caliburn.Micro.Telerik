using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Telerik.Examples.Conventions.Implementations
{
	[Export(typeof(IScreen))]
	public class RadMaskedTextBoxViewModel : Screen
	{
		public RadMaskedTextBoxViewModel()
		{
			DisplayName = "RadMaskedTextBox";
			StringToDisplay = "some text";
		}

		public string StringToDisplay { get; set; }
	}
}