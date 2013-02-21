using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Caliburn.Micro;
using Caliburn.Micro.Telerik;
using Telerik.Windows.Controls;

namespace Examples.Conventions
{
	public class AppBootstrapper : Bootstrapper<IShell>
	{
		private CompositionContainer container;

		protected override void Configure()
		{
			container = new CompositionContainer(
				new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)))
				);

			var batch = new CompositionBatch();

			batch.AddExportedValue<IWindowManager>(new TelerikWindowManager());
			batch.AddExportedValue<IEventAggregator>(new EventAggregator());
			batch.AddExportedValue(container);

			// This is essential to enable Telerik's conventions
			TelerikConventions.Install();

			StyleManager.ApplicationTheme = ThemeManager.FromName("Windows8");

			container.Compose(batch);
		}

		protected override object GetInstance(Type serviceType, string key)
		{
			string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
			var exports = container.GetExportedValues<object>(contract);

			if (exports.Any())
				return exports.First();

			throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
		}

		protected override void BuildUp(object instance)
		{
			container.SatisfyImportsOnce(instance);
		}
	}
}