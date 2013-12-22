using CoreAudioApi;
using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Amp
{
	public partial class Options : Form
	{
		private bool _frozen = false;
		public Options()
		{
			InitializeComponent();
			Shown += OnShow;
		}

		public void Update()
		{
			if (!String.IsNullOrEmpty(Properties.Settings.Default.CycleKey))
				cycleButton.Text = "Cycle Hotkey: " + Regex.Replace(Properties.Settings.Default.CycleMod, ", ", "+") + "+" + Properties.Settings.Default.CycleKey;
			else
				cycleButton.Text = "Cycle Hotkey: None";

			if (!String.IsNullOrEmpty(Properties.Settings.Default.MuteKey))
				muteButton.Text = "Mute Hotkey: " + Regex.Replace(Properties.Settings.Default.MuteMod, ", ", "+") + "+" + Properties.Settings.Default.MuteKey;
			else
				muteButton.Text = "Mute Hotkey: None";
		}

		private void OnShow(object sender, EventArgs e)
		{
			_frozen = true;

			stateCheck.Checked = Properties.Settings.Default.StateSound;

			RegistryKey key = Registry.CurrentUser;
			RegistryKey group = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
			bootCheck.Checked = group.GetValue("Amp") != null;

			_frozen = false;

			Update();
		}

		private void cycleButton_Click(object sender, EventArgs e)
		{
			var Hook = new Amp.Register();
			Register.type = "cycle";
			Hook.ShowDialog(this);
		}

		private void muteButton_Click(object sender, EventArgs e)
		{
			var Hook = new Amp.Register();
			Register.type = "mute";
			Hook.ShowDialog(this);
		}

		private void stateCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (!_frozen)
			{
				Properties.Settings.Default.StateSound = !Properties.Settings.Default.StateSound;
				Properties.Settings.Default.Save();
			}
		}

		private void bootCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (!_frozen)
			{
				RegistryKey key = Registry.CurrentUser;
				RegistryKey group = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

				if (bootCheck.Checked)
					group.SetValue("Amp", Application.ExecutablePath, RegistryValueKind.String);
				else if (group.GetValue("Amp") != null)
					group.DeleteValue("Amp");
			}
		}
    }
}
