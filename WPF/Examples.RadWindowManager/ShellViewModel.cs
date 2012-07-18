using System.ComponentModel.Composition;

namespace Examples.RadWindowManager {
	[Export(typeof(IShell))]
    public class ShellViewModel : IShell {}
}