using System;
using System.Linq;
using System.Windows.Forms;

namespace Amp
{
	public partial class Register : Form
	{
		public static string type;
		private static Keys key;
		private static Modifiers mod;

		public Register()
		{
			InitializeComponent();
			Shown += Hook_Shown;

			hookInput.KeyDown += hookInput_KeyDown;
			hookInput.LostFocus += hookInput_LostFocus;
		}

		private void Hook_Shown(object sender, EventArgs e)
		{
			label.Text = "Press any\u2122 key combination";

			acceptButton.Hide();
			clearButton.Show();
			hookInput.Focus();
		}

		private void hookInput_LostFocus(object sender, EventArgs e)
		{
			hookInput.Focus();
		}

		private void hookInput_KeyDown(object sender, KeyEventArgs e)
		{
			if (!new[] { 8, 16, 17, 18, 27, 46 }.Contains(e.KeyValue))
			{
				key = 0;
				mod = 0;

				var modString = "";
				if (e.Alt)
				{
					mod |= Amp.Modifiers.Alt;
					modString += "Alt+";
				}
				
				if (e.Control)
				{
					mod |= Amp.Modifiers.Control;
					modString += "Ctrl+";
				}

				if (e.Shift)
				{
					mod |= Amp.Modifiers.Shift;
					modString += "Shift+";
				}

				if (modString.Length > 0)
				{
					label.Text = String.Format("Use \"{0}{1}\" as the new {2} keybinding?", modString, e.KeyCode, type);
					key = e.KeyCode;

					acceptButton.Show();
					clearButton.Hide();
				}
			}
		}

		private void acceptButton_Click(object sender, EventArgs e)
		{
			if (type.Equals("cycle")) {
				Properties.Settings.Default.CycleKey = Enum.Format(typeof(Keys), key, "g");
				Properties.Settings.Default.CycleMod = Enum.Format(typeof(Modifiers), mod, "g");
			}
			else if (type.Equals("mute")) {
				Properties.Settings.Default.MuteKey = Enum.Format(typeof(Keys), key, "g");
				Properties.Settings.Default.MuteMod = Enum.Format(typeof(Modifiers), mod, "g");
			}
			else {
				this.Close();
				return;
			}

			Properties.Settings.Default.Save();
			Amp.Program.ApplyBindings();

			Options options = Application.OpenForms["Options"] as Options;
			options.OnShow(this, new EventArgs());

			this.Close();
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			if (type.Equals("cycle"))
			{
				Properties.Settings.Default.CycleKey = "";
				Properties.Settings.Default.CycleMod = "";
			}
			else if (type.Equals("mute"))
			{
				Properties.Settings.Default.MuteKey = "";
				Properties.Settings.Default.MuteMod = "";
			}
			else
			{
				this.Close();
				return;
			}

			Properties.Settings.Default.Save();
			Amp.Program.ApplyBindings();

			Options options = Application.OpenForms["Options"] as Options;
			options.OnShow(this, new EventArgs());

			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
