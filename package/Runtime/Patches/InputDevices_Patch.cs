using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using needle.Weaver;
using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(InputDevices))]
	public class InputDevices_Patch
	{
		private static readonly List<InputDevice> _buffer = new List<InputDevice>();
		private static IList<MockInputDevice> _inputDevices => XRInputSubsystem_Patch.InputDevices;
		private static bool running => XRInputSubsystem_Patch.Instance.running;

		public enum ConnectionChangeType : uint
		{
			Connected,
			Disconnected,
			ConfigChange,
		}

		public static void DoInvokeConnectionEvent(ulong deviceId, ConnectionChangeType change)
		{
			var m = typeof(InputDevices).GetMethod("InvokeConnectionEvent", (BindingFlags) ~0);
			m?.Invoke(null, new object[] {deviceId, change});
			if (m == null) Debug.LogError("Failed to reflect invoke connection event");
		}

		private static InputDevice GetDeviceAtXRNode(XRNode node)
		{
			if (running)
			{
				if (_buffer.Count != _inputDevices.Count) XRInputSubsystem_Patch.Instance.TryGetInputDevices(_buffer);

				for (var index = 0; index < _inputDevices.Count && index < _buffer.Count; index++)
				{
					var dev = _inputDevices[index];
					if (dev.Node == node)
					{
						return _buffer[index];
					}
				}

				Debug.LogWarning("Could not find device at " + node);
			}
			
			return new InputDevice();
		}

		private static void GetDevices_Internal(List<InputDevice> inputDevices)
		{
			XRInputSubsystem_Patch.Instance.TryGetInputDevices(inputDevices);
		}

		
		private static bool IsDeviceValid(ulong deviceId)
		{
			return _inputDevices.Any(d => deviceId == d.Id);
		}

		internal static string GetDeviceName(ulong deviceId) => XRInputSubsystem_Patch.TryGetDevice(deviceId)?.Name;

		internal static string GetDeviceManufacturer(ulong deviceId) => XRInputSubsystem_Patch.TryGetDevice(deviceId)?.Manufacturer;

		internal static string GetDeviceSerialNumber(ulong deviceId) => XRInputSubsystem_Patch.TryGetDevice(deviceId)?.SerialNumber;

		internal static InputDeviceCharacteristics GetDeviceCharacteristics(ulong deviceId) => XRInputSubsystem_Patch.TryGetDevice(deviceId).DeviceCharacteristics;
		

		internal static bool TryGetFeatureUsages(ulong deviceId, List<InputFeatureUsage> featureUsages)
		{
			if (!running) return false;
			var device = XRInputSubsystem_Patch.TryGetDevice(deviceId);
			return device != null && device.TryGetUsages(featureUsages);
		}

		internal static bool TryGetFeatureValue_bool(
			ulong deviceId,
			string usage,
			out bool value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}

		internal static bool TryGetFeatureValue_UInt32(
			ulong deviceId,
			string usage,
			out uint value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}

		internal static bool TryGetFeatureValue_float(
			ulong deviceId,
			string usage,
			out float value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}

		internal static bool TryGetFeatureValue_Vector2f(
			ulong deviceId,
			string usage,
			out Vector2 value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}

		internal static bool TryGetFeatureValue_Vector3f(
			ulong deviceId,
			string usage,
			out Vector3 value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValue_Quaternionf(
			ulong deviceId,
			string usage,
			out Quaternion value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValue_Custom(
			ulong deviceId,
			string usage,
			out byte[] value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValueAtTime_bool(
			ulong deviceId,
			string usage,
			long time,
			out bool value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValueAtTime_UInt32(
			ulong deviceId,
			string usage,
			long time,
			out uint value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValueAtTime_float(
			ulong deviceId,
			string usage,
			long time,
			out float value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValueAtTime_Vector2f(
			ulong deviceId,
			string usage,
			long time,
			out Vector2 value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValueAtTime_Vector3f(
			ulong deviceId,
			string usage,
			long time,
			out Vector3 value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValueAtTime_Quaternionf(
			ulong deviceId,
			string usage,
			long time,
			out Quaternion value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValue_XRHand(
			ulong deviceId,
			string usage,
			out Hand value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValue_XRBone(
			ulong deviceId,
			string usage,
			out Bone value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}


		internal static bool TryGetFeatureValue_XREyes(
			ulong deviceId,
			string usage,
			out Eyes value)
		{
			value = default;
			if (!running) return false;
			var dev = _inputDevices.FirstOrDefault(d => d.Id == deviceId);
			return dev != null && dev.TryGetUsage(usage, out value);
		}
	}
}