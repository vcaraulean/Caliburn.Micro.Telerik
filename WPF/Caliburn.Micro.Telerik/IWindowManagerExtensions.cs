using System;
using System.Collections.Generic;
using Telerik.Windows.Controls;

namespace Caliburn.Micro
{
	public static class IWindowManagerExtensions
	{
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