using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using Caliburn.Micro.Telerik;
using Telerik.Windows.Controls;

namespace Caliburn.Micro
{
	public class TelerikWindowManager : WindowManager
	{
		public override bool? ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null)
		{
			var viewType = ViewLocator.LocateTypeForModelType(rootModel.GetType(), null, null);
			if (typeof(RadWindow).IsAssignableFrom(viewType)
				|| typeof(UserControl).IsAssignableFrom(viewType))
			{
				var radWindow = CreateRadWindow(rootModel, true, context, settings);
				radWindow.ShowDialog();
				return radWindow.DialogResult;
			}

			return base.ShowDialog(rootModel, context, settings);
		}

		public override void ShowWindow(object rootModel, object context = null, IDictionary<string, object> settings = null)
		{
			var viewType = ViewLocator.LocateTypeForModelType(rootModel.GetType(), null, null);
			if (typeof(RadWindow).IsAssignableFrom(viewType)
				|| typeof(UserControl).IsAssignableFrom(viewType))
			{
				NavigationWindow navWindow = null;

				if (Application.Current != null && Application.Current.MainWindow != null)
				{
					navWindow = Application.Current.MainWindow as NavigationWindow;
				}

				if (navWindow != null)
				{
					var window = CreatePage(rootModel, context, settings);
					navWindow.Navigate(window);
				}
				else
				{
					CreateRadWindow(rootModel, false, context, settings).Show();
				}
				return;
			}
			base.ShowWindow(rootModel, context, settings);
		}


		/// <summary>
		/// Creates a window.
		/// </summary>
		/// <param name="rootModel">The view model.</param>
		/// <param name="isDialog">Whethor or not the window is being shown as a dialog.</param>
		/// <param name="context">The view context.</param>
		/// <param name="settings">The optional popup settings.</param>
		/// <returns>The window.</returns>
		protected virtual RadWindow CreateRadWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
		{
			var view = EnsureRadWindow(rootModel, ViewLocator.LocateForModel(rootModel, null, context), isDialog);
			ViewModelBinder.Bind(rootModel, view, context);

			var haveDisplayName = rootModel as IHaveDisplayName;
			if (haveDisplayName != null && !ConventionManager.HasBinding(view, RadWindow.HeaderProperty))
			{
				var binding = new Binding("DisplayName") { Mode = BindingMode.TwoWay };
				view.SetBinding(RadWindow.HeaderProperty, binding);
			}

			ApplyRadWindowSettings(view, settings);

			new RadWindowConductor(rootModel, view);

			return view;
		}
		
		bool ApplyRadWindowSettings(object target, IEnumerable<KeyValuePair<string, object>> settings)
		{
			if (settings != null)
			{
				var type = target.GetType();

				foreach (var pair in settings)
				{
					var propertyInfo = type.GetProperty(pair.Key);

					if (propertyInfo != null)
					{
						propertyInfo.SetValue(target, pair.Value, null);
					}
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Makes sure the view is a window is is wrapped by one.
		/// </summary>
		/// <param name="model">The view model.</param>
		/// <param name="view">The view.</param>
		/// <param name="isDialog">Whethor or not the window is being shown as a dialog.</param>
		/// <returns>The window.</returns>
		protected virtual RadWindow EnsureRadWindow(object model, object view, bool isDialog)
		{
			var window = view as RadWindow;

			if (window == null)
			{
				var contentElement = view as FrameworkElement;
				if (contentElement == null)
					throw new ArgumentNullException("view");

				window = new RadWindow
				{
					Content = view,
					SizeToContent = true,
				};

				AdjustWindowAndContentSize(window, contentElement);

				window.SetValue(View.IsGeneratedProperty, true);

				var owner = GetActiveWindow();
				if (owner != null)
				{
					window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
					window.Owner = owner;
				}
				else
				{
					window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
			}
			else
			{
				var owner = GetActiveWindow();
				if (owner != null && isDialog)
				{
					window.Owner = owner;
				}
			}

			return window;
		}

		/// <summary>
		/// Initializes Window size with values extracted by the view.
		/// 
		/// Note:
		///	The real size of the content will be smaller than provided values.
		///	The form has the header (title) and border so they will take place.
		/// 
		/// </summary>
		/// <param name="window">The RadWindow</param>
		/// <param name="view">The view</param>
		private static void AdjustWindowAndContentSize(RadWindow window, FrameworkElement view)
		{
			window.MinWidth = view.MinWidth;
			window.MaxWidth = view.MaxWidth;
			window.Width = view.Width;
			window.MinHeight = view.MinHeight;
			window.MaxHeight = view.MaxHeight;
			window.Height = view.Height;
			
			// Resetting view's settings
			view.Width = view.Height = Double.NaN;
			view.MinWidth = view.MinHeight = 0;
			view.MaxWidth = view.MaxHeight = int.MaxValue;

			// Stretching content to the Window
			view.VerticalAlignment = VerticalAlignment.Stretch;
			view.HorizontalAlignment = HorizontalAlignment.Stretch;
		}

		/// <summary>
		/// Infers the owner of the window.
		/// </summary>
		/// <returns>The owner.</returns>
		protected virtual Window GetActiveWindow()
		{
			if (Application.Current == null)
			{
				return null;
			}

			var active = Application.Current
				.Windows.OfType<Window>()
				.FirstOrDefault(x => x.IsActive);

			return active ?? Application.Current.MainWindow;
		}

		public static void Alert(string title, string message)
		{
			Alert(new DialogParameters { Header = title, Content = message });
		}

		public static void Alert(DialogParameters dialogParameters)
		{
			RadWindow.Alert(dialogParameters);
		}

		public static void Confirm(string title, string message, System.Action onOK, System.Action onCancel = null)
		{
			var dialogParameters = new DialogParameters
			{
				Header = title,
				Content = message
			};
			dialogParameters.Closed += (sender, args) =>
			{
				var result = args.DialogResult;
				if (result.HasValue && result.Value)
				{
					onOK();
					return;
				}

				if (onCancel != null)
					onCancel();
			};
			Confirm(dialogParameters);
		}

		public static void Confirm(DialogParameters dialogParameters)
		{
			RadWindow.Confirm(dialogParameters);
		}

		public static void Prompt(string title, string message, string defaultPromptResultValue, Action<string> onOK)
		{
			var dialogParameters = new DialogParameters
			{
				Header = title,
				Content = message,
				DefaultPromptResultValue = defaultPromptResultValue,
			};
			dialogParameters.Closed += (o, args) =>
			{
				if (args.DialogResult.HasValue && args.DialogResult.Value)
					onOK(args.PromptResult);
			};

			Prompt(dialogParameters);
		}

		public static void Prompt(DialogParameters dialogParameters)
		{
			RadWindow.Prompt(dialogParameters);
		}

	}
}