using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Navigation;
using Caliburn.Micro.Telerik;
using Telerik.Windows.Controls;
using PlacementMode = System.Windows.Controls.Primitives.PlacementMode;

namespace Caliburn.Micro
{
	///// <summary>
	///// A service that manages windows.
	///// Implementation for <see cref="RadWindow"/>
	///// </summary>
	//public class TelerikWindowManager : IWindowManager
	//{
	//    /// <summary>
	//    /// Shows a modal dialog for the specified model.
	//    /// </summary>
	//    /// <param name="rootModel">The root model.</param>
	//    /// <param name="context">The context.</param>
	//    /// <param name="settings">The dialog popup settings.</param>
	//    /// <returns>The dialog result.</returns>
	//    public virtual bool? ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null)
	//    {
	//        var radWindow = CreateWindow(rootModel, true, context, settings);
	//        radWindow.ShowDialog();
	//        return radWindow.DialogResult;
	//    }

	//    /// <summary>
	//    /// Shows a window for the specified model.
	//    /// </summary>
	//    /// <param name="rootModel">The root model.</param>
	//    /// <param name="context">The context.</param>
	//    /// <param name="settings">The optional window settings.</param>
	//    public virtual void ShowWindow(object rootModel, object context = null, IDictionary<string, object> settings = null)
	//    {
	//        NavigationWindow navWindow = null;

	//        if (Application.Current != null && Application.Current.MainWindow != null)
	//        {
	//            navWindow = Application.Current.MainWindow as NavigationWindow;
	//        }

	//        if (navWindow != null)
	//        {
	//            var window = CreatePage(rootModel, context, settings);
	//            navWindow.Navigate(window);
	//        }
	//        else
	//        {
	//            CreateWindow(rootModel, false, context, settings).Show();
	//        }
	//    }

	//    /// <summary>
	//    /// Shows a popup at the current mouse position.
	//    /// </summary>
	//    /// <param name="rootModel">The root model.</param>
	//    /// <param name="context">The view context.</param>
	//    /// <param name="settings">The optional popup settings.</param>
	//    public virtual void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
	//    {
	//        var popup = CreatePopup(rootModel, settings);
	//        var view = ViewLocator.LocateForModel(rootModel, popup, context);

	//        popup.Child = view;
	//        popup.SetValue(View.IsGeneratedProperty, true);

	//        ViewModelBinder.Bind(rootModel, popup, null);
	//        Action.SetTargetWithoutContext(view, rootModel);

	//        var activatable = rootModel as IActivate;
	//        if (activatable != null)
	//        {
	//            activatable.Activate();
	//        }

	//        var deactivator = rootModel as IDeactivate;
	//        if (deactivator != null)
	//        {
	//            popup.Closed += delegate { deactivator.Deactivate(true); };
	//        }

	//        popup.IsOpen = true;
	//        popup.CaptureMouse();
	//    }

	//    /// <summary>
	//    /// Creates a popup for hosting a popup window.
	//    /// </summary>
	//    /// <param name="rootModel">The model.</param>
	//    /// <param name="settings">The optional popup settings.</param>
	//    /// <returns>The popup.</returns>
	//    protected virtual Popup CreatePopup(object rootModel, IDictionary<string, object> settings)
	//    {
	//        var popup = new Popup();

	//        if (ApplySettings(popup, settings))
	//        {
	//            if (!settings.ContainsKey("PlacementTarget") && !settings.ContainsKey("Placement"))
	//                popup.Placement = PlacementMode.MousePoint;
	//            if (!settings.ContainsKey("AllowsTransparency"))
	//                popup.AllowsTransparency = true;
	//        }
	//        else
	//        {
	//            popup.AllowsTransparency = true;
	//            popup.Placement = PlacementMode.MousePoint;
	//        }

	//        return popup;
	//    }

	//    /// <summary>
	//    /// Creates a window.
	//    /// </summary>
	//    /// <param name="rootModel">The view model.</param>
	//    /// <param name="isDialog">Whethor or not the window is being shown as a dialog.</param>
	//    /// <param name="context">The view context.</param>
	//    /// <param name="settings">The optional popup settings.</param>
	//    /// <returns>The window.</returns>
	//    protected virtual RadWindow CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
	//    {
	//        var view = EnsureWindow(rootModel, ViewLocator.LocateForModel(rootModel, null, context), isDialog);
	//        ViewModelBinder.Bind(rootModel, view, context);

	//        var haveDisplayName = rootModel as IHaveDisplayName;
	//        if (haveDisplayName != null && !ConventionManager.HasBinding(view, RadWindow.HeaderProperty))
	//        {
	//            var binding = new Binding("DisplayName") { Mode = BindingMode.TwoWay };
	//            view.SetBinding(RadWindow.HeaderProperty, binding);
	//        }

	//        ApplySettings(view, settings);

	//        new RadWindowConductor(rootModel, view);

	//        return view;
	//    }

	//    /// <summary>
	//    /// Makes sure the view is a window is is wrapped by one.
	//    /// </summary>
	//    /// <param name="model">The view model.</param>
	//    /// <param name="view">The view.</param>
	//    /// <param name="isDialog">Whethor or not the window is being shown as a dialog.</param>
	//    /// <returns>The window.</returns>
	//    protected virtual RadWindow EnsureWindow(object model, object view, bool isDialog)
	//    {
	//        var window = view as RadWindow;

	//        if (window == null)
	//        {
	//            window = new RadWindow
	//            {
	//                Content = view,
	//                SizeToContent = true,
	//            };

	//            window.SetValue(View.IsGeneratedProperty, true);

	//            var owner = GetActiveWindow();
	//            if (owner != null)
	//            {
	//                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
	//                window.Owner = owner;
	//            }
	//            else
	//            {
	//                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
	//            }
	//        }
	//        else
	//        {
	//            var owner = GetActiveWindow();
	//            if (owner != null && isDialog)
	//            {
	//                window.Owner = owner;
	//            }
	//        }

	//        return window;
	//    }

	//    /// <summary>
	//    /// Infers the owner of the window.
	//    /// </summary>
	//    /// <returns>The owner.</returns>
	//    protected virtual Window GetActiveWindow()
	//    {
	//        if (Application.Current == null)
	//        {
	//            return null;
	//        }

	//        var active = Application.Current
	//            .Windows.OfType<Window>()
	//            .FirstOrDefault(x => x.IsActive);

	//        return active ?? Application.Current.MainWindow;
	//    }

	//    /// <summary>
	//    /// Creates the page.
	//    /// </summary>
	//    /// <param name="rootModel">The root model.</param>
	//    /// <param name="context">The context.</param>
	//    /// <param name="settings">The optional popup settings.</param>
	//    /// <returns>The page.</returns>
	//    public virtual Page CreatePage(object rootModel, object context, IDictionary<string, object> settings)
	//    {
	//        var view = EnsurePage(rootModel, ViewLocator.LocateForModel(rootModel, null, context));
	//        ViewModelBinder.Bind(rootModel, view, context);

	//        var haveDisplayName = rootModel as IHaveDisplayName;
	//        if (haveDisplayName != null && !ConventionManager.HasBinding(view, Page.TitleProperty))
	//        {
	//            var binding = new Binding("DisplayName") { Mode = BindingMode.TwoWay };
	//            view.SetBinding(Page.TitleProperty, binding);
	//        }

	//        ApplySettings(view, settings);

	//        var activatable = rootModel as IActivate;
	//        if (activatable != null)
	//        {
	//            activatable.Activate();
	//        }

	//        var deactivatable = rootModel as IDeactivate;
	//        if (deactivatable != null)
	//        {
	//            view.Unloaded += (s, e) => deactivatable.Deactivate(true);
	//        }

	//        return view;
	//    }

	//    /// <summary>
	//    /// Ensures the view is a page or provides one.
	//    /// </summary>
	//    /// <param name="model">The model.</param>
	//    /// <param name="view">The view.</param>
	//    /// <returns>The page.</returns>
	//    protected virtual Page EnsurePage(object model, object view)
	//    {
	//        var page = view as Page;

	//        if (page == null)
	//        {
	//            page = new Page { Content = view };
	//            page.SetValue(View.IsGeneratedProperty, true);
	//        }

	//        return page;
	//    }

	//    bool ApplySettings(object target, IEnumerable<KeyValuePair<string, object>> settings)
	//    {
	//        if (settings != null)
	//        {
	//            var type = target.GetType();

	//            foreach (var pair in settings)
	//            {
	//                var propertyInfo = type.GetProperty(pair.Key);

	//                if (propertyInfo != null)
	//                {
	//                    propertyInfo.SetValue(target, pair.Value, null);
	//                }
	//            }

	//            return true;
	//        }

	//        return false;
	//    }

	//    public static void Alert(string title, string message)
	//    {
	//        Alert(new DialogParameters { Header = title, Content = message });
	//    }

	//    public static void Alert(DialogParameters dialogParameters)
	//    {
	//        RadWindow.Alert(dialogParameters);
	//    }

	//    public static void Confirm(string title, string message, System.Action onOK, System.Action onCancel = null)
	//    {
	//        var dialogParameters = new DialogParameters
	//        {
	//            Header = title,
	//            Content = message
	//        };
	//        dialogParameters.Closed += (sender, args) =>
	//        {
	//            var result = args.DialogResult;
	//            if (result.HasValue && result.Value)
	//            {
	//                onOK();
	//                return;
	//            }
				
	//            if(onCancel != null)
	//                onCancel();
	//        };
	//        Confirm(dialogParameters);
	//    }

	//    public static void Confirm(DialogParameters dialogParameters)
	//    {
	//        RadWindow.Confirm(dialogParameters);
	//    }

	//    public static void Prompt(string title, string message, string defaultPromptResultValue, Action<string> onOK)
	//    {
	//        var dialogParameters = new DialogParameters
	//        {
	//            Header = title,
	//            Content = message,
	//            DefaultPromptResultValue = defaultPromptResultValue,
	//        };
	//        dialogParameters.Closed += (o, args) =>
	//        {
	//            if (args.DialogResult.HasValue && args.DialogResult.Value)
	//                onOK(args.PromptResult);
	//        };
			
	//        Prompt(dialogParameters);
	//    }

	//    public static void Prompt(DialogParameters dialogParameters)
	//    {
	//        RadWindow.Prompt(dialogParameters);
	//    }
	//}
}