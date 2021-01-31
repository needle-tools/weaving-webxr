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
			// refer to CommonUsages to naming
			switch (usage.name)
			{
				case "IsTracked":
					return new XRNodeUsage(device, InputType.Tracked, callback);
				case "LeftEyePosition":
					return new XRNodeUsage(XRNode.LeftEye, InputType.Position, callback);
				case "LeftEyeRotation":
					return new XRNodeUsage(XRNode.LeftEye, InputType.Rotation, callback);
				case "RightEyePosition":
					return new XRNodeUsage(XRNode.RightEye, InputType.Position, callback);
				case "RightEyeRotation":
					return new XRNodeUsage(XRNode.RightEye, InputType.Rotation, callback);
				case "CenterEyePosition":
					return new XRNodeUsage(XRNode.CenterEye, InputType.Position, callback);
				case "CenterEyeRotation":
					return new XRNodeUsage(XRNode.CenterEye, InputType.Rotation, callback);
				case "DevicePosition":
					return new XRNodeUsage(device, InputType.Position, callback);
				case "DeviceRotation":
					return new XRNodeUsage(device, InputType.Rotation, callback);
			}

			return null;
		}
	}
}