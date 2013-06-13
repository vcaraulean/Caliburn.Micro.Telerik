using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Examples.Conventions.Implementations
{
	[Export(typeof(IScreen))]
	public class RadMaskedControlsViewModel : Screen
	{
		public RadMaskedControlsViewModel()
		{
			DisplayName = "RadMasked* controls";

			SomeString = "some string value";
			PhoneNumber = "0123456789";
			CurrencyValue = 12345.78;
			DateTimeValue = new DateTime(2012, 12, 12);
			NumericValue = 9876543;
		}

		public string SomeString { get; set; }
		public string PhoneNumber { get; set; }
		public double CurrencyValue { get; set; }
		public DateTime DateTimeValue { get; set; }
		public int NumericValue { get; set; }

	}
}