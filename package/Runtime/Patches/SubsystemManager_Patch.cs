using System.Collections.Generic;
using needle.Weaver;
using UnityEngine;

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(SubsystemManager))]
	public class SubsystemManager_Patch
	{
		internal static void StaticConstructScriptingClassMap()
		{
			Debug.Log(nameof(StaticConstructScriptingClassMap));
		}

	}
}