using System;
using System.Collections.Generic;
using Telerik.Windows.Controls;
using RadWindowManager = Caliburn.Micro.Telerik.RadWindowManager;

namespace Caliburn.Micro
{
	public static class IWindowManagerExtensions
	{
		public static void ShowDialog(this IWindowManager windowManager, object rootModel, object context = null, IDictionary<string, object> settings = null)
		{
			var wm = windowManager as RadWindowManager;
			if (wm == null)
				throw new InvalidOperationException("Expected 'Caliburn.Micro.Telerik.RadWindowManager'.");

			wm.ShowDialog(rootModel, context, settings);
		}

		public static void Alert(this IWindowManager windowManager, string title, string message)
		{
			RadWindowManager.Alert(title, message);
		}

		public static void Alert(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			RadWindowManager.Alert(dialogParameters);
		}

		public static void Confirm(this IWindowManager windowManager, string title, string message, Action<bool> dialogResult)
		{
			RadWindowManager.Confirm(title, message, dialogResult);
		}

		public static void Confirm(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			RadWindowManager.Confirm(dialogParameters);
		}

		public static void Prompt(this IWindowManager windowManager, string title, string message, string defaultPromptResultValue, Action<bool, string> result)
		{
			RadWindowManager.Prompt(title, message, defaultPromptResultValue, result);
		}

		public static void Prompt(this IWindowManager windowManager, DialogParameters dialogParameters)
		{
			RadWindowManager.Prompt(dialogParameters);
		}
	}
}