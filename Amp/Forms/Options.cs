using CoreAudioApi;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Amp
{
	public partial class Options : Form
    {
		public Options()
        {
            InitializeComponent();
			Shown += OnShow;
        }

		public void OnShow(object sender, EventArgs e)
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
    }
}
