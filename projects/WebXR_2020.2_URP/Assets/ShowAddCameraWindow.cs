#if UNITY_EDITOR

using System.Runtime.InteropServices;
using MockHMD.Editor.MultiCamera;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace DefaultNamespace
{
	public static class ShowAddCameraWindow
	{
		[MenuItem("Tools/ShowMockWindow")]
		private static void Show()
		{
			var window = EditorWindow.CreateInstance<AddCameraWindow>();
			window.Show();

			var window1 = EditorWindow.CreateInstance<RenderPassTextureWindow>();
			window1.Show();
		}
	}
}

#endif