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
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log("Search descriptor binding: " + descriptorPtr);
#endif

			foreach (var man in ManagedBinding.Instances)
			{
				if (man == null) continue;
				if (man.DescriptorPointer == descriptorPtr)
				{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
					Debug.Log("Found Descriptor Binding: " + descriptorPtr + " -> " + man);
#endif
					return man.SubsystemPointer;
				}
			}

#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.LogError("Could not find binding for descriptor: " + descriptorPtr);
#endif
			return IntPtr.Zero;
		}
	}
}