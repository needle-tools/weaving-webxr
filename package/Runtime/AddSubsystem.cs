using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;

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
			#if UNITY_2020_2_OR_NEWER
			var type = typeof(SubsystemManager);
			#else
			var type = Type.GetType("UnityEngine.Internal_SubsystemInstances");
			#endif
			var field = type.GetField("s_IntegratedSubsystems", (BindingFlags) ~0);
			if (field == null) Debug.LogError("Could not get integrated subsystems list");
			var list = field?.GetValue(null) as List<IntegratedSubsystem>;
			list?.Add(XRInputSubsystem_Patch.Instance);
			list?.Add(XRDisplaySubsystem_Patch.Instance);

			var ml = new List<ISubsystem>();
			SubsystemManager.GetInstances(ml);
			if (ml.Count <= 0) Debug.LogError("Failed adding Subsystem for webgl support");
			else Debug.Log("Added subsystem successfully");
		}
	}
}