using UnityEngine.SpatialTracking;
using UnityEngine.XR.Interaction.Toolkit;

namespace needle.weaver.webxr
{
	public class XRRigInfo : DebugInfo
	{
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

		public override string GetInfo()
		{
			if (!xrRig) return string.Empty;
			var str = "";
			str += "TrackingMode: " + xrRig.trackingOriginMode + ", New Input? " + (newInputDriver == true) + ", Old Input?: " + (oldInputDriver == true);
			return str;
		}
	}
}