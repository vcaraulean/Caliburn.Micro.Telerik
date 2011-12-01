using System.Linq;
using System.Reflection;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace Caliburn.Micro.Telerik
{
	public static class TelerikConventions
	{
		public static void Install()
		{
			ConventionManager.AddElementConvention<RadTabControl>(RadTabControl.ItemsSourceProperty,
			                                                      "ItemsSource",
			                                                      "SelectionChanged")
				.ApplyBinding = (viewModelType, path, property, element, convention) =>
				{
					if (!ConventionManager.SetBinding(viewModelType, path, property, element, convention))
						return false;

					var tabControl = (RadTabControl) element;
					if (tabControl.ContentTemplate == null
					    && tabControl.ContentTemplateSelector == null
					    && property.PropertyType.IsGenericType)
					{
						var itemType = property.PropertyType.GetGenericArguments().First();
						if (!itemType.IsValueType && !typeof (string).IsAssignableFrom(itemType))
							tabControl.ContentTemplate = ConventionManager.DefaultItemTemplate;
					}
					ConventionManager.ConfigureSelectedItem(element,
					                                        RadTabControl.SelectedItemProperty,
					                                        viewModelType,
					                                        path);

					if (string.IsNullOrEmpty(tabControl.DisplayMemberPath))
						ConventionManager.ApplyHeaderTemplate(tabControl,
						                                      RadTabControl.ItemTemplateProperty,
						                                      viewModelType);
					return true;
				};

			ConventionManager.AddElementConvention<RadMenuItem>(RadMenuItem.ItemsSourceProperty, "DataContext", "Click");
			ConventionManager.AddElementConvention<RadBusyIndicator>(RadBusyIndicator.IsBusyProperty, "IsBusy", "Loaded");
			ConventionManager.AddElementConvention<RadMaskedTextBox>(RadMaskedTextBox.MaskedTextProperty, "MaskedText",
			                                                         "ValueChanged");
			ConventionManager.AddElementConvention<RadMaskedTextInput>(RadMaskedTextInput.ValueProperty, "Value", "ValueChanged");

			// Works also for RadTimePicker, RadDatePicker
			ConventionManager.AddElementConvention<RadDateTimePicker>(RadDateTimePicker.SelectedValueProperty, "SelectedValue",
			                                                          "SelectionChanged");

			ConventionManager.AddElementConvention<RadGridView>(RadGridView.ItemsSourceProperty, "SelectedItem", "SelectionChanged")
				.ApplyBinding = (viewModelType, path, property, element, convention) =>
				{
					if (!ConventionManager.SetBinding(viewModelType, path, property, element, convention)) return false;

					if (ConventionManager.HasBinding(element, RadGridView.SelectedItemProperty)) return false;

					var index = path.LastIndexOf('.');
					index = index == -1 ? 0 : index + 1;
					var baseName = path.Substring(index);

					foreach (var potentialName in ConventionManager.DerivePotentialSelectionNames(baseName))
					{
						var propertyInfo = viewModelType.GetProperty(potentialName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
						if (propertyInfo == null) 
							continue;
						
						var selectionPath = path.Replace(baseName, potentialName);
						var binding = new Binding(selectionPath) {Mode = BindingMode.TwoWay};
						BindingOperations.SetBinding(element, RadGridView.SelectedItemProperty, binding);
						break;
					}
					return true;
				};
		}
	}
}