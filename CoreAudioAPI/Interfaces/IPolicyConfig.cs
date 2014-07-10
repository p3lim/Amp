/*
  LICENSE
  -------
  Copyright (C) 2007-2010 Ray Molenkamp

  This source code is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this source code or the software it produces.

  Permission is granted to anyone to use this source code for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this source code must not be misrepresented; you must not
     claim that you wrote the original source code.  If you use this source code
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original source code.
  3. This notice may not be removed or altered from any source distribution.
*/
﻿using System;
using System.Runtime.InteropServices;

namespace CoreAudioApi.Interfaces
{
	[Guid("f8679f50-850a-41cf-9c72-430f290290c8"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPolicyConfig
	{
		[PreserveSig]
		int GetMixFormat(string pszDeviceName, IntPtr ppFormat);

		[PreserveSig]
		int GetDeviceFormat(string pszDeviceName, bool bDefault, IntPtr ppFormat);

		[PreserveSig]
		int ResetDeviceFormat(string pszDeviceName);

		[PreserveSig]
		int SetDeviceFormat(string pszDeviceName, IntPtr pEndpointFormat, IntPtr MixFormat);

		[PreserveSig]
		int GetProcessingPeriod(string pszDeviceName, bool bDefault, IntPtr pmftDefaultPeriod, IntPtr pmftMinimumPeriod);

		[PreserveSig]
		int SetProcessingPeriod(string pszDeviceName, IntPtr pmftPeriod);

		[PreserveSig]
		int GetShareMode(string pszDeviceName, IntPtr pMode);

		[PreserveSig]
		int SetShareMode(string pszDeviceName, IntPtr mode);

		[PreserveSig]
		int GetPropertyValue(string pszDeviceName, bool bFxStore, IntPtr key, IntPtr pv);

		[PreserveSig]
		int SetPropertyValue(string pszDeviceName, bool bFxStore, IntPtr key, IntPtr pv);

		[PreserveSig]
		int SetDefaultEndpoint(string pszDeviceName, ERole role);

		[PreserveSig]
		int SetEndpointVisibility(string pszDeviceName, bool bVisible);
	}
}