using System.ComponentModel.Composition;
using System.Threading;
using Caliburn.Micro;

namespace Telerik.Examples.Conventions.Implementations
{
	[Export(typeof(IScreen))]
	public class RadBusyIndicatorViewModel : Screen
	{
		public RadBusyIndicatorViewModel()
		{
			DisplayName = "RadBusyIndicator";
			IsBusy = true;
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			ThreadPool.QueueUserWorkItem(state =>
			{
				while (true)
				{
					Thread.Sleep(2000);
					Execute.OnUIThread(() =>
					{
						IsBusy = !IsBusy;
					});
				}
			}, null);
		}

		private bool isBusy;
		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				isBusy = value;
				NotifyOfPropertyChange(() => IsBusy);
			}
		}
	}
}