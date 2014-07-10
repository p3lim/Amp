using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Amp
{
	public partial class Register : Form
	{
		public static String type;
		public static bool active;

		private static Keys key;
		private static int modifiers;
		private static String modifierString;
		private KeyboardHookListener keyboardHook;

		public Register(String newType)
		{
			InitializeComponent();

			type = newType;
			Shown += Register_Shown;

			keyboardHook = new KeyboardHookListener(new AppHooker());
			keyboardHook.Enabled = true;
			keyboardHook.KeyDown += keyboardHook_KeyDown;
		}

		private void Register_Shown(object sender, EventArgs e)
		{
			label.Text = "Press any\u2122 key combination";
			acceptButton.Hide();
			clearButton.Show();
			active = true;
		}

		private void keyboardHook_KeyDown(object sender, KeyEventArgs e)
		{	
			// Shift, Ctrl, Alt
			if (!new[] { 16, 17, 18 }.Contains(e.KeyValue))
			{
				e.SuppressKeyPress = true;

				modifiers = 0;
				modifierString = "";

				if (e.Shift)
				{
					modifiers += 1;
					modifierString += "Shift+";
				}

				if (e.Control)
				{
					modifiers += 2;
					modifierString += "Ctrl+";
				}

				if (e.Alt)
				{
					modifiers += 4;
					modifierString += "Alt+";
				}

				label.Text = String.Format("Use \"{0}\" as the new {1} keybinding?", modifierString + e.KeyCode, e.KeyValue);
				key = e.KeyCode;

				acceptButton.Show();
				clearButton.Hide();
			}
		}

		private void Exit()
		{
			keyboardHook.Enabled = false;
			active = false;
			type = "";
			
			this.Close();
		}

		private void ApplyBindings(int newKey, int newModifiers, String newModifierString)
		{
			if (type.Equals("cycle") || type.Equals("mute"))
			{
				Settings.SetKey(type, newKey);
				Settings.SetModifiers(type, newModifiers);
				Settings.SetModifierString(type, newModifierString);
			}
			else
			{
				Exit();
				return;
			}

			Options options = Application.OpenForms["Options"] as Options;
			options.UpdateDetails();

			Exit();
		}

		private void acceptButton_Click(object sender, EventArgs e)
		{
			ApplyBindings((int)key, modifiers, modifierString);
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			ApplyBindings(0, 0, "");
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			Exit();
		}
	}
}
