using System;
using Terminal.Gui;

namespace DevDashboard.Components
{
    public class DashComponent
    {
		/// <summary>
		/// The Top level for the <see cref="Scenario"/>. This should be set to <see cref="Terminal.Gui.Application.Top"/> in most cases.
		/// </summary>
		public Toplevel Top { get; set; }

		/// <summary>
		/// The Window for the <see cref="Scenario"/>. This should be set within the <see cref="Terminal.Gui.Application.Top"/> in most cases.
		/// </summary>
		//public Window Win { get; set; }

		public FrameView Frame { get; set; }

		public string Name { get; set; }

		public virtual void Init(Toplevel top, Pos x, Pos y, Dim height, Dim width, ColorScheme colorScheme)
		{
			Application.Init();

			Top = top;
			if (Top == null)
			{
				Top = Application.Top;
			}

			//Win = new Window($"CTRL-Q to Close - Scenario: {this.Name}")
			//{
			//	X = 0,
			//	Y = 0,
			//	Width = Dim.Fill(),
			//	Height = Dim.Fill(),
			//	ColorScheme = colorScheme,
			//};
			//Top.Add(Win);

			this.Frame = new FrameView(this.Name)
			{
				X = x,
				Y = y,
				Height = height,
				Width = width,
				ColorScheme = colorScheme
			};
			top.Add(this.Frame);
		}

		/// <summary>
		/// Override this to implement the <see cref="Scenario"/> setup logic (create controls, etc...). 
		/// </summary>
		/// <remarks>This is typically the best place to put scenario logic code.</remarks>
		public virtual void Setup()
        {
        }

		protected void AddTimeout(TimeSpan ts, Action action)
		{
			Application.MainLoop.AddTimeout(ts, ml =>
			{
				action();
				return true;
			});
		}
	}
}
