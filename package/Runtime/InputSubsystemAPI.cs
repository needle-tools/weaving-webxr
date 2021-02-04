using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public static class InputSubsystemAPI
	{
		public static XRInputSubsystem Instance => XRInputSubsystem_Patch.Instance;

		public static void SetSupportedTrackingMode(TrackingOriginModeFlags modes)
		{
			XRInputSubsystem_Patch.SupportedTrackingOriginMode = modes;
		}

		public static void Connect(this MockInputDevice dev)
		{
			if (dev == null) return;
			if (XRInputSubsystem_Patch.InputDevices.Contains(dev)) return;
			XRInputSubsystem_Patch.InputDevices.Add(dev);
#if DEVELOPMENT_BUILD
			Debug.Log("Registered input device " + dev.Id + " - " + dev.Node);
#endif
		}

		public static void Disconnect(this MockInputDevice dev)
		{
			if (dev == null) return;
			if (!XRInputSubsystem_Patch.InputDevices.Contains(dev)) return;
			XRInputSubsystem_Patch.InputDevices.Remove(dev);
#if DEVELOPMENT_BUILD
			Debug.Log("Removed input device " + dev.Id + " - " + dev.Node);
#endif
		}
	}
}