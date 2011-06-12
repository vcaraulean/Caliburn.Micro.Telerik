using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation;

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
			var settings = new Dictionary<string, object>();
			settings["WindowStartupLocation"] = WindowStartupLocation.CenterScreen;

			ShowDialog(rootModel, context, settings);
		}

		/// <summary>
		/// Shows a modal dialog for the specified model.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <param name="context">The context.</param>
		/// <param name="settings">The optional RadWindow settings.</param>
		public virtual void ShowDialog(object rootModel, object context, IDictionary<string, object> settings = null)
		{
			var view = EnsureWindow(rootModel, ViewLocator.LocateForModel(rootModel, null, context));
			ApplySettings(view, settings);
			ViewModelBinder.Bind(rootModel, view, context);

			var haveDisplayName = rootModel as IHaveDisplayName;
			if (haveDisplayName != null && !ConventionManager.HasBinding(view, RadWindow.HeaderProperty))
			{
				var binding = new Binding("DisplayName") { Mode = BindingMode.TwoWay };
				view.SetBinding(RadWindow.HeaderProperty, binding);
			}

			new RadWindowConductor(rootModel, view);

			view.ShowDialog();
		}

		/// <summary>
		/// Shows a toast notification for the specified model.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <param name="durationInMilliseconds">How long the notification should appear for.</param>
		/// <param name="context">The context.</param>
		public void ShowNotification(object rootModel, int durationInMilliseconds, object context)
		{
			var window = new NotificationWindow();
			var view = ViewLocator.LocateForModel(rootModel, window, context);

			ViewModelBinder.Bind(rootModel, view, null);
			window.Content = (FrameworkElement)view;

			var activator = rootModel as IActivate;
			if (activator != null)
				activator.Activate();

			var deactivator = rootModel as IDeactivate;
			if (deactivator != null)
				window.Closed += delegate { deactivator.Deactivate(true); };

			window.Show(durationInMilliseconds);
		}

		public void ShowPopup(object rootModel, object context, IDictionary<string, object> settings)
		{
			throw new NotImplementedException();
		}
		
		private static void ApplySettings(RadWindow radWindow, IDictionary<string, object> settings)
		{
			if (settings == null)
				return;

			var type = radWindow.GetType();

			foreach (var pair in settings)
			{
				var propertyInfo = type.GetProperty(pair.Key);

				if (propertyInfo != null)
					propertyInfo.SetValue(radWindow, pair.Value, null);
			}
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

		public static void Alert(string title, string message)
		{
			Alert(new DialogParameters { Header = title, Content = message });
		}

		public static void Alert(DialogParameters dialogParameters)
		{
			RadWindow.Alert(dialogParameters);
		}

		public static void Confirm(string title, string message, Action<bool?> dialogResult)
		{
			var dialogParameters = new DialogParameters
			{
				Header = title,
				Content = message,
			};
			dialogParameters.Closed += (o, eventArgs) => dialogResult(eventArgs.DialogResult);

			Confirm(dialogParameters);
		}

		public static void Confirm(DialogParameters dialogParameters)
		{
			RadWindow.Confirm(dialogParameters);
		}

		public static void Prompt(string title, string message, string defaultPromptResultValue, Action<bool?, string> result)
		{
			var dialogParameters = new DialogParameters
			{
				Header = title,
				Content = message,
				DefaultPromptResultValue = defaultPromptResultValue,
			};
			dialogParameters.Closed += (o, eventArgs) => result(eventArgs.DialogResult, eventArgs.PromptResult);
			
			Prompt(dialogParameters);
		}

		public static void Prompt(DialogParameters dialogParameters)
		{
			RadWindow.Prompt(dialogParameters);
		}
	}
}