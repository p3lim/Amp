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

			ToolTip tipCycle = new ToolTip();
			tipCycle.SetToolTip(this.cycleNoteButton, "Plays a sound when devices are cycled");

			ToolTip tipMute = new ToolTip();
			tipMute.SetToolTip(this.muteNoteButton, "Plays a sound when microphone mute is toggled");

			Shown += OnShow;
		}

		public void UpdateDetails()
		{
			if (!String.IsNullOrEmpty(Properties.Settings.Default.CycleKey))
				cycleButton.Text = "Cycle Hotkey: " + Regex.Replace(Properties.Settings.Default.CycleMod, ", ", "+") + "+" + Properties.Settings.Default.CycleKey;
			else
				cycleButton.Text = "Cycle Hotkey: None";

			if (!String.IsNullOrEmpty(Properties.Settings.Default.MuteKey))
				muteButton.Text = "Mute Hotkey: " + Regex.Replace(Properties.Settings.Default.MuteMod, ", ", "+") + "+" + Properties.Settings.Default.MuteKey;
			else
				muteButton.Text = "Mute Hotkey: None";

			if (Properties.Settings.Default.CycleSound)
				cycleNoteButton.BackgroundImage = Properties.Resources.Note;
			else
				cycleNoteButton.BackgroundImage = Properties.Resources.NoteDisabled;

			if (Properties.Settings.Default.MuteSound)
				muteNoteButton.BackgroundImage = Properties.Resources.Note;
			else
				muteNoteButton.BackgroundImage = Properties.Resources.NoteDisabled;
		}

		private void OnShow(object sender, EventArgs e)
		{
			_frozen = true;
			RegistryKey key = Registry.CurrentUser;
			RegistryKey group = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
			bootCheck.Checked = group.GetValue("Amp") != null;
			balloonCheck.Checked = Properties.Settings.Default.TrayNotifications;
			_frozen = false;

			UpdateDetails();
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

		private void balloonCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (!_frozen)
			{
				Properties.Settings.Default.TrayNotifications = balloonCheck.Checked;
			}
		}

		private void cycleNoteButton_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.CycleSound = !Properties.Settings.Default.CycleSound;
			Properties.Settings.Default.Save();
			UpdateDetails();
		}

		private void muteNoteButton_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.MuteSound = !Properties.Settings.Default.MuteSound;
			Properties.Settings.Default.Save();
			UpdateDetails();
		}
    }
}
