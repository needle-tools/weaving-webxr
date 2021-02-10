using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
// ReSharper disable UnusedMember.Local
#if UNITY_EDITOR
using UnityEditor;

#endif
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
			var field = type?.GetField("s_IntegratedSubsystems", (BindingFlags) ~0);
#else
			var type = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.FirstOrDefault(t => t.FullName == "UnityEngine.Internal_SubsystemInstances");
			var field = type?.GetField("s_IntegratedSubsystemInstances", (BindingFlags) ~0);
#endif
			if (type == null) Debug.LogError("Could not get type for integrated subsystems");
			else if (field == null) Debug.LogError("Could not get field for integrated subsystems list");
			var list = field?.GetValue(null) as IList;
			if (list == null) Debug.LogError("Could not get integrated subsystems list");
			if (Application.isEditor && !Application.isPlaying) return;
			
			foreach (var sub in Subsystems())
				list?.Add(sub);

			var ml = new List<ISubsystem>();
			SubsystemManager.GetInstances(ml);
			if (ml.Count <= 0)
			{
				Debug.LogError("Failed adding Subsystem for webgl support");
			}
			else
			{
				if (Debug.isDebugBuild) Debug.Log($"Registered subsystems successfully:\n" + string.Join("\n", Subsystems()));
			}
		}

		private static void RegisterDescriptors()
		{
#if UNITY_2020_2_OR_NEWER
			var type = typeof(SubsystemDescriptorStore);
			var list = type?.GetField("s_IntegratedDescriptors", (BindingFlags) ~0)?.GetValue(null) as List<IntegratedSubsystemDescriptor>;
#else
			var type = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.FirstOrDefault(t => t.FullName == "UnityEngine.Internal_SubsystemDescriptors");
			var listField = type?.GetField("s_IntegratedSubsystemDescriptors", (BindingFlags) ~0);
			var list = listField?.GetValue(null) as IList;
#endif
			if (type == null) Debug.LogError("Could not get subsystem descriptor store");
			else if (listField == null) Debug.LogError("Failed getting integrated subsystem descriptors field");
			else if (list == null) Debug.LogError("Failed getting integrated subsystem descriptors list");

			if (Application.isEditor || !Application.isPlaying) return;

			foreach (var sub in Subsystems())
			{
				bool Add(IntegratedSubsystemDescriptor desc)
				{
					if (desc != null)
					{
						if (Debug.isDebugBuild) Debug.Log("Registered Descriptor: " + desc + ", id=" + desc.id);
						list.Add(desc);
						// check if actually added:
						var inList = new List<IntegratedSubsystemDescriptor>();
						SubsystemManager.GetSubsystemDescriptors(inList);
						if (!inList.Contains(desc))
						{
							Debug.LogError("Failed adding subsystem descriptor");
							return false;
						}

						if (Debug.isDebugBuild) Debug.Log("Successfully added " + desc.id);
						return true;
					}

					if (Debug.isDebugBuild) Debug.LogError("Descriptor is null");
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

			if (list?.Count <= 0)
			{
				if (Debug.isDebugBuild) Debug.LogWarning("No subsystem descriptors added");
			}
		}
	}
}