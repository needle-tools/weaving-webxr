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


		public struct S1
		{
			public int val;
		}

		public struct K
		{
			public int test;
			public int val2;
		}
		
		[InitializeOnLoadMethod]
		private static void Init()
		{
			var s = (object)new S1() {val = 42};
			K k = new K();
			k = CopyStruct<K>(ref s);
			Debug.Log(k.val2);
			Debug.Log(k.test);
		}
		private static T CopyStruct<T>(ref object s1)
		{
			GCHandle handle = GCHandle.Alloc(s1, GCHandleType.Pinned);
			T typedStruct = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();
			return typedStruct;
		}
	}
}

#endif