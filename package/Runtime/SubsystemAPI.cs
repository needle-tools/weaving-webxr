using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public static class SubsystemAPI
	{
		public static XRInputSubsystem Instance => XRInputSubsystem_Patch.Instance;

		public static void SetSupportedTrackingMode(TrackingOriginModeFlags modes)
		{
			XRInputSubsystem_Patch.SupportedTrackingOriginMode = modes;
		}
		
		public static void RegisterInputDevice(MockInputDevice dev)
		{
			if (dev == null) return;
			if (XRInputSubsystem_Patch.InputDevices.Contains(dev)) return;
			XRInputSubsystem_Patch.InputDevices.Add(dev);
			Debug.Log("Registered input device " + dev.Id + " - " + dev.Node);
		}

		public static void RemoveDevice(MockInputDevice dev)
		{
			if (dev == null) return;
			if (!XRInputSubsystem_Patch.InputDevices.Contains(dev)) return;
			XRInputSubsystem_Patch.InputDevices.Remove(dev);
			Debug.Log("Removed input device " + dev.Id + " - " + dev.Node);
		}
	}
}