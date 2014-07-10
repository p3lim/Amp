using System;
using System.Windows.Forms;

namespace Amp
{
	public partial class Options : Form
	{
		private bool frozen;

		public Options()
		{
			InitializeComponent();

			ToolTip tipCycle = new ToolTip();
			tipCycle.SetToolTip(this.cycleNoteButton, "Plays a sound when devices are cycles");

			ToolTip tipMute = new ToolTip();
			tipMute.SetToolTip(this.muteNoteButton, "Plays a sound when microphone is toggled");

			frozen = false;
			Shown += Options_Shown;
		}

		public void UpdateDetails()
		{
			if (Settings.GetKey("cycle") > 0)
				cycleButton.Text = String.Format("Cycle Hotkey: {0}", Settings.GetModifierString("cycle") + (Keys)Settings.GetKey("cycle"));
			else
				cycleButton.Text = "Cycle Hotkey: None";

			if (Settings.GetKey("mute") > 0)
				muteButton.Text = String.Format("Mute Hotkey: {0}", Settings.GetModifierString("mute") + (Keys)Settings.GetKey("mute"));
			else
				muteButton.Text = "Mute Hotkey: None";

			if (Settings.ShouldPlaySound("cycle"))
				cycleNoteButton.BackgroundImage = Resources.NoteEnabled;
			else
				cycleNoteButton.BackgroundImage = Resources.NoteDisabled;

			if (Settings.ShouldPlaySound("mute"))
				muteNoteButton.BackgroundImage = Resources.NoteEnabled;
			else
				muteNoteButton.BackgroundImage = Resources.NoteDisabled;
		}

		private void Options_Shown(object sender, System.EventArgs e)
		{
			frozen = true;
			bootCheck.Checked = Settings.ShouldAutoRun();
			balloonCheck.Checked = Settings.ShouldShowBalloon();
			frozen = false;

			UpdateDetails();
		}

		private void cycleButton_Click(object sender, System.EventArgs e)
		{
			var register = new Register("cycle");
			register.ShowDialog(this);
		}

		private void cycleNoteButton_Click(object sender, System.EventArgs e)
		{
			Settings.SetShouldPlaySound("cycle", !Settings.ShouldPlaySound("cycle"));
			UpdateDetails();
		}

		private void muteButton_Click(object sender, System.EventArgs e)
		{
			var register = new Register("mute");
			register.ShowDialog(this);
		}

		private void muteNoteButton_Click(object sender, System.EventArgs e)
		{
			Settings.SetShouldPlaySound("mute", !Settings.ShouldPlaySound("mute"));
			UpdateDetails();
		}

		private void bootCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!frozen)
				Settings.SetShouldAutoRun(bootCheck.Checked);
		}

		private void balloonCheck_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!frozen)
				Settings.SetShouldShowBalloon(balloonCheck.Checked);
		}
	}
}
