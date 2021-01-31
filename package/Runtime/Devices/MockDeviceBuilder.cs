using System;
using UnityEngine;
using UnityEngine.XR;

namespace needle.Weavers.InputDevicesPatch
{
	public class MockDeviceBuilder
	{
		public static MockInputDevice CreateHeadset(Func<bool> isTrackedCallback, Func<Vector3> positionCallback, Func<Quaternion> rotationCallback)
		{
			var deviceName = "<XRHMD>";
			var device = new MockInputDevice(deviceName, XRNode.Head)
			{
				SerialNumber = "1.0.0",
				Manufacturer = "Needle",
				DeviceCharacteristics = InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice
			};

			device.AddFeature(new InputFeatureUsage<bool>("isTracked"), isTrackedCallback);
			device.AddFeature(new InputFeatureUsage<InputTrackingState>("trackingState"), () => InputTrackingState.Position | InputTrackingState.Rotation);
			device.AddFeature(new InputFeatureUsage<Vector3>("devicePosition"), positionCallback);
			device.AddFeature(new InputFeatureUsage<Quaternion>("deviceRotation"), rotationCallback);
			device.AddFeature(new InputFeatureUsage<Vector3>("leftEyePosition"), positionCallback);
			device.AddFeature(new InputFeatureUsage<Quaternion>("leftEyeRotation"), rotationCallback);
			device.AddFeature(new InputFeatureUsage<Vector3>("rightEyePosition"), positionCallback);
			device.AddFeature(new InputFeatureUsage<Quaternion>("rightEyeRotation"), rotationCallback);
			device.AddFeature(new InputFeatureUsage<Vector3>("centerEyePosition"), positionCallback);
			device.AddFeature(new InputFeatureUsage<Quaternion>("centerEyeRotation"), rotationCallback);
			return device;
		}
	}
}