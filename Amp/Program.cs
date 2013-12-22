﻿using CoreAudioApi;
using System;
using System.Windows.Forms;

namespace Amp
{
    static class Program
    {
		[STAThread]
        static void Main()
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
			contextMenu.MenuItems.Add("Exit", (sender, e) => Application.Exit());

			trayIcon.ContextMenu = contextMenu;
			trayIcon.DoubleClick += (sender, e) => System.Diagnostics.Process.Start("sndvol.exe");
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
			var Options = new Amp.Options();
			Options.Show();
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
		}

		private static void hookMute_OnKeyPressed(object sender, EventArgs e)
		{
			var muted = Devices.MuteMicrophone();
			UpdateTray(muted);

			if(muted)
				trayIcon.ShowBalloonTip(500, "Amp", "Microphone is now muted", ToolTipIcon.Info);
			else
				trayIcon.ShowBalloonTip(500, "Amp", "Microphone is now active", ToolTipIcon.Info);
		}
		#endregion
	}
}
