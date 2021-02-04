using System;
using needle.Weaver;
using UnityEngine;

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(SubsystemManager))]
	public static class SubsystemManager_Patch
	{
		// internal static void StaticConstructScriptingClassMap()
		// {
		// }


		// // patching this method does not work: null reference when building
		// internal static IntegratedSubsystem GetIntegratedSubsystemByPtr(IntPtr ptr)
		// {
		// 	// Debug.Log("requested integrated subsystem");
		// 	return (IntegratedSubsystem)null;
		// }

	}
}