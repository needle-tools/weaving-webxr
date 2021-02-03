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
			XRDisplaySubsystem_Patch.behaviour.GetRenderParameter_Injected(ref _unity_self, camera, renderParameterIndex, out renderParameter);
		}

		private static int GetRenderParameterCount_Injected(ref XRDisplaySubsystem.XRRenderPass _unity_self)
		{
			return XRDisplaySubsystem_Patch.behaviour.GetRenderParameterCount_Injected(ref _unity_self);
		}
	}

	[NeedlePatch(typeof(XRDisplaySubsystem.XRMirrorViewBlitDesc))]
	// ReSharper disable once ClassNeverInstantiated.Global
	public class XRMirrorViewBlitDesc_Patch
	{
		private static void GetBlitParameter_Injected(
			ref XRDisplaySubsystem.XRMirrorViewBlitDesc _unity_self,
			int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			Debug.Log("Get blit parameter " + blitParameterIndex);
			XRDisplaySubsystem_Patch.behaviour.GetBlitParameter_Injected(ref _unity_self, blitParameterIndex, out blitParameter);
		}
	}
}