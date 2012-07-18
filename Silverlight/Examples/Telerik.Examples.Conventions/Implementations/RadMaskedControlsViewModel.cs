using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Telerik.Examples.Conventions.Implementations
{
	[Export(typeof(IScreen))]
	public class RadMaskedControlsViewModel : Screen
	{
		public RadMaskedControlsViewModel()
		{
			DisplayName = "RadMaskedTextBox";
			
			SomeString = "some string value";
			PhoneNumber = "0123456789";
			AnotherPhoneNumber = "9876543210";
		}

		public string SomeString { get; set; }
		public string PhoneNumber { get; set; }
		public string AnotherPhoneNumber { get; set; }
	}
}