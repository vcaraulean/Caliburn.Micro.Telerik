using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Examples.Conventions.Implementations;

namespace Examples.Conventions
{
	[Export(typeof(IShell))]
	public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
	{
		[ImportMany]
		public IEnumerable<IScreen> ExportedScreens { get; set; }

		protected override void OnActivate()
		{
			base.OnActivate();

			var conventionSamples = ExportedScreens
				.Where(s => s.GetType().Namespace.StartsWith(typeof(RadBusyIndicatorViewModel).Namespace))
				.OrderBy(s => s.DisplayName);
			Items.AddRange(conventionSamples);
		}
	}
}