using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr.Utils
{
	internal static class IntegratedSubsystemsHelper
	{
		public static TSubsystem CreateInstance<TSubsystem, TDescriptor>(string id) 
			where TSubsystem : IntegratedSubsystem<TDescriptor>, new()
			where TDescriptor : ISubsystemDescriptor, new()
		{
			var subsystem = new TSubsystem();
			var descriptor = new XRInputSubsystemDescriptor();
			var desc = ManagedDescriptor.CreateAndRegister(id, subsystem);

			var idPtr = typeof(IntegratedSubsystemDescriptor<>).GetField("m_Ptr", (BindingFlags) ~0);
			if (idPtr != null)
			{
				idPtr.SetValue(descriptor, desc.IdPointer);
			}
			
			var ptrField = typeof(IntegratedSubsystem).GetField("m_Ptr", (BindingFlags) ~0);
			if (ptrField != null)
			{
				ptrField.SetValue(subsystem, desc.SubsystemPointer);
			}
			else Debug.LogWarning("Could not set instance pointer");
			
			var descriptorField = typeof(IntegratedSubsystem).GetField("m_SubsystemDescriptor", (BindingFlags) ~0);
			if (descriptorField != null)
			{
				descriptorField.SetValue(subsystem, descriptor);
			}
			else Debug.LogWarning("Could not set descriptor");

			return subsystem;
		}
	}
}