﻿using CoreAudioApi;
using Microsoft.Win32;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Amp
{
	public class App : Form
	{
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new App());
		}

		private KeyboardHookListener keyboardHook;

		private NotifyIcon trayIcon;
		private ContextMenu trayMenu;

		public App()
		{
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			{
				string resourceName = new AssemblyName(args.Name).Name + ".dll";
				string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
				{
					Byte[] assemblyData = new Byte[stream.Length];
					stream.Read(assemblyData, 0, assemblyData.Length);
					return Assembly.Load(assemblyData);
				}
			};

			InitializeComponent();
		}

		private void InitializeComponent()
		{
			keyboardHook = new KeyboardHookListener(new GlobalHooker());
			keyboardHook.Enabled = true;
			keyboardHook.KeyDown += keyboardHook_KeyDown;

			trayMenu = new ContextMenu();
			trayMenu.MenuItems.Add("Mixer", (sender, e) => System.Diagnostics.Process.Start("sndvol.exe"));
			trayMenu.MenuItems.Add("Devices", (sender, e) => System.Diagnostics.Process.Start("mmsys.cpl"));
			trayMenu.MenuItems.Add("-");
			trayMenu.MenuItems.Add("Settings", trayMenu_Settings);
			trayMenu.MenuItems.Add("-");
			trayMenu.MenuItems.Add("Exit", trayMenu_Exit);

			trayIcon = new NotifyIcon();
			trayIcon.Text = "Amp";
			trayIcon.ContextMenu = trayMenu;
			trayIcon.DoubleClick += trayIcon_DoubleClick;
			trayIcon.Visible = true;

			trayIcon_UpdateIcon(Devices.IsMuted());
		}

		private void keyboardHook_KeyDown(object sender, KeyEventArgs e)
		{
			if (Register.active)
				return;

			int modifiers = 0;

			if (e.Shift)
				modifiers += 1;
			if (e.Control)
				modifiers += 2;
			if (e.Alt)
				modifiers += 4;

			int keyCode = (int)e.KeyCode;

			if(keyCode == Settings.GetKey("cycle") && modifiers == Settings.GetModifiers("cycle"))
			{
				e.SuppressKeyPress = true;

				String device = Devices.CycleDevices();

				if (Settings.ShouldShowBalloon())
					trayIcon.ShowBalloonTip(500, "Amp", "Audio output changed to " + device, ToolTipIcon.Info);

				if (Settings.ShouldPlaySound("cycle"))
					new System.Media.SoundPlayer(Resources.Sound).Play();
			}

			if (keyCode == Settings.GetKey("mute") && modifiers == Settings.GetModifiers("mute"))
			{
				e.SuppressKeyPress = true;
				trayIcon_DoubleClick(sender, e);
			}
		}

		private void trayIcon_UpdateIcon(bool muted)
		{
			if (muted)
				trayIcon.Icon = Resources.Disabled;
			else
				trayIcon.Icon = Resources.Enabled;
		}

		private void trayIcon_DoubleClick(object sender, EventArgs e)
		{
			bool muted = Devices.MuteMicrophone();
			trayIcon_UpdateIcon(muted);

			if (Settings.ShouldShowBalloon())
			{
				if (muted)
					trayIcon.ShowBalloonTip(500, "Amp", "Microphone is now muted", ToolTipIcon.Info);
				else
					trayIcon.ShowBalloonTip(500, "Amp", "Microphone is now active", ToolTipIcon.Info);
			}

			if (Settings.ShouldPlaySound("mute"))
				new System.Media.SoundPlayer(Resources.Sound).Play();
		}

		public static void trayMenu_Settings(object sender, EventArgs e)
		{
			Options options = Application.OpenForms["Options"] as Options;
			if (options != null)
				options.BringToFront();
			else
			{
				var newOptions = new Options();
				newOptions.Show();
			}
		}

		public static void trayMenu_Exit(object sender, EventArgs e)
		{
			Application.Exit();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				trayIcon.Dispose();
				
			base.Dispose(disposing);
		}

		protected override void OnLoad(EventArgs e)
		{
			Visible = false;
			ShowInTaskbar = false;
			
			base.OnLoad(e);
		}
	}

	public class Devices
	{
		private static readonly MMDeviceEnumerator devicesEnum = new MMDeviceEnumerator();
		private static readonly PolicyConfigClient policyConfig = new PolicyConfigClient();

		public static string CycleDevices()
		{
			MMDeviceCollection devices = devicesEnum.EnumerateAudioEndPoints(EDataFlow.eRender, EDeviceState.DEVICE_STATE_ACTIVE);
			for (int index = 0; index < devices.Count; index++)
			{
				MMDevice device = devices[index];
				if (device.ID != devicesEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia).ID)
				{
					policyConfig.SetDefaultEndpoint(device.ID, ERole.eMultimedia);
					policyConfig.SetDefaultEndpoint(device.ID, ERole.eCommunications);
					break;
				}
			}

			return devicesEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia).FriendlyName;
		}

		public static bool MuteMicrophone()
		{
			MMDevice microphone = devicesEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications);
			microphone.AudioEndpointVolume.Mute = !microphone.AudioEndpointVolume.Mute;
			return microphone.AudioEndpointVolume.Mute;
		}

		public static bool IsMuted()
		{
			return devicesEnum.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eCommunications).AudioEndpointVolume.Mute;
		}
	}

	public class Settings
	{
		private static String appPath = "HKEY_CURRENT_USER\\Software\\Amp";
		private static String runPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run";

		public static int GetKey(String type)
		{
			return Convert.ToInt32(Registry.GetValue(appPath, type + "Key", 0));
		}

		public static int GetModifiers(String type)
		{
			return Convert.ToInt32(Registry.GetValue(appPath, type + "Modifiers", 0));
		}

		public static String GetModifierString(String type)
		{
			return Convert.ToString(Registry.GetValue(appPath, type + "ModifierString", ""));
		}

		public static bool ShouldPlaySound(String type)
		{
			return Convert.ToBoolean(Registry.GetValue(appPath, type + "Sound", 0));
		}

		public static bool ShouldShowBalloon()
		{
			return Convert.ToBoolean(Registry.GetValue(appPath, "showBalloon", 0));
		}

		public static bool ShouldAutoRun()
		{
			return Convert.ToString(Registry.GetValue(runPath, "Amp", "")).Length != 0;
		}

		public static bool SetKey(String type, int key)
		{
			Registry.SetValue(appPath, type + "Key", key, RegistryValueKind.DWord);
			return true;
		}

		public static bool SetModifiers(String type, int modifiers)
		{
			Registry.SetValue(appPath, type + "Modifiers", modifiers, RegistryValueKind.DWord);
			return true;
		}

		public static bool SetModifierString(String type, String modifierString)
		{
			Registry.SetValue(appPath, type + "ModifierString", modifierString, RegistryValueKind.String);
			return true;
		}

		public static bool SetShouldPlaySound(String type, bool enabled)
		{
			Registry.SetValue(appPath, type + "Sound", enabled, RegistryValueKind.DWord);
			return true;
		}

		public static bool SetShouldShowBalloon(bool enabled)
		{
			Registry.SetValue(appPath, "showBalloon", enabled, RegistryValueKind.DWord);
			return true;
		}

		public static bool SetShouldAutoRun(bool enabled)
		{
			if (enabled)
				Registry.SetValue(runPath, "Amp", Application.ExecutablePath, RegistryValueKind.String);
			else
				Registry.SetValue(runPath, "Amp", "", RegistryValueKind.String);

			return true;
		}
	}
}