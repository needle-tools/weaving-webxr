using System.Collections.Generic;
using needle.Weaver;
using UnityEngine;

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(SubsystemManager))]
	public class SubsystemManager_Patch
	{
		// public static void GetInstances<T>(List<T> instances) where T : ISubsystem
		// {
		// 	instances.Clear();
		// 	if(XRInputSubsystem_Patch.Instance is T instance)
		// 		instances.Add(instance);
		// 	else Debug.LogError("Failed adding mock subsystem");
		// }


		internal static void StaticConstructScriptingClassMap()
		{
			Debug.Log(nameof(StaticConstructScriptingClassMap));
		}

	}
}