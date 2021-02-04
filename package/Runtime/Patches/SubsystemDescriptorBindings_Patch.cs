using System;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;

namespace needle.weaver.webxr
{
	[NeedlePatch("UnityEngine.SubsystemDescriptorBindings")]
	public class SubsystemDescriptorBindings_Patch
	{
		// called from IntegratedSubsystemDescriptor<TSubsystem> 
		public static IntPtr Create(IntPtr descriptorPtr)
		{
			Debug.Log("Search descriptor binding: " + descriptorPtr);
			
			foreach (var man in ManagedBinding.Instances)
			{
				if (man == null) continue;
				if (man.DescriptorPointer == descriptorPtr)
				{
					#if DEVELOPMENT_BUILD	
					Debug.Log("Found Descriptor Binding: " + descriptorPtr + " -> " + man.SubsystemPointer);
					#endif
					return man.SubsystemPointer;
				}
			}
			
#if DEVELOPMENT_BUILD	
			Debug.LogError("Could not find binding for descriptor: " + descriptorPtr);
#endif
			return IntPtr.Zero;
		}
	}
}