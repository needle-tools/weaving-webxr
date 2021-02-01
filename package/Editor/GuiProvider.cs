using needle.weaver.webxr;
using Mono.Cecil;
using needle.Weaver;
using UnityEditor;
using UnityEngine;
using Actions = needle.weaver.webxr.Actions;

namespace needle.weaver.webxr
{
	internal class WebGlPatchGui : IPatchGUIProvider
	{
		[InitializeOnLoadMethod]
		private static void Init() => WeaverSettingsProvider.Register(new WebGlPatchGui());
			
		public void OnDrawGUI()
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Patch WebGL XR Assemblies"))
				Actions.PatchAssemblies();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
	}
}