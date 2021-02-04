using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace needle.weaver.webxr.Utils
{
	public class ManagedBinding : IDisposable
	{
		internal static readonly List<ManagedBinding> Instances = new List<ManagedBinding>();

		public static bool TryGetInstance(string id, out ManagedBinding instance)
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

		public static ManagedBinding CreateAndRegister(string id, ISubsystem instance, ISubsystemDescriptor descriptor)
		{
			var handle = new ManagedBinding(id, instance, descriptor);
			Instances.Add(handle);
#if DEVELOPMENT_BUILD
			Debug.Log("Register managed instance " + handle);
#endif
			return handle;
		}

		private readonly string Id;
		private readonly ISubsystem Subsystem;
		private readonly ISubsystemDescriptor Descriptor;

		private GCHandle IdHandle { get; }
		private GCHandle SubsystemHandle, DescriptorHandle;

		// public IntPtr IdPointer => IdHandle.IsAllocated ? IdHandle.AddrOfPinnedObject() : IntPtr.Zero;
		public IntPtr SubsystemPointer => SubsystemHandle.IsAllocated ? GCHandle.ToIntPtr(SubsystemHandle) : IntPtr.Zero;
		public IntPtr DescriptorPointer => DescriptorHandle.IsAllocated ? GCHandle.ToIntPtr(DescriptorHandle) : IntPtr.Zero;

		public bool TryGetDescriptorId(IntPtr ptr, out string id)
		{
			if (IdHandle.IsAllocated && ptr == DescriptorPointer)
			{
				id = Id;
				return true;
			}

			id = null;
			return false;
		}

		private ManagedBinding(string id, ISubsystem subsystem, ISubsystemDescriptor descriptor)
		{
			Id = id;
			IdHandle = GCHandle.Alloc(id, GCHandleType.Pinned);
			Subsystem = subsystem;
			SubsystemHandle = GCHandle.Alloc(subsystem);
			Descriptor = descriptor;
			DescriptorHandle = GCHandle.Alloc(Descriptor);
		}

		public void Dispose()
		{
#if DEVELOPMENT_BUILD
			Debug.Log("Dispose " + Id);
#endif

			if (IdHandle.IsAllocated)
				IdHandle.Free();

			if (SubsystemHandle.IsAllocated)
				SubsystemHandle.Free();
		}

		public override string ToString()
		{
			return Subsystem?.GetType() + " - " + Id + ", SubsystemPointer: " + SubsystemPointer + ", DescriptorPointer: " + DescriptorPointer;
		}
	}
}