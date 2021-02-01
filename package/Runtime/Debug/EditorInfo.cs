using UnityEngine;

namespace needle.weaver.webxr
{
	public class EditorInfo : DebugInfo
	{
		public override string GetInfo()
		{
			return Application.unityVersion + ", Frame: " + Time.frameCount;
		}
	}
}