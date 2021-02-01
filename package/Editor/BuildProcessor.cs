#if UNITY_WEBGL
using needle.Weaver;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace needle.weaver.webxr
{
	internal class BuildProcessor : IPreprocessBuildWithReport
	{
		public int callbackOrder => 1000;
		public void OnPreprocessBuild(BuildReport report)
		{
			Actions.AddEmScriptArgs();
			
			if(WeaverSettings.instance.PatchOnBuild)
				Actions.PatchAssemblies();
		}
	}
}
#endif