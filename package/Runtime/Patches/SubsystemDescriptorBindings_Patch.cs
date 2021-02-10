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
			if(Debug.isDebugBuild)
				Debug.Log("Search descriptor binding: " + descriptorPtr);

			foreach (var man in ManagedBinding.Instances)
			{
				if (man == null) continue;
				if (man.DescriptorPointer == descriptorPtr)
				{
					if(Debug.isDebugBuild)
						Debug.Log("Found Descriptor Binding: " + descriptorPtr + " -> " + man);
					return man.SubsystemPointer;
				}
			}

			if(Debug.isDebugBuild)			
				Debug.LogError("Could not find binding for descriptor: " + descriptorPtr);
			return IntPtr.Zero;
		}
	}
}