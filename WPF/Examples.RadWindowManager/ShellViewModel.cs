using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Examples.RadWindowManager {
	[Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell {}
}