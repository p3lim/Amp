using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Amp
{
	public sealed class Hook : IDisposable
	{
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		private class Window : NativeWindow, IDisposable
		{
			public event EventHandler<KeyPressedEventArgs> OnKeyPressed;
			public Window()
			{
				this.CreateHandle(new CreateParams());
			}

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 0x0312)
				{
					Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
					Modifiers mod = (Modifiers)((int)m.LParam & 0xFFFF);

					if (OnKeyPressed != null)
						OnKeyPressed(this, new KeyPressedEventArgs(mod, key));
				}

				base.WndProc(ref m);
			}

			public void Dispose()
			{
				this.DestroyHandle();
			}
		}

		private Window _window = new Window();
		private int _current;

		public event EventHandler<KeyPressedEventArgs> OnKeyPressed;
		public Hook()
		{
			_window.OnKeyPressed += delegate(object sender, KeyPressedEventArgs args)
			{
				if (OnKeyPressed != null)
					OnKeyPressed(this, args);
			};
		}

		public void Register(Modifiers mod, Keys key)
		{
			_current++;

			if (!RegisterHotKey(_window.Handle, _current, (uint)mod, (uint)key))
				throw new InvalidOperationException("Could not bind the key");
		}

		public void Dispose()
		{
			for (int index = _current; index > 0; index--)
				UnregisterHotKey(_window.Handle, index);
	
			_window.Dispose();
		}
	}

	public class KeyPressedEventArgs : EventArgs
	{
		private Modifiers _mod;
		private Keys _key;

		internal KeyPressedEventArgs(Modifiers mod, Keys key)
		{
			_mod= mod;
			_key = key;
		}

		public Modifiers mod
		{
			get { return _mod; }
		}

		public Keys key
		{
			get { return _key; }
		}
	}

	[Flags]
	public enum Modifiers : uint
	{
		Alt = 1,
		Control = 2,
		Shift = 4,
	}
}
