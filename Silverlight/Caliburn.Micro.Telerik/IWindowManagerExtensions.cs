using System;
using System.Collections.Generic;
using Telerik.Windows.Controls;

namespace Caliburn.Micro
{
	public static class IWindowManagerExtensions
	{
		/// <summary>
		/// Shows a normal, non-modal dialog for the specified model.
		/// 
		/// By default RadWindow dialog is shown at the center of the screen
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <param name="context">The context.</param>
		/// <param name="settings">The optional RadWindow settings.</param>
		public static void Show(this IWindowManager windowManager, object rootModel, object context = null, IDictionary<string, object> settings = null)
		{
			var wm = windowManager as TelerikWindowManager;
			if (wm == null)
				throw new InvalidOperationException("Expected 'Caliburn.Micro.TelerikWindowManager'.");

			wm.Show(rootModel, context, settings);
		}

		/// <summary>
		/// Opens an Alert modal window
		/// </summary>
		public static void Alert(this IWindowManager windowManager, string title, string message)
		{
			TelerikWindowManager.Alert(title, message);
		}

		/// <summary>
		/// Opens an Alert modal window
		/// </summary>
		public static void Alert(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			TelerikWindowManager.Alert(dialogParameters);
		}

		/// <summary>
		/// Opens a Confirm modal window
		/// </summary>
		public static void Confirm(this IWindowManager windowManager, string title, string message, System.Action onOK, System.Action onCancel = null)
		{
			TelerikWindowManager.Confirm(title, message, onOK, onCancel);
		}

		/// <summary>
		/// Opens a Confirm modal window
		/// </summary>
		public static void Confirm(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			TelerikWindowManager.Confirm(dialogParameters);
		}

		/// <summary>
		/// Opens a Prompt modal window
		/// </summary>
		public static void Prompt(this IWindowManager windowManager, string title, string message, string defaultPromptResultValue, Action<string> onOK)
		{
			TelerikWindowManager.Prompt(title, message, defaultPromptResultValue, onOK);
		}

		/// <summary>
		/// Opens a Prompt modal window
		/// </summary>
		public static void Prompt(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			TelerikWindowManager.Prompt(dialogParameters);
		}
	}
}