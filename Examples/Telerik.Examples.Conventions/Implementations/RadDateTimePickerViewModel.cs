using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Telerik.Examples.Conventions.Implementations
{
	[Export(typeof(IScreen))]
	public class RadDateTimePickerViewModel : Screen
	{
		public RadDateTimePickerViewModel()
		{
			DisplayName = "RadDateTimePicker";
			DateAndTimeValue = DateTime.Now;
			DateAndTimeValue2 = DateTime.Now;
			DateAndTimeValue3 = DateTime.Now;
		}

		public DateTime DateAndTimeValue { get; set; }
		public DateTime DateAndTimeValue2 { get; set; }
		public DateTime DateAndTimeValue3 { get; set; }
	}
}