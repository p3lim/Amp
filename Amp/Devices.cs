using CoreAudioApi;
using System;
using System.Collections.Generic;

namespace Amp
{
	class Devices
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
}
