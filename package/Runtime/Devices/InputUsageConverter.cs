using System;
using UnityEngine.XR;

namespace needle.Weavers.InputDevicesPatch
{
	public static class InputUsageConverter
	{
		/// <summary>
		/// try get xr node usage and xr node from InputFeatureUsageName name
		/// </summary>
		public static XRNodeUsage GetXRNodeUsage(this InputFeatureUsage usage, Delegate callback, XRNode device)
		{
			switch (usage.name)
			{
				case "isTracked":
					return new XRNodeUsage(device, InputType.Tracked, callback);
				case "leftEyePosition":
					return new XRNodeUsage(XRNode.LeftEye, InputType.Position, callback);
				case "leftEyeRotation":
					return new XRNodeUsage(XRNode.LeftEye, InputType.Rotation, callback);
				case "rightEyePosition":
					return new XRNodeUsage(XRNode.RightEye, InputType.Position, callback);
				case "rightEyeRotation":
					return new XRNodeUsage(XRNode.RightEye, InputType.Rotation, callback);
				case "centerEyePosition":
					return new XRNodeUsage(XRNode.CenterEye, InputType.Position, callback);
				case "centerEyeRotation":
					return new XRNodeUsage(XRNode.CenterEye, InputType.Rotation, callback);
			}

			return null;
		}
	}
}