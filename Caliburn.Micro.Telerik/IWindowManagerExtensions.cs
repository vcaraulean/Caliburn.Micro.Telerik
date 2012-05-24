using System;
using System.Collections.Generic;
using Telerik.Windows.Controls;

namespace Caliburn.Micro
{
	public static class IWindowManagerExtensions
	{
		public static void Show(this IWindowManager windowManager, object rootModel, object context = null, IDictionary<string, object> settings = null)
		{
			var wm = windowManager as TelerikWindowManager;
			if (wm == null)
				throw new InvalidOperationException("Expected 'Caliburn.Micro.TelerikWindowManager'.");

			wm.Show(rootModel, context, settings);
		}

		public static void Alert(this IWindowManager windowManager, string title, string message)
		{
			TelerikWindowManager.Alert(title, message);
		}

		public static void Alert(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			TelerikWindowManager.Alert(dialogParameters);
		}

		public static void Confirm(this IWindowManager windowManager, string title, string message, System.Action onOK, System.Action onCancel = null)
		{
			TelerikWindowManager.Confirm(title, message, onOK, onCancel);
		}

		public static void Confirm(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			TelerikWindowManager.Confirm(dialogParameters);
		}

		public static void Prompt(this IWindowManager windowManager, string title, string message, string defaultPromptResultValue, Action<string> onOK)
		{
			TelerikWindowManager.Prompt(title, message, defaultPromptResultValue, onOK);
		}

		public static void Prompt(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			TelerikWindowManager.Prompt(dialogParameters);
		}
	}
}