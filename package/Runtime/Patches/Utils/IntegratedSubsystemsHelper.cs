using System.Reflection;
using UnityEngine;

namespace needle.weaver.webxr.Utils
{
	internal static class IntegratedSubsystemsHelper
	{
		public static TSubsystem CreateInstance<TSubsystem, TDescriptor>(string id)
			where TSubsystem : IntegratedSubsystem<TDescriptor>, new()
			where TDescriptor : IntegratedSubsystemDescriptor, new()
		{
			var subsystem = new TSubsystem();
			
#if !UNITY_EDITOR && UNITY_WEBGL
			var descriptor = new TDescriptor();
			var desc = ManagedDescriptor.CreateAndRegister(id, subsystem);

			var idPtr = typeof(IntegratedSubsystemDescriptor).GetField("m_Ptr", (BindingFlags) ~0);
			if (idPtr != null)
			{
				idPtr.SetValue(descriptor, desc.IdPointer);
			}
			else Debug.LogError("Could not set descriptor id string pointer " + subsystem);

			var ptrField = typeof(IntegratedSubsystem).GetField("m_Ptr", (BindingFlags) ~0);
			if (ptrField != null)
			{
				ptrField.SetValue(subsystem, desc.SubsystemPointer);
			}
			else Debug.LogError("Could not set instance pointer for " + subsystem);

			var descriptorField = typeof(IntegratedSubsystem).GetField("m_SubsystemDescriptor", (BindingFlags) ~0);
			if (descriptorField != null)
			{
				descriptorField.SetValue(subsystem, descriptor);
			}
			else Debug.LogError("Could not set descriptor for " + subsystem);
#endif

			return subsystem;
		}
	}
}