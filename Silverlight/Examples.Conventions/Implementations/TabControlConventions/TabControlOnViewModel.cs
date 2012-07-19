using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Examples.Conventions.Implementations.TabControlConventions
{
	[Export(typeof(IScreen))]
	public class TabControlOnViewModel : Conductor<IScreen>.Collection.OneActive
	{
		public TabControlOnViewModel()
		{
			DisplayName = "RadTabControl";
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

			ActivateItem(new FirstTabViewModel());

			Items.Add(new SecondTabViewModel());
		}
	}
}