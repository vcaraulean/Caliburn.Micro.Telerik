using System;
using System.Collections.Generic;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace Caliburn.Micro.Telerik
{
	/// <summary>
	/// A service that manages windows.
	/// Implementation for <see cref="RadWindow"/>
	/// </summary>
	public class RadWindowManager : IWindowManager
	{
		/// <summary>
		/// Shows a modal dialog for the specified model.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <param name="context">The context.</param>
		public virtual void ShowDialog(object rootModel, object context = null)
		{
			var view = EnsureWindow(rootModel, ViewLocator.LocateForModel(rootModel, null, context));
			ViewModelBinder.Bind(rootModel, view, context);

			var haveDisplayName = rootModel as IHaveDisplayName;
			if (haveDisplayName != null && !ConventionManager.HasBinding(view, RadWindow.HeaderProperty))
			{
				var binding = new Binding("DisplayName") { Mode = BindingMode.TwoWay };
				view.SetBinding(RadWindow.HeaderProperty, binding);
			}

			view.WindowStartupLocation = WindowStartupLocation.CenterScreen;

			new RadWindowConductor(rootModel, view);

			view.ShowDialog();
		}

		public void ShowNotification(object rootModel, int durationInMilliseconds, object context)
		{
			throw new NotImplementedException();
		}

		public void ShowPopup(object rootModel, object context, IDictionary<string, object> settings)
		{
			throw new NotImplementedException();
		}

		private static RadWindow EnsureWindow(object model, object view)
		{
			var window = view as RadWindow;

			if (window == null)
			{
				window = new RadWindow { Content = view };
				window.SetValue(View.IsGeneratedProperty, true);
			}

			return window;
		}

		class RadWindowConductor
		{
			bool deactivatingFromView;
			bool deactivateFromViewModel;
			bool actuallyClosing;
			readonly RadWindow view;
			readonly object model;

			public RadWindowConductor(object model, RadWindow view)
			{
				this.model = model;
				this.view = view;

				var activatable = model as IActivate;
				if (activatable != null)
					activatable.Activate();

				var deactivatable = model as IDeactivate;
				if (deactivatable != null)
				{
					view.Closed += Closed;
					deactivatable.Deactivated += Deactivated;
				}

				var guard = model as IGuardClose;
				if (guard != null)
					view.PreviewClosed += PreviewClosed;
			}

			void Closed(object sender, EventArgs e)
			{
				view.Closed -= Closed;
				view.PreviewClosed -= PreviewClosed;

				if (deactivateFromViewModel)
					return;

				var deactivatable = (IDeactivate)model;

				deactivatingFromView = true;
				deactivatable.Deactivate(true);
				deactivatingFromView = false;
			}

			void Deactivated(object sender, DeactivationEventArgs e)
			{
				if (!e.WasClosed)
					return;

				((IDeactivate)model).Deactivated -= Deactivated;

				if (deactivatingFromView)
					return;

				deactivateFromViewModel = true;
				actuallyClosing = true;
				view.Close();
				actuallyClosing = false;
				deactivateFromViewModel = false;
			}

			void PreviewClosed(object sender, WindowPreviewClosedEventArgs e)
			{
				if (e.Cancel.HasValue && e.Cancel.Value)
					return;

				var guard = (IGuardClose)model;

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
						else e.Cancel = !canClose;

						shouldEnd = true;
					});
				});

				if (shouldEnd)
					return;
				
				e.Cancel = true;
				runningAsync = true;
			}
		}
	}
}