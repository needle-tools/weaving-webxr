using System;
using UnityEngine;
using UnityEngine.XR;

namespace needle.Weavers.InputDevicesPatch
{
	public class MockDeviceBuilder
	{
		public static MockInputDevice CreateHeadset(Func<bool> isTrackedCallback, Func<Vector3> positionCallback, Func<Quaternion> rotationCallback, Func<InputTrackingState> stateCallback = null)
		{
			var device = new MockInputDevice("<XRHMD>", XRNode.Head)
			{
				SerialNumber = "1.0.0",
				Manufacturer = "Needle",
				DeviceCharacteristics = InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice
			};

			device.AddFeature(CommonUsages.isTracked, isTrackedCallback);
			device.AddFeature(CommonUsages.trackingState,  stateCallback ?? (() => InputTrackingState.Position | InputTrackingState.Rotation));
			device.AddFeature(CommonUsages.devicePosition, positionCallback);
			device.AddFeature(CommonUsages.deviceRotation, rotationCallback);
			device.AddFeature(CommonUsages.leftEyePosition, positionCallback);
			device.AddFeature(CommonUsages.leftEyeRotation, rotationCallback);
			device.AddFeature(CommonUsages.rightEyePosition, positionCallback);
			device.AddFeature(CommonUsages.rightEyeRotation, rotationCallback);
			device.AddFeature(CommonUsages.centerEyePosition, positionCallback);
			device.AddFeature(CommonUsages.centerEyeRotation, rotationCallback);
			return device;
		}
		
		public static MockInputDevice CreateRightController(Func<bool> isTrackedCallback, Func<Vector3> positionCallback, Func<Quaternion> rotationCallback, Func<InputTrackingState> stateCallback = null)
		{
			return CreateController(XRNode.RightHand, isTrackedCallback, positionCallback, rotationCallback, stateCallback);
		}
		
		private static MockInputDevice CreateController(XRNode node, Func<bool> isTrackedCallback, Func<Vector3> positionCallback, Func<Quaternion> rotationCallback, Func<InputTrackingState> stateCallback = null)
		{
			var device = new MockInputDevice("<XRController>", node)
			{
				SerialNumber = "1.0.0",
				Manufacturer = "Needle",
				DeviceCharacteristics = InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice
			};

			device.AddFeature(CommonUsages.isTracked, isTrackedCallback);
			device.AddFeature(CommonUsages.trackingState, stateCallback ?? (() => InputTrackingState.Position | InputTrackingState.Rotation));
			device.AddFeature(CommonUsages.devicePosition, positionCallback);
			device.AddFeature(CommonUsages.deviceRotation, rotationCallback);
			return device;
		}
	}
}