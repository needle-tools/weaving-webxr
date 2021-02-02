using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
// ReSharper disable UnusedMember.Local

namespace needle.weaver.webxr
{
	public static class AddSubsystem
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Init()
		{
#if UNITY_EDITOR
			if (Application.isPlaying)
			{
				Debug.Log("Would inject subsystem in webgl build but not in editor");
				return;
			}
#endif
			
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
			if (ml.Count <= 0) Debug.LogError("Failed adding Subsystem for webgl support");
			else
			{
				Debug.Log($"Registered subsystems successfully:\n" + string.Join("\n", Subsystems()));
			}
		}

		private static void RegisterDescriptors()
		{
			var descriptorStore = typeof(SubsystemDescriptorStore);
			var list = descriptorStore.GetField("s_IntegratedDescriptors", (BindingFlags) ~0)?.GetValue(null) as List<IntegratedSubsystemDescriptor>;
			if (list != null)
			{
				foreach (var sub in Subsystems())
				{
					bool Add(IntegratedSubsystemDescriptor desc)
					{
						if(desc != null)
						{
							Debug.Log("Registered Descriptor: " + desc + ", id=" + desc?.id);
							list.Add(desc);
							return true;
						}
						Debug.LogError("Descriptor is null");
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
				Debug.LogWarning("No subsystem descriptors added");
			}
		}
	}
}