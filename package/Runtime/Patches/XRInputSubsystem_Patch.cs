using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using needle.Weaver;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

// disable hide member warning
#pragma warning disable 108,114

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(UnityEngine.XR.XRInputSubsystem))]
	internal class XRInputSubsystem_Patch : UnityEngine.XR.XRInputSubsystem
	{
		private static GCHandle _descriptorIdPtr;
		private static GCHandle _subsystemInstanceHandle;

		internal const string DescriptorId = "com.needle.webxr.input";
		internal static bool IsDescriptorId(IntPtr ptr) => _descriptorIdPtr.IsAllocated && _descriptorIdPtr.AddrOfPinnedObject() == ptr;
		
		private static readonly Lazy<XRInputSubsystem_Patch> _instance = new Lazy<XRInputSubsystem_Patch>(() =>
		{
			var subsystem = new XRInputSubsystem_Patch();
			var descriptor = new XRInputSubsystemDescriptor();

			var idPtr = typeof(IntegratedSubsystemDescriptor).GetField("m_Ptr", (BindingFlags) ~0);
			if (idPtr != null)
			{
				_descriptorIdPtr = GCHandle.Alloc(DescriptorId, GCHandleType.Pinned);
				var ptr = _descriptorIdPtr.AddrOfPinnedObject();
				Debug.Log("ID POINTER " + ptr);
				idPtr.SetValue(descriptor, ptr);
			}
			
			var ptrField = typeof(IntegratedSubsystem).GetField("m_Ptr", (BindingFlags) ~0);
			if (ptrField != null)
			{
				_subsystemInstanceHandle = GCHandle.Alloc(subsystem);
				var ptr = GCHandle.ToIntPtr(_subsystemInstanceHandle);
				ptrField.SetValue(subsystem, ptr);
			}
			else Debug.LogWarning("Could not set instance pointer");
			
			var descriptorField = typeof(IntegratedSubsystem).GetField("m_SubsystemDescriptor", (BindingFlags) ~0);
			if (descriptorField != null)
			{
				descriptorField.SetValue(subsystem, descriptor);
			}
			else Debug.LogWarning("Could not set descriptor");
			
			return subsystem;
		});
		
		public static XRInputSubsystem_Patch Instance => _instance.Value;
		internal static readonly List<MockInputDevice> InputDevices = new List<MockInputDevice>();
		internal static MockInputDevice TryGetDevice(ulong id) => InputDevices.FirstOrDefault(d => d.Id == id);
		internal static TrackingOriginModeFlags SupportedTrackingOriginMode = TrackingOriginModeFlags.Floor | TrackingOriginModeFlags.Device;
		private static TrackingOriginModeFlags currentTrackingMode = TrackingOriginModeFlags.Device;

		internal void OnStart()
		{
			Debug.Log("OnStart");
		}

		internal void OnStop()
		{
			
		}

		internal void OnDestroy()
		{
			
		}
		
		
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