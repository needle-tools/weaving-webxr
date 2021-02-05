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

	}
}