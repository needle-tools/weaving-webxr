using System;
using System.Collections.Generic;
using System.Linq;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;
using UnityEngine.XR;

// disable hide member warning
#pragma warning disable 108,114

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(XRInputSubsystem))]
	public class XRInputSubsystem_Patch : XRInputSubsystem, ISubsystemLifecycleCallbacks
	{
		public static string Id => "com.needle.webxr.input";
		private static readonly Lazy<XRInputSubsystem_Patch> _instance = new Lazy<XRInputSubsystem_Patch>( () => IntegratedSubsystemsHelper.CreateInstance<XRInputSubsystem_Patch, XRInputSubsystemDescriptor>(Id));
		
		public static XRInputSubsystem_Patch Instance => _instance.Value;
		internal static readonly List<MockInputDevice> InputDevices = new List<MockInputDevice>();
		internal static MockInputDevice TryGetDevice(ulong id) => InputDevices.FirstOrDefault(d => d.Id == id);
		internal static TrackingOriginModeFlags SupportedTrackingOriginMode = TrackingOriginModeFlags.Floor | TrackingOriginModeFlags.Device;
		private static TrackingOriginModeFlags currentTrackingMode = TrackingOriginModeFlags.Device;

		private bool isRunning;
		
		public void OnStart()
		{
			isRunning = true;
		}

		public void OnStop()
		{
			isRunning = false;
		}

		public void OnDestroy()
		{
			isRunning = false;
		}

		public bool GetIsRunning() => isRunning;

		// ----------------------- patched methods:
		
		
		private static uint Index => 0;
		internal uint GetIndex() => Index;
		
		
		// this is used to build the "InputDevice" list, called from InputDevices
		internal void TryGetDeviceIds_AsList(List<ulong> deviceIds)
		{
			deviceIds.Clear();
			if (!running) return;
			// it is very important that the order is the same
			foreach (var dev in InputDevices) 
				deviceIds.Add(dev.Id);
		}

		public bool TryRecenter()
		{
			if (!running) return false;
			
			// TODO: implement
			return true;
		}

		public bool TrySetTrackingOriginMode(TrackingOriginModeFlags origin)
		{
			currentTrackingMode = origin;
			return true;
		}

		public TrackingOriginModeFlags GetTrackingOriginMode() => currentTrackingMode;

		public TrackingOriginModeFlags GetSupportedTrackingOriginModes() => SupportedTrackingOriginMode;

		private bool TryGetBoundaryPoints_AsList(List<Vector3> boundaryPoints)
		{
			if (!running) return false;
			// TODO implement
			// boundaryPoints.Clear();
			// boundaryPoints.AddRange();
			return true;
		}


		public event Action<UnityEngine.XR.XRInputSubsystem> trackingOriginUpdated;
		public event Action<UnityEngine.XR.XRInputSubsystem> boundaryChanged;

		internal static void InvokeTrackingOriginUpdatedEvent(IntPtr internalPtr)
		{
			if (!Instance.running) return;
			Instance.trackingOriginUpdated?.Invoke(Instance);
		}
		
		internal static void InvokeBoundaryChangedEvent(IntPtr internalPtr)
		{
			if (!Instance.running) return;
			Instance.boundaryChanged?.Invoke(Instance);
		}
	}
}