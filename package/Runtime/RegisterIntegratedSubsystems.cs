using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
// ReSharper disable UnusedMember.Local

#if UNITY_2020_2_OR_NEWER
using UnityEngine.SubsystemsImplementation;
#endif

namespace needle.weaver.webxr
{
	public static class RegisterIntegratedSubsystems
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Init()
		{
			if (Application.isPlaying && Application.isEditor)
			{
				Debug.Log("Would inject subsystem in webgl build but not in editor");
				return;
			}
			
#if !UNITY_EDITOR && UNITY_WEBGL
			RegisterSubsystems();
			RegisterDescriptors();
#endif
		}

		private static IEnumerable<IntegratedSubsystem> Subsystems()
		{
			yield return XRInputSubsystem_Patch.Instance;
			yield return XRDisplaySubsystem_Patch.Instance;
		}

		private static void RegisterSubsystems()
		{
#if UNITY_2020_2_OR_NEWER
			var type = typeof(SubsystemManager);
#else
			var type = Type.GetType("UnityEngine.Internal_SubsystemInstances");
#endif
			var field = type.GetField("s_IntegratedSubsystems", (BindingFlags) ~0);
			if (field == null) Debug.LogError("Could not get integrated subsystems list");
			var list = field?.GetValue(null) as List<IntegratedSubsystem>;
			list?.AddRange(Subsystems());

			var ml = new List<ISubsystem>();
			SubsystemManager.GetInstances(ml);
			if (ml.Count <= 0)
			{
				Debug.LogError("Failed adding Subsystem for webgl support");
			}
			else
			{
				if(Debug.isDebugBuild) Debug.Log($"Registered subsystems successfully:\n" + string.Join("\n", Subsystems()));
			}
		}

		private static void RegisterDescriptors()
		{
#if UNITY_2020_2_OR_NEWER
			var descriptorStore = typeof(SubsystemDescriptorStore);
			var list = descriptorStore?.GetField("s_IntegratedDescriptors", (BindingFlags) ~0)?.GetValue(null) as List<IntegratedSubsystemDescriptor>;
#else
			var descriptorStore = Type.GetType("UnityEngine.Internal_SubsystemDescriptors");
			var list = descriptorStore?.GetField("s_IntegratedSubsystemDescriptors", (BindingFlags) ~0)?.GetValue(null) as List<IntegratedSubsystemDescriptor>;
#endif
			if (list != null)
			{
				foreach (var sub in Subsystems())
				{
					bool Add(IntegratedSubsystemDescriptor desc)
					{
						if(desc != null)
						{
							if(Debug.isDebugBuild) Debug.Log("Registered Descriptor: " + desc + ", id=" + desc.id);
							list.Add(desc);
							// check if actually added:
							var inList = new List<IntegratedSubsystemDescriptor>();
							SubsystemManager.GetSubsystemDescriptors(inList);
							if (!inList.Contains(desc))
							{
								Debug.LogError("Failed adding subsystem descriptor");
								return false;
							}
							if(Debug.isDebugBuild) Debug.Log("Successfully added " + desc.id);
							return true;
						}
						if(Debug.isDebugBuild) Debug.LogError("Descriptor is null");
						return false;
					}

					switch (sub)
					{
						case XRInputSubsystem_Patch xi when Add(xi.SubsystemDescriptor):
							break;
						case XRDisplaySubsystem_Patch dp when Add(dp.SubsystemDescriptor):
							break;
						default:
							Debug.LogError("Could not add subsystem descriptor for " + sub);
							break;
					}
				}
			}
			else Debug.LogError("Failed getting integrated subsystem descriptors list");

			if (list?.Count <= 0)
			{
				if(Debug.isDebugBuild) Debug.LogWarning("No subsystem descriptors added");
			}
		}
	}
}