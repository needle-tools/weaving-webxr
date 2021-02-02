using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

namespace needle.weaver.webxr.Utils
{
	public class ManagedDescriptor : IDisposable
	{
		internal readonly static List<ManagedDescriptor> Instances = new List<ManagedDescriptor>();

		public static bool TryGetInstance(string id, out ManagedDescriptor instance)
		{
			foreach (var inst in Instances)
			{
				if (inst.Id == id)
				{
					instance = inst;
					return true;
				}
			}

			instance = null;
			return false;
		}

		public static ManagedDescriptor CreateAndRegister(string id, ISubsystem instance)
		{
			var handle = new ManagedDescriptor(id, instance);
			Instances.Add(handle);
			Debug.Log("Register managed instance " + handle);
			return handle;
		}

		private readonly string Id;
		private readonly ISubsystem Instance;

		private GCHandle IdHandle { get; }
		private GCHandle SubsystemHandle { get; }

		public IntPtr IdPointer => IdHandle.IsAllocated ? IdHandle.AddrOfPinnedObject() : IntPtr.Zero;
		public IntPtr SubsystemPointer => SubsystemHandle.IsAllocated ? GCHandle.ToIntPtr(SubsystemHandle) : IntPtr.Zero;

		public bool TryGetDescriptorId(IntPtr ptr, out string id)
		{
			// Debug.Log("Try get id " + ptr + " / " + IdPointer + ", " + IdHandle.IsAllocated);
			if (IdHandle.IsAllocated && ptr == IdPointer)
			{
				id = Id;
				return true;
			}

			id = null;
			return false;
		}

		private ManagedDescriptor(string id, ISubsystem subsystem)
		{
			Id = id;
			IdHandle = GCHandle.Alloc(id, GCHandleType.Pinned);
			Instance = subsystem;
			SubsystemHandle = GCHandle.Alloc(subsystem);
		}

		public void Dispose()
		{
			if (IdHandle.IsAllocated)
				IdHandle.Free();

			if (SubsystemHandle.IsAllocated)
				SubsystemHandle.Free();
		}

		public override string ToString()
		{
			return Instance?.GetType() + " - " + Id + ", IdPointer: " + IdPointer + ", SubsystemPointer: " + SubsystemPointer;
		}
	}
}