using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Caliburn.Micro.Telerik;
using Telerik.Windows.Controls;

namespace Caliburn.Micro
{
	/// <summary>
	/// A service that manages windows.
	/// Implementation for <see cref="RadWindow"/>
	/// </summary>
	public class TelerikWindowManager : IWindowManager
	{
		/// <summary>
		/// Shows a modal dialog for the specified model.
		/// 
		/// By default RadWindow dialog is shown at the center of the screen
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <param name="context">The context.</param>
		/// <param name="settings">The optional RadWindow settings.</param>
		public virtual void ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null)
		{
			var view = PrepareRadWindow(rootModel, context, settings);

			view.ShowDialog();
		}

		/// <summary>
		/// Shows a normal, non-modal dialog for the specified model.
		/// 
		/// By default RadWindow dialog is shown at the center of the screen
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <param name="context">The context.</param>
		/// <param name="settings">The optional RadWindow settings.</param>
		public virtual void Show(object rootModel, object context = null, IDictionary<string, object> settings = null)
		{
			var view = PrepareRadWindow(rootModel, context, settings);

			view.Show();
		}

		private RadWindow PrepareRadWindow(object rootModel, object context, IDictionary<string, object> settings)
		{
			var view = EnsureWindow(rootModel, ViewLocator.LocateForModel(rootModel, null, context));
			ViewModelBinder.Bind(rootModel, view, context);

			var haveDisplayName = rootModel as IHaveDisplayName;
			if (haveDisplayName != null && !ConventionManager.HasBinding(view, RadWindow.HeaderProperty))
			{
				var binding = new Binding("DisplayName") {Mode = BindingMode.TwoWay};
				view.SetBinding(RadWindow.HeaderProperty, binding);
			}

			new RadWindowConductor(rootModel, view);

			ApplySettings(view, settings);
			return view;
		}

		/// <summary>
		/// Shows a toast notification for the specified model.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <param name="durationInMilliseconds">How long the notification should appear for.</param>
		/// <param name="context">The context.</param>
		/// <param name="settings">The optional RadWindow Settings</param>
		public void ShowNotification(object rootModel, int durationInMilliseconds, object context = null, IDictionary<string, object> settings = null)
		{
			var window = new NotificationWindow();
			var view = ViewLocator.LocateForModel(rootModel, window, context);

			ViewModelBinder.Bind(rootModel, view, null);
			window.Content = (FrameworkElement)view;

			ApplySettings(window, settings);

			var activator = rootModel as IActivate;
			if (activator != null)
				activator.Activate();

			var deactivator = rootModel as IDeactivate;
			if (deactivator != null)
				window.Closed += delegate { deactivator.Deactivate(true); };

			window.Show(durationInMilliseconds);
		}

		/// <summary>
		/// Shows a popup at the current mouse position.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <param name="context">The view context.</param>
		/// <param name="settings">The optional popup settings.</param>
		public virtual void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
		{
			var popup = CreatePopup(rootModel, settings);
			var view = ViewLocator.LocateForModel(rootModel, popup, context);

			popup.Child = view;
			popup.SetValue(View.IsGeneratedProperty, true);

			ViewModelBinder.Bind(rootModel, popup, null);

			var activatable = rootModel as IActivate;
			if (activatable != null)
				activatable.Activate();

			var deactivator = rootModel as IDeactivate;
			if (deactivator != null)
				popup.Closed += delegate { deactivator.Deactivate(true); };

			popup.IsOpen = true;
			popup.CaptureMouse();
		}

		/// <summary>
		/// Creates a popup for hosting a popup window.
		/// </summary>
		/// <param name="rootModel">The model.</param>
		/// <param name="settings">The optional popup settings.</param>
		/// <returns>The popup.</returns>
		protected virtual Popup CreatePopup(object rootModel, IDictionary<string, object> settings)
		{
			var popup = new Popup
			{
				HorizontalOffset = Mouse.Position.X,
				VerticalOffset = Mouse.Position.Y
			};

			if (settings != null)
			{
				var type = popup.GetType();

				foreach (var pair in settings)
				{
					var propertyInfo = type.GetProperty(pair.Key);

					if (propertyInfo != null)
						propertyInfo.SetValue(popup, pair.Value, null);
				}
			}

			return popup;
		}


		bool ApplySettings(object target, IEnumerable<KeyValuePair<string, object>> settings)
		{
			if (settings != null)
			{
				var type = target.GetType();

				foreach (var pair in settings)
				{
					var propertyInfo = type.GetProperty(pair.Key);

					if (propertyInfo != null)
						propertyInfo.SetValue(target, pair.Value, null);
				}

				return true;
			}

			return false;
		}

		private static RadWindow EnsureWindow(object model, object view)
		{
			var window = view as RadWindow;

			if (window == null)
			{
				window = new RadWindow
				{
					Content = view,
					WindowStartupLocation = WindowStartupLocation.CenterScreen
				};
				window.SetValue(View.IsGeneratedProperty, true);
			}

			return window;
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
				
				if(onCancel != null)
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