using CoreAudioApi;
using System;
using System.Windows.Forms;

namespace Amp
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			bool created;
			using (var muted = new System.Threading.Mutex(true, "Amp", out created))
			{
				if (created)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);

					trayIcon = new NotifyIcon();
					InitializeTray();

					hookCycle = new Hook();
					hookMute = new Hook();
					ApplyBindings();

					Application.Run();
				}
			}
		}

		#region tray
		public static NotifyIcon trayIcon;

		private static void InitializeTray()
		{
			var contextMenu = new ContextMenu();
			contextMenu.MenuItems.Add("Mixer", (sender, e) => System.Diagnostics.Process.Start("sndvol.exe"));
			contextMenu.MenuItems.Add("Devices", (sender, e) => System.Diagnostics.Process.Start("mmsys.cpl"));
			contextMenu.MenuItems.Add("-");
			contextMenu.MenuItems.Add("Settings", contextMenu_Settings);
			contextMenu.MenuItems.Add("-");
			contextMenu.MenuItems.Add("Exit", contextMenu_Exit);

			trayIcon.ContextMenu = contextMenu;
			trayIcon.Click += hookMute_OnKeyPressed; // TODO: Give the user the option what happens here
			trayIcon.Text = "Amp";
			trayIcon.Visible = true;

			UpdateTray(Devices.IsMuted());
		}

		public static void UpdateTray(bool muted)
		{
			if (muted)
				trayIcon.Icon = Properties.Resources.Disabled;
			else
				trayIcon.Icon = Properties.Resources.Enabled;
		}

		private static void contextMenu_Settings(object sender, EventArgs e)
		{
			Options option = Application.OpenForms["Options"] as Options;
			if (option != null)
				option.BringToFront();
			else
			{
				var Options = new Amp.Options();
				Options.Show();
			}
		}

		private static void contextMenu_Exit(object sender, EventArgs e)
		{
			trayIcon.Dispose();
			Application.Exit();
		}
		#endregion

		#region binding
		private static Hook hookCycle { get; set; }
		private static Hook hookMute { get; set; }

		public static void ApplyBindings()
		{
			try
			{
				if (!String.IsNullOrEmpty(Properties.Settings.Default.CycleKey))
				{
					hookCycle.Dispose();
					hookCycle = new Hook();

					Keys key = (Keys)Enum.Parse(typeof(Keys), Properties.Settings.Default.CycleKey);
					Modifiers mod = (Modifiers)Enum.Parse(typeof(Modifiers), Properties.Settings.Default.CycleMod);

					hookCycle.Register(mod, key);
					hookCycle.OnKeyPressed += hookCycle_OnKeyPressed;
				}
				else
				{
					hookCycle.Dispose();
					hookCycle = new Hook();
				}

				if (!String.IsNullOrEmpty(Properties.Settings.Default.MuteKey))
				{
					hookMute.Dispose();
					hookMute = new Hook();

					Keys key = (Keys)Enum.Parse(typeof(Keys), Properties.Settings.Default.MuteKey);
					Modifiers mod = (Modifiers)Enum.Parse(typeof(Modifiers), Properties.Settings.Default.MuteMod);

					hookMute.Register(mod, key);
					hookMute.OnKeyPressed += hookMute_OnKeyPressed;
				}
				else
				{
					hookMute.Dispose();
					hookMute = new Hook();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(String.Format("{0}\n\n{1}", e.Message, e.StackTrace));
				// TODO: balloon notification?
			}
		}

		private static void hookCycle_OnKeyPressed(object sender, EventArgs e)
		{
			var device = Devices.CycleDevices();
			trayIcon.ShowBalloonTip(500, "Amp", "Audio output changed to " + device, ToolTipIcon.Info);

			if (Properties.Settings.Default.CycleSound)
				new System.Media.SoundPlayer(Properties.Resources.Sound_Switch).Play();
		}

		private static void hookMute_OnKeyPressed(object sender, EventArgs e)
		{
			var muted = Devices.MuteMicrophone();
			UpdateTray(muted);

			if (muted)
				trayIcon.ShowBalloonTip(500, "Amp", "Microphone is now muted", ToolTipIcon.Info);
			else
				trayIcon.ShowBalloonTip(500, "Amp", "Microphone is now active", ToolTipIcon.Info);

			if (Properties.Settings.Default.MuteSound)
				new System.Media.SoundPlayer(Properties.Resources.Sound_Switch).Play();
		}
		#endregion
	}
}
