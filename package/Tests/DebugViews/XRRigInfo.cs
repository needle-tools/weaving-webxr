#if XR_INTERACTION_TOOLKIT_INSTALLED
using UnityEngine.SpatialTracking;
using UnityEngine.XR.Interaction.Toolkit;
#endif

namespace needle.weaver.webxr
{
	public class XRRigInfo : DebugInfo
	{
#if XR_INTERACTION_TOOLKIT_INSTALLED
		private XRRig xrRig;
		private UnityEngine.SpatialTracking.TrackedPoseDriver oldInputDriver;
		private UnityEngine.InputSystem.XR.TrackedPoseDriver newInputDriver;
		
		private void OnEnable()
		{
			xrRig = FindObjectOfType<XRRig>();
			if (!xrRig) enabled = false;

			oldInputDriver = FindObjectOfType<TrackedPoseDriver>();
			newInputDriver = FindObjectOfType<UnityEngine.InputSystem.XR.TrackedPoseDriver>();
		}
#endif

		public override string GetInfo()
		{
#if XR_INTERACTION_TOOLKIT_INSTALLED
			if (!xrRig) return string.Empty;
			var str = "";
			str += "TrackingMode: " + xrRig.trackingOriginMode + ", New Input? " + (newInputDriver == true) + ", Old Input?: " + (oldInputDriver == true);
			return str;
#else
			return "XR InteractionToolkit not installed";
#endif
		}
	}
}