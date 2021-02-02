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
			renderParameter.projection = camera.projectionMatrix;
			renderParameter.viewport = new Rect(0, 0, 1, 1);
			renderParameter.textureArraySlice = 0;
			renderParameter.view = Matrix4x4.identity;
		}

		private static int GetRenderParameterCount_Injected(
			ref XRDisplaySubsystem.XRRenderPass _unity_self)
		{
			return 1;
		}
	}

	[NeedlePatch(typeof(XRDisplaySubsystem.XRMirrorViewBlitDesc))]
	public class XRMirrorViewBlitDesc_Patch
	{
		private static RenderTexture src;
		
		private static void GetBlitParameter_Injected(
			ref XRDisplaySubsystem.XRMirrorViewBlitDesc _unity_self,
			int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			Debug.Log("Get blit parameter");
			var bp = new XRDisplaySubsystem.XRBlitParams();
			bp.srcRect = new Rect(0, 0, 1, 1);
			bp.destRect = new Rect(0, 0, 1, 1);
			if (!src)
			{
				src = new RenderTexture(1, 1, 0);
				src.Create();
				Graphics.Blit(Texture2D.redTexture, src);
			}
			bp.srcTex = src;
			blitParameter = bp;
		}
	}
}