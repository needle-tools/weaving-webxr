using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using needle.Weaver;
using UnityEditor;
using UnityEngine;

// ReSharper disable StringLiteralTypo

namespace needle.weaver.webxr
{
	public static class Actions
	{
		[MenuItem(Constants.MenuItemBase + nameof(PatchAssemblies))]
		public static void PatchAssemblies()
		{
			var res = new DefaultAssemblyResolver();
			res.AddSearchDirectory(needle.Weaver.Constants.WebGLAssembliesPath);

			if (!Directory.Exists(needle.Weaver.Constants.ManualPatchingAssembliesPath))
				Directory.CreateDirectory(needle.Weaver.Constants.ManualPatchingAssembliesPath);

			var dllNames = new string[]
			{
				"UnityEngine.SubsystemsModule.dll",
				"UnityEngine.XRModule.dll",
				"UnityEngine.VRModule.dll"
			};

			var dlls = Directory.GetFiles(needle.Weaver.Constants.WebGLAssembliesPath, "*.dll", SearchOption.AllDirectories).Where(f => dllNames.Any(f.EndsWith));
			var assemblies = new HashSet<string>();
			foreach (var dll in dlls) assemblies.Add(dll);
			AssemblyWeaver.ProcessAssemblies(assemblies, res);
		}

		[MenuItem(Constants.MenuItemBase + nameof(AddEmScriptArgs))]
		public static void AddEmScriptArgs()
		{
			const string fix = "-s \"BINARYEN_TRAP_MODE='clamp'\"";
			var args = PlayerSettings.WebGL.emscriptenArgs;
			if (args.Contains(fix)) return;
			Debug.Log("Adding emscripten Arg " + fix + " to " + args);
			PlayerSettings.WebGL.emscriptenArgs += " " + fix;
			AssetDatabase.SaveAssets();
		}
	}
}