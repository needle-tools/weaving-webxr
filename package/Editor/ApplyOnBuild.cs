#if UNITY_WEBGL
using needle.Weaver;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace needle.weaver.webxr
{
	internal class ApplyOnBuild : IPreprocessBuildWithReport
	{
		public int callbackOrder => 1000;
		public void OnPreprocessBuild(BuildReport report)
		{
			if(WeaverSettings.instance.PatchOnBuild)
				Actions.Weave_WebGL_Patches();
		}
	}
}
#endif