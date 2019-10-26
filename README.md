# ConsoleGUI

ConsoleGUI is a simple C# framework for creating console based GUI applications.

It provides most essential layout management utilities as well as a set of basic controls.

<p align="center">
  <img src="https://github.com/TomaszRewak/C-sharp-console-gui-framework/blob/master/Resources/example.png?raw=true" width=800/>
</p>

# Setup

First install the Nuget packege:

```dotnet add package ConsoleGUI```

Then include required namespaces in your code:

```
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Space;
```

And finaly setup the `ConsoleManager`:

```
// optional: adjusts the buffer size and sets the output encoding to the UTF8
ConsoleManager.Setup();

// optional: resizes the console window (the size is set in number of characters, not pixels)
ConsoleManager.Resize(new Size(150, 40));

// sets the main layout element and prints it on the screen
ConsoleManager.Content = new TextBlock { Text = "Hello world" };
```
