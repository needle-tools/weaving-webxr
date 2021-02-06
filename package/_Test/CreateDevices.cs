using System;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using Utils;
using CommonUsages = UnityEngine.XR.CommonUsages;
using Random = UnityEngine.Random;

namespace needle.weaver.webxr
{
	public class CreateDevices : MonoBehaviour
	{
		private static Quaternion _rotation = Quaternion.identity;
		private List<MockInputDevice> devices = new List<MockInputDevice>();
		public bool UpdateDevices;

		private void Awake()
		{
			if (Application.isEditor) return;

			InputSystem.RegisterLayout<XRHMD>();
			InputSystem.RegisterLayout<XRController>();
			InputSystem.RegisterLayout<OpenVRControllerWMR>();

			var headset = MockDeviceBuilder.CreateHeadset(
				() => true,
				() => Vector3.LerpUnclamped(new Vector3(0, 0, -.5f), Vector3.up * .2f, Mathf.Sin(Time.time)),
				() => _rotation
			).Connect();
			devices.Add(headset);

			var rightController = MockDeviceBuilder.CreateRightController(
				() => true,
				() => Vector3.LerpUnclamped(Vector3.zero, Vector3.down * .2f, Mathf.Sin(Time.time * 2f)),
				() => Quaternion.Euler(20, 20, 20));
			rightController.AddFeature(CommonUsages.trigger, () => Random.value);
			rightController.AddFeature(CommonUsages.triggerButton, () => Random.value > .5f);
			rightController.AddFeature(CommonUsages.primary2DAxis, () => Random.insideUnitCircle);
			rightController.AddFeature(CommonUsages.primaryButton, () => Random.value > .5);
			rightController.AddFeature(CommonUsages.secondaryButton, () => Random.value > .5);
			rightController.AddFeature(CommonUsages.grip, () => Random.value);
			rightController.AddFeature(CommonUsages.gripButton, () => Random.value > .5f);
			rightController.Connect();
			devices.Add(rightController);

			var leftController = MockDeviceBuilder.CreateLeftController(
				() => true,
				() => Vector3.LerpUnclamped(Vector3.zero, Vector3.right * .5f, Mathf.Sin(Time.time * 5f)),
				() => Quaternion.identity);
			leftController.Connect();
			devices.Add(leftController);

			InputSubsystemAPI.SetSupportedTrackingMode(TrackingOriginModeFlags.Device | TrackingOriginModeFlags.Floor);
			XRDisplaySubsystem_Patch.Instance.Stop();

		}

		private void OnEnable()
		{
			PlayerLoopHelper.AddUpdateCallback(this, this.CustomUpdate, PlayerLoopHelper.Stages.PostLateUpdate);
		}

		private void OnDisable()
		{
			PlayerLoopHelper.RemoveUpdateDelegate(this, this.CustomUpdate);
		}

		private void CustomUpdate()
		{
			if (UpdateDevices)
				foreach (var dev in devices)
					dev.UpdateDevice();
		}

		private void Update()
		{
			_rotation = Quaternion.Lerp(Quaternion.Euler(0, -20, 0), Quaternion.Euler(0, 20, 0), Mathf.Sin(Time.time) * .5f + .5f);
		}
	}
}