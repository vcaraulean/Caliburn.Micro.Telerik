Caliburn.Micro.Telerik
==============

A library containing Caliburn.Micro conventions for Telerik's visual controls. Also contains some other stuff, like implementation of IWindowManager for RadWindow, helpers & extensions for Telerik's Prompt Dialogs.

Target platforms: Silverlight and WPF.

Contains also an Examples folder with sample projects to demo and test conventions and the rest of the stuff.

Nuget package
-------------
Is the quickest way to use the library. Check out the [Caliburn.Micro.Telerik](https://nuget.org/packages/Caliburn.Micro.Telerik) package. Includes both versions for Silverlight & WPF.

Compile the source code
----------------------
If you want to compile projects from sources, be sure to have Telerik's components installed & verify assembly references.

How to use the code
-------------------
You can either copy required classes in your solution or install Caliburn.Micro.Telerik package using nuget.

How to enable conventions
-------------------------
In your app's bootstrapper, in Configure override add next line:

```
    TelerikConventions.Install();
```
