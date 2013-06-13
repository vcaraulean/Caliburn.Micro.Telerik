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
					if (!ConventionManager.SetBindingWithoutBindingOrValueOverwrite(viewModelType,
					                                                                path,
					                                                                property,
					                                                                element,
					                                                                convention,
					                                                                RadTabControl.ItemsSourceProperty))
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
						                                      RadTabControl.ItemTemplateSelectorProperty,
						                                      viewModelType);
					return true;
				};

			ConventionManager.AddElementConvention<RadMenuItem>(RadMenuItem.ItemsSourceProperty, "DataContext", "Click");
			ConventionManager.AddElementConvention<RadBusyIndicator>(RadBusyIndicator.IsBusyProperty, "IsBusy", "Loaded");
			ConventionManager.AddElementConvention<RadMaskedTextBox>(RadMaskedTextBox.MaskedTextProperty, "MaskedText", "ValueChanged");
			ConventionManager.AddElementConvention<RadMaskedTextInput>(RadMaskedTextInput.ValueProperty, "Value", "ValueChanged");
			ConventionManager.AddElementConvention<RadMaskedCurrencyInput>(RadMaskedCurrencyInput.ValueProperty, "Value", "ValueChanged");
			ConventionManager.AddElementConvention<RadMaskedDateTimeInput>(RadMaskedDateTimeInput.ValueProperty, "Value", "ValueChanged");
			ConventionManager.AddElementConvention<RadMaskedNumericInput>(RadMaskedNumericInput.ValueProperty, "Value", "ValueChanged");

			// Affects: RadDateTimePicker, RadTimePicker, RadDatePicker
			ConventionManager.AddElementConvention<RadDateTimePicker>(RadDateTimePicker.SelectedValueProperty, "SelectedValue",
			                                                          "SelectionChanged");

			// Affects: RadGridView, RadTreeListView
			ConventionManager.AddElementConvention<DataControl>(DataControl.ItemsSourceProperty, "SelectedItem",
			                                                    "SelectionChanged")
				.ApplyBinding = (viewModelType, path, property, element, convention) =>
				{
					if (!ConventionManager.SetBindingWithoutBindingOrValueOverwrite(viewModelType,
					                                                                path,
					                                                                property,
					                                                                element,
					                                                                convention,
					                                                                DataControl.ItemsSourceProperty))
						return false;

					if (ConventionManager.HasBinding(element, DataControl.SelectedItemProperty)) return true;
					var index = path.LastIndexOf('.');
					index = index == -1 ? 0 : index + 1;
					var baseName = path.Substring(index);
					foreach (var selectionPath in
						from potentialName in ConventionManager.DerivePotentialSelectionNames(baseName)
						where
							viewModelType.GetProperty(potentialName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) !=
							null
						select path.Replace(baseName, potentialName))
					{
						var binding = new Binding(selectionPath) {Mode = BindingMode.TwoWay};
						BindingOperations.SetBinding(element, DataControl.SelectedItemProperty, binding);
					}
					return true;
				};
		}
	}
}