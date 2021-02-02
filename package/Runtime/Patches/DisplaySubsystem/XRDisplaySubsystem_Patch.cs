using System;
using System.Security.Permissions;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(XRDisplaySubsystem))]
	public class XRDisplaySubsystem_Patch : XRDisplaySubsystem, ISubsystemLifecycleCallbacks
	{
		public static string Id => "com.needle.webxr.display";
		
		private static readonly Lazy<XRDisplaySubsystem_Patch> _instance = new Lazy<XRDisplaySubsystem_Patch>(() =>
			IntegratedSubsystemsHelper.CreateInstance<XRDisplaySubsystem_Patch, XRDisplaySubsystemDescriptor>(Id));

		public static XRDisplaySubsystem_Patch Instance => _instance.Value;
		
		public void OnStart()
		{
			Debug.Log("Started display subsystem");
		}

		public void OnStop()
		{
		}

		public void OnDestroy()
		{
		}
		
		
		// ------------------------ patched methods

		private bool didLog = false;
		public new float zFar
		{
			get { return Mathf.Lerp(2, 20, Mathf.Sin(Time.time) * .5f + .5f); }
			set
			{
				if (didLog) return;
				didLog = true;
				Debug.Log("set zfar " + zFar);
			}
		}

		public new bool disableLegacyRenderer
		{
			get
			{
				Debug.Log("get " + nameof(disableLegacyRenderer));
				return false;
			}
			set => Debug.Log("set " + nameof(disableLegacyRenderer) + " to " + value);
		}
		

		// displayList[i].disableLegacyRenderer = true;
		// displayList[i].textureLayout = XRDisplaySubsystem.TextureLayout.Texture2DArray;
		// displayList[i].sRGB = QualitySettings.activeColorSpace == ColorSpace.Linear;

		public new void SetPreferredMirrorBlitMode(int blitMode)
		{
			Debug.Log(nameof(SetPreferredMirrorBlitMode) + ": " + blitMode);
		}


		public new RenderTexture GetRenderTextureForRenderPass(int renderPass)
		{
			Debug.Log(nameof(GetRenderTextureForRenderPass) + ": " +renderPass);
			return null;
		}

		public new void SetMSAALevel(int level)
		{
			Debug.Log(nameof(SetMSAALevel) + ", " + level);
		}
		
	}
}