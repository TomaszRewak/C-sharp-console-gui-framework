using System;
using System.Collections.Generic;
using System.Text;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.Utils;

namespace ConsoleGUI.UserDefined
{
	public class SimpleControl : IControl
	{
		private IControl _content;
		protected IControl Content
		{
			get => _content;
			set => Setter
				.Set(ref _content, value)
				.Then(BindContent);
		}

		private IDrawingContext _context;
		public IDrawingContext Context
		{
			get => _context;
			set => Setter
				.Set(ref _context, value)
				.Then(BindContent);
		}

		private DrawingContextWrapper _contextWrapper;
		private DrawingContextWrapper ContextWrapper
		{
			get => _contextWrapper;
			set => Setter
				.SetDisposable(ref _contextWrapper, value);
		}

		public Character this[Position position] => _content[position];
		public Size Size => _content.Size;

		private void BindContent()
		{
			ContextWrapper = new DrawingContextWrapper(this, Content, Context);
			Content.Context = ContextWrapper;
		}
	}
}
