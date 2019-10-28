# ConsoleGUI

ConsoleGUI is a simple .NET framework for creating console-based GUI applications.

It provides most essential layout management utilities as well as a set of basic controls.

<p align="center">
  <img src="https://github.com/TomaszRewak/C-sharp-console-gui-framework/blob/master/Resources/example.png?raw=true" width=800/>
</p>

#### Motivation

What sets this library apart from other projects that provide similar functionalities, is the fact that the ConsoleGUI framework is fully layout-driven. In this regard it’s more like WPF or HTML, than for example Windows Forms. You don’t specify exact coordinates at which a given control should reside, but rather let stack panels, dock panels and other layout managers do their work. I don’t claim it’s THE right way of doing things, it’s just what my background is.

## Setup

First install the NuGet package:

```powershell
dotnet add package ConsoleGUI
```

then include required namespaces in your code:

```csharp
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Space;
```

and finally setup the `ConsoleManager`:

```csharp
// optional: adjusts the buffer size and sets the output encoding to the UTF8
ConsoleManager.Setup();

// optional: resizes the console window (the size is set in a number of characters, not pixels)
ConsoleManager.Resize(new Size(150, 40));

// required for terminals that don't support true color formatting (e.g. powershell.exe)
ConsoleManager.CompatibilityMode = true;

// sets the main layout element and prints it on the screen
ConsoleManager.Content = new TextBlock { Text = "Hello world" };
```

And that's it. As you can see most of those steps are optional, depending on how you want to configure your window.

After that, whenever you make a change to any of the controls within the UI tree, the updates will be propagated and displayed automatically. No manual `Redraw()` calls are required.

#### Compatibility mode

The above example uses the compatibility mode. It's required if your terminal of choice doesn't support true color formatting. If set, the ConsoleGUI will translate all of the RGB colors into 4bit values of the ConsoleColor enum. This will, of course, degrade the user experience.

Terminals that DO NOT support the true color formatting are (for example): powershell.exe and cmd.exe.

Terminals that DO support the true color formatting are (for example): the new Windows Terminal and the terminal that is built in into the VS.

#### Responsiveness

If the window size is not set explicitly, the layout will be adjusted to the current size of that window. It's important to note that this framework doesn't detect terminal size changes automatically. If the user resizes the window manually, the layout will become broken.

To readjust the layout to the manually updated window size, call the `ConsoleManager.Setup()` method. Just remember that it might take more time to repaint the entire screen then to draw a single update (as the entire buffer must be invalidated), so do it only if the size of the window has actually changed.

## Basic controls

This is a list of all available controls:

##### Background

Sets the background color of the `Content` control. If the `Important` property is set, the background color will be updated even if the stored control already sets its own background color.

##### Border

Draws a border around the `Content` control. The `BorderPlacement` and the `BorderStyle` can be adjusted to change the look of the generated outline.

##### Boundary

Allows the user to modify the `MinWidth`, `MinHeight`, `MaxWidth` and `MaxHeight` of the `Content` control in relation to its parent control.

Especially useful to limit the space taken by controls that would otherwise stretch to fill all of the available space (like when storing a `HorizontalStackPanel` within a horizontal `DockPanel`)

##### Box

Aligns the `Content` control vertically (`Top`/`Center`/`Bottom`/`Stretch`) and horizontally (`Left`/`Center`/`Right`/`Stretch`).

##### Canvas

Can host multiple child controls, each displayed within a specified rectangle. Allows content overlapping.

##### DataGrid<T>
  
Displays `Data` in a grid based on provided column definitions.

The `ColumnDefinition` defines the column `Header`, its `Width` and the data `Selector`. The `Selector` can be used to extract a text from a data row, specify that cell's color, or even define a custom content generator.

##### DockPanel

`DockPanel` consists of two parts: `DockedControl` and `FillingControl`. The `DockedControl` is placed within the available space according to the `Placement` property value (`Top`/`Right`/`Bottom`/`Left`). The `FillingControl` takes up all of the remaining space.

##### Grid

Splits the available space into smaller pieces according to the provided `Columns` and `Rows` definitions. Each cell can store up to one child control.

##### HorizontalSeparator

Draws a horizontal line.

##### HorizontalStackPanel

Stacks multiple controls horizontally.

##### Margin

Adds the `Offset` around the `Content` control when displaying it. It affects both the `MinSize` and the `MaxSize` of the `IDrawingContext`.

##### Overlay

Allows two controls to be displayed on top of each other. Unlike the `Canvas`, it uses its own size when specifying size limits for child controls.

##### Style

Modifies the `Background` and the `Foreground` colors of its `Content`.

##### TextBlock

Displays a single line of text.

##### TextBox

An input control. Allows the user to insert a single line of text.

##### VerticalScrollPanel

Allows its `Content` to expand infinitely in the vertical dimension and displays only the part of it that is currently in view. The `ScrollBarForeground` and the `ScrollBarBackground` can be modified to adjust the look of the scroll bar.

##### VerticalSeparator

Draws a vertical line.

##### VerticalStackPanel

Stacks multiple controls vertically.

##### WrapPanel

Breaks a single line of text into multiple lines based on the available vertical space. It can be used with any type of a control (`TextBox`, `TextBlock` but also `HorizontalStackPanel` and any other).

## Creating custom controls

The set of predefined control is relatively small, but it's very easy to create custom ones. There are two main ways to do it.

#### Inheriting the `SimpleControl` class

If you want to define a control that is simply composed of other controls (like a text box with a specific background and border), inheriting from the `SimpleControl` class is the way to go.

All you have to do is to set the `protected` `Content` property with a content that you want to display.

```csharp
internal sealed class MyControl : SimpleControl
{
	private readonly TextBlock _textBlock;

	public MyControl()
	{
		_textBlock = new TextBlock();

		Content = new Background
		{
			Color = new Color(200, 200, 100),
			Content = new Border
			{
				Content = _textBlock
			}
		};
	}

	public string Text
	{
		get => _textBlock.Text;
		set => _textBlock.Text = value;
	}
}
```

#### Implementing the `IControl` interface or inheriting the `Control` class

This approach can be used to define fully custom controls. All of the basic within this library are implemented this way.

The `IControl` interface requires providing 3 members:

```csharp
public interface IControl
{
	Character this[Position position] { get; }
	Size Size { get; }
	IDrawingContext Context { get; set; }
}
```

The `[]` operator must return a character that is to be displayed on the specific position. The position is defined relative to this control's space and not to the screen.

The control can also notify its parent about its internal changes using the provided `Context`. The `IDrawingContext` interface is defined as follows:

```csharp
public interface IDrawingContext
{
	Size MinSize { get; }
	Size MaxSize { get; }

	void Redraw(IControl control);
	void Update(IControl control, in Rect rect);

	event SizeLimitsChangedHandler SizeLimitsChanged;
}
```

If only a part of the control has changed, it should call the `Update` method, providing a reference to itself and the rect (once again - in its local space) that has to be redrawn. If the `Size` of the control has changed or the entire control requires redrawing, the control should call the `Redraw` method of its current `Context`.

The `Context` is also used to notify the child control about changes in size limits imposed on it by its parent. The child control should listen to the `SizeLimitsChanged` event and update its layout according to the `MinSize` and `MaxSize` values of the current `Context`.

When defining a custom control that can host other controls, you might have to implement a custom `IDrawingContext` class.

Instead of implementing the `IControl` and `IDrawingContext` directly, you can also use the `Control` and `DrawingContext` base classes. They allow for a similar level of flexibility, at the same time providing more advanced functionalities. 

The `DrawingContext` is an `IDisposable` non-abstract class that translates the parent's space into the child's space based on the provided size limits and offset. It also ensures that propagated notifications actually come from the hosted control and not from controls that were previously assigned to a given parent.

The `Control` class not only trims all of the incoming and outgoing messages to the current size limits but also allows to temporarily freeze the control so that only a single update message is generated after multiple related changes are performed.

For more information on how to define custom controls using the `IControl`/`IDrawingContext` interfaces or the `Control`/`DrawingContext` classes, please see the source code of one of the controls defined within this library.

## Input

As the standard `Console` class doesn't provide any event-based interface for detecting incoming characters, the availability of input messages has to be checked periodically within the main loop of your application. Of course, it's not required if your layout doesn't contain any interactive components.

To handle pending input messages, call the `ReadInput` method of the `ConsoleManager` class. It accepts a single argument being a collection of `IInputListener` objects. You can define this collection just once and reuse it - it specifies the list of input elements that are currently active and should be listening to keystrokes. The order of those elements is important, because if one control will set the `Handled` property of the provided `InputEvent`, the propagation will be terminated.

```csharp
var input = new IInputListener[]
{
	scrollPanel,
	tabPanel,
	textBox
};

for (int i = 0; ; i++)
{
	Thread.Sleep(10);
	ConsoleManager.ReadInput(input);
}
```

The `IInputListener` interface is not only restricted for classes that implement the `IControl` interface and can be used to define any custom (user defined) controllers that manage application's behavior.

#### Forms

As you might have noticed, there is no general purpose `Form` control available in this framework. That’s because it’s very hard to come up with a design that would fit all needs. Of course such obstacle is not a good reason on its own, but at the same time it’s extremely easy to implement a tailor made form controller within the target application itself. Here is an example:

```csharp
class FromController : IInputListener
{
	IInputListener _currentInput;
	
	// ...

	public void OnInput(InputEvent inputEvent)
	{
		if (inputEvent.Key.Key == ConsoleKey.Tab)
		{
			_currentInput = // nextInput...
		}
		else
		{
			_currentInput.OnInput(inputEvent)
		}
	}
}
```

After implementing it, all you have to do is to initialize an instance of this class with a list of you inputs and call the ` ConsoleManager.ReadInput(fromControllers)` each frame.

The biggest strength of this approach is that you decide what’s the order of controls within the form, you can do special validation after leaving each input, create a custom layout of the form itself, highlight currently active input, and much, much more. I believe it’s a good tradeoff. 

## Performance

This library is designed with high performance applications in mind. It means that if a control requests an `Update`, only the specified screen rectangle will be recalculated, and only if all of its parent controls agree that this part of the content is actually visible.

As the most expensive operation of the whole process is printing characters on the screen, the `ConsoleManager` defines its own, additional buffer. If the requested pixel (character) didn't change, it's not repainted.

## Contributions

I'm open to all sorts of contributions and feedback.
