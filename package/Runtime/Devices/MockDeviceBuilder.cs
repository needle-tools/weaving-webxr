using System;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class MockDeviceBuilder
	{
		public static MockInputDevice CreateHeadset(Func<bool> isTrackedCallback, Func<Vector3> positionCallback, Func<Quaternion> rotationCallback, Func<InputTrackingState> stateCallback = null, Func<Vector3> leftEyePositionCallback = null, Func<Quaternion> leftEyeRotationCallback = null, Func<Vector3> rightEyePositionCallback = null, Func<Quaternion> rightEyeRotationCallback = null)
		{
			var device = new MockInputDevice("<XRHMD>", 
				InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice,
				XRNode.Head, "XRHMD");

			device.AddFeature(CommonUsages.isTracked, isTrackedCallback);
			device.AddFeature(CommonUsages.trackingState,  stateCallback ?? (() => InputTrackingState.Position | InputTrackingState.Rotation));
			device.AddFeature(CommonUsages.devicePosition, positionCallback);
			device.AddFeature(CommonUsages.deviceRotation, rotationCallback);
			device.AddFeature(CommonUsages.centerEyePosition, positionCallback);
			device.AddFeature(CommonUsages.centerEyeRotation, rotationCallback);
			device.AddFeature(CommonUsages.leftEyePosition, leftEyePositionCallback ?? positionCallback);
			device.AddFeature(CommonUsages.leftEyeRotation, leftEyeRotationCallback ?? rotationCallback);
			device.AddFeature(CommonUsages.rightEyePosition, rightEyePositionCallback ?? positionCallback);
			device.AddFeature(CommonUsages.rightEyeRotation, rightEyeRotationCallback ?? rotationCallback);
			return device;
		}
		
		public static MockInputDevice CreateRightController(Func<bool> isTrackedCallback, Func<Vector3> positionCallback, Func<Quaternion> rotationCallback, Func<InputTrackingState> stateCallback = null)
		{
			return CreateController(XRNode.RightHand, isTrackedCallback, positionCallback, rotationCallback, stateCallback);
		}
		
		public static MockInputDevice CreateLeftController(Func<bool> isTrackedCallback, Func<Vector3> positionCallback, Func<Quaternion> rotationCallback, Func<InputTrackingState> stateCallback = null)
		{
			return CreateController(XRNode.LeftHand, isTrackedCallback, positionCallback, rotationCallback, stateCallback);
		}
		
		private static MockInputDevice CreateController(XRNode node, Func<bool> isTrackedCallback, Func<Vector3> positionCallback, Func<Quaternion> rotationCallback, Func<InputTrackingState> stateCallback = null)
		{
			var device = new MockInputDevice("<XRController>",
				InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand,
				node,
				nameof(XRController));
			device.AddFeature(CommonUsages.isTracked, isTrackedCallback);
			device.AddFeature(CommonUsages.trackingState, stateCallback ?? (() => InputTrackingState.Position | InputTrackingState.Rotation));
			device.AddFeature(CommonUsages.devicePosition, positionCallback);
			device.AddFeature(CommonUsages.deviceRotation, rotationCallback);
			return device;
		}
	}
}