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
			Debug.Log("Get Render parameter " + renderParameterIndex);
			renderParameter = new XRDisplaySubsystem.XRRenderParameter();
			XRDisplaySubsystem_Patch.Behaviour.GetRenderParameter(ref _unity_self, camera, renderParameterIndex, out renderParameter);
		}

		private static int GetRenderParameterCount_Injected(ref XRDisplaySubsystem.XRRenderPass _unity_self)
		{
			return XRDisplaySubsystem_Patch.Behaviour.GetRenderParameterCount(ref _unity_self);
		}
	}

	[NeedlePatch(typeof(XRDisplaySubsystem.XRMirrorViewBlitDesc))]
	public class XRMirrorViewBlitDesc_Patch
	{
		
		public void GetBlitParameter(
			int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			Debug.Log("Get Blit parameter " + blitParameterIndex);
			XRDisplaySubsystem_Patch.Behaviour.GetBlitParameter(blitParameterIndex, out blitParameter);
		}
		
	}
}