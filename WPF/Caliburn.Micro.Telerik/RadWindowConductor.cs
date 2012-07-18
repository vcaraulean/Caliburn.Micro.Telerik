using System;
using Telerik.Windows.Controls;

namespace Caliburn.Micro.Telerik
{
	internal class RadWindowConductor
	{
		private bool deactivatingFromView;
		private bool deactivateFromViewModel;
		private bool actuallyClosing;
		private readonly RadWindow view;
		private readonly object model;

		public RadWindowConductor(object model, RadWindow view)
		{
			this.model = model;
			this.view = view;

			var activatable = model as IActivate;
			if (activatable != null)
			{
				activatable.Activate();
			}

			var deactivatable = model as IDeactivate;
			if (deactivatable != null)
			{
				view.Closed += Closed;
				deactivatable.Deactivated += Deactivated;
			}

			var guard = model as IGuardClose;
			if (guard != null)
			{
				view.PreviewClosed += PreviewClosed;
			}
		}

		private void Closed(object sender, EventArgs e)
		{
			view.Closed -= Closed;
			view.PreviewClosed -= PreviewClosed;

			if (deactivateFromViewModel)
			{
				return;
			}

			var deactivatable = (IDeactivate) model;

			deactivatingFromView = true;
			deactivatable.Deactivate(true);
			deactivatingFromView = false;
		}

		private void Deactivated(object sender, DeactivationEventArgs e)
		{
			if (!e.WasClosed)
			{
				return;
			}

			((IDeactivate) model).Deactivated -= Deactivated;

			if (deactivatingFromView)
			{
				return;
			}

			deactivateFromViewModel = true;
			actuallyClosing = true;
			view.Close();
			actuallyClosing = false;
			deactivateFromViewModel = false;
		}

		private void PreviewClosed(object sender, WindowPreviewClosedEventArgs e)
		{
			if (e.Cancel == true)
			{
				return;
			}

			var guard = (IGuardClose) model;

			if (actuallyClosing)
			{
				actuallyClosing = false;
				return;
			}

			bool runningAsync = false, shouldEnd = false;

			guard.CanClose(canClose =>
			{
				Execute.OnUIThread(() =>
				{
					if (runningAsync && canClose)
					{
						actuallyClosing = true;
						view.Close();
					}
					else
					{
						e.Cancel = !canClose;
					}

					shouldEnd = true;
				});
			});

			if (shouldEnd)
			{
				return;
			}

			e.Cancel = true;
			runningAsync = true;
		}
	}
}