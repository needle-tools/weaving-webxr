using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using needle.Weaver;
using UnityEngine;
using UnityEngine.XR;

// disable hide member warning
#pragma warning disable 108,114

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(UnityEngine.XR.XRInputSubsystem))]
	internal class XRInputSubsystem_Patch : UnityEngine.XR.XRInputSubsystem
	{
		private static readonly Lazy<XRInputSubsystem_Patch> _instance = new Lazy<XRInputSubsystem_Patch>(() => new XRInputSubsystem_Patch());
		public static XRInputSubsystem_Patch Instance => _instance.Value;

		internal static TrackingOriginModeFlags SupportedTrackingOriginMode = TrackingOriginModeFlags.Floor;

		internal static readonly List<MockInputDevice> InputDevices = new List<MockInputDevice>();
		internal static MockInputDevice TryGetDevice(ulong id) => InputDevices.FirstOrDefault(d => d.Id == id);


		private uint Index { get; set; }
		internal uint GetIndex() => Index;

		private ISubsystemDescriptor m_SubsystemDescriptor;
		
		
		internal bool valid => true;

		internal bool IsRunning() => isRunning;

		private bool isRunning;

		public void Start()
		{
			Debug.Log("Starting " + this);
			isRunning = true;
			m_SubsystemDescriptor = new XRInputSubsystemDescriptor();
		}

		public void Stop()
		{
			isRunning = false;
			Debug.Log("Stopping");
		}
		
		public void Destroy()
		{
			
		}
		
		// this is used to build the "InputDevice" list, called from InputDevices
		internal void TryGetDeviceIds_AsList(List<ulong> deviceIds)
		{
			// it is very important that the order is the same
			foreach (var dev in InputDevices) 
				deviceIds.Add(dev.Id);
		}

		public bool TryRecenter()
		{
			// TODO: implement
			return true;
		}


		private TrackingOriginModeFlags currentTrackingMode = TrackingOriginModeFlags.Device;
		
		public bool TrySetTrackingOriginMode(TrackingOriginModeFlags origin)
		{
			currentTrackingMode = origin;
			return true;
		}

		public TrackingOriginModeFlags GetTrackingOriginMode() => currentTrackingMode;

		public TrackingOriginModeFlags GetSupportedTrackingOriginModes() => SupportedTrackingOriginMode;

		private bool TryGetBoundaryPoints_AsList(List<Vector3> boundaryPoints)
		{
			// TODO implement
			// boundaryPoints.Clear();
			// boundaryPoints.AddRange();
			return true;
		}


		public event Action<UnityEngine.XR.XRInputSubsystem> trackingOriginUpdated;
		public event Action<UnityEngine.XR.XRInputSubsystem> boundaryChanged;

		private static void InvokeTrackingOriginUpdatedEvent(IntPtr internalPtr)
		{
			Instance.trackingOriginUpdated?.Invoke(Instance);
		}
		
		private static void InvokeBoundaryChangedEvent(IntPtr internalPtr)
		{
			Instance.boundaryChanged?.Invoke(Instance);
		}


	}
}