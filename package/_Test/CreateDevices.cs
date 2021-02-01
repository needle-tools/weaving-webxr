using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class CreateDevices : MonoBehaviour
	{
		private static Quaternion _rotation = Quaternion.identity;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		private static void Init()
		{
			if (Application.isEditor) return;
			
			SubsystemAPI.RegisterInputDevice(MockDeviceBuilder.CreateHeadset(
					() => true,
					() => Vector3.LerpUnclamped(new Vector3(0,0,-.5f), Vector3.up * .2f, Mathf.Sin(Time.time)),
					() => _rotation
				)
			);
			var rightController = MockDeviceBuilder.CreateRightController(
				() => true,
				() => Vector3.LerpUnclamped(Vector3.zero, Vector3.down * .2f, Mathf.Sin(Time.time * 2f)),
				() => Quaternion.Euler(20, 20, 20));
			SubsystemAPI.RegisterInputDevice(rightController);
			
			var leftController = MockDeviceBuilder.CreateLeftController(
				() => true,
				() => Vector3.LerpUnclamped(Vector3.zero, Vector3.right * .5f, Mathf.Sin(Time.time * 5f)),
				() => Quaternion.identity);
			SubsystemAPI.RegisterInputDevice(leftController);

			// leftController.DebugLog = rightController.DebugLog = true;

			SubsystemAPI.SetSupportedTrackingMode(TrackingOriginModeFlags.Device | TrackingOriginModeFlags.Floor);
			SubsystemAPI.Instance.Start();
		}

		private void Update()
		{
			_rotation = Quaternion.Lerp(Quaternion.Euler(0, -20, 0), Quaternion.Euler(0, 20, 0), Mathf.Sin(Time.time) * .5f + .5f);
		}
	}
}