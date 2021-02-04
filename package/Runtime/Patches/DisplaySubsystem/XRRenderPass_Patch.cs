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
			renderParameter = new XRDisplaySubsystem.XRRenderParameter();
			if (XRDisplaySubsystem_Patch.CurrentBehaviour != null)
			{
				XRDisplaySubsystem_Patch.CurrentBehaviour.OnGetRenderParameter(ref _unity_self, camera, renderParameterIndex, out renderParameter);
			}
			// else
			// {
			// 	renderParameter.projection = camera.projectionMatrix;
			// 	renderParameter.view = camera.worldToCameraMatrix;
			// 	renderParameter.viewport = new Rect(0, 0, 1, 1);
			// }
		}

		private static int GetRenderParameterCount_Injected(ref XRDisplaySubsystem.XRRenderPass _unity_self)
		{
			return XRDisplaySubsystem_Patch.CurrentBehaviour?.OnGetRenderParameterCount(ref _unity_self) ?? 1;
		}
	}

	[NeedlePatch(typeof(XRDisplaySubsystem.XRMirrorViewBlitDesc))]
	public class XRMirrorViewBlitDesc_Patch
	{
		public void GetBlitParameter(
			int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			if (XRDisplaySubsystem_Patch.CurrentBehaviour != null)
				XRDisplaySubsystem_Patch.CurrentBehaviour.OnGetBlitParameter(blitParameterIndex, out blitParameter);
			else
			{
				blitParameter = new XRDisplaySubsystem.XRBlitParams();
			}
		}
	}
}