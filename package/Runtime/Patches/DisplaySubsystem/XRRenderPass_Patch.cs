using needle.Weaver;
using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(XRDisplaySubsystem.XRRenderPass))]
	public class XRRenderPass_Patch
	{
		private static void GetRenderParameter_Injected(
			ref XRDisplaySubsystem.XRRenderPass _unity_self,
			Camera camera,
			int renderParameterIndex,
			out XRDisplaySubsystem.XRRenderParameter renderParameter)
		{
#if DEVELOPMENT_BUILD
			Debug.Log("Get Render parameter " + renderParameterIndex);
#endif
			renderParameter = new XRDisplaySubsystem.XRRenderParameter();
			XRDisplaySubsystem_Patch.CurrentBehaviour.OnGetRenderParameter(ref _unity_self, camera, renderParameterIndex, out renderParameter);
		}

		private static int GetRenderParameterCount_Injected(ref XRDisplaySubsystem.XRRenderPass _unity_self)
		{
			return XRDisplaySubsystem_Patch.CurrentBehaviour.OnGetRenderParameterCount(ref _unity_self);
		}
	}

	[NeedlePatch(typeof(XRDisplaySubsystem.XRMirrorViewBlitDesc))]
	public class XRMirrorViewBlitDesc_Patch
	{
		public void GetBlitParameter(
			int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
#if DEVELOPMENT_BUILD
			Debug.Log("Get Blit parameter " + blitParameterIndex);
#endif
			XRDisplaySubsystem_Patch.CurrentBehaviour.OnGetBlitParameter(blitParameterIndex, out blitParameter);
		}
		
	}
}