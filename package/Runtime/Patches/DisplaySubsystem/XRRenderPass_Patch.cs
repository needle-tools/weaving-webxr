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
			renderParameter.view = camera.worldToCameraMatrix;
		}

		private static int GetRenderParameterCount_Injected(
			ref XRDisplaySubsystem.XRRenderPass _unity_self)
		{
			return 2;
		}
	}

	[NeedlePatch(typeof(XRDisplaySubsystem.XRMirrorViewBlitDesc))]
	public class XRMirrorViewBlitDesc_Patch
	{
		private static RenderTexture[] src;
		
		private static void GetBlitParameter_Injected(
			ref XRDisplaySubsystem.XRMirrorViewBlitDesc _unity_self,
			int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			Debug.Log("Get blit parameter " + blitParameterIndex);
			var bp = new XRDisplaySubsystem.XRBlitParams();
			bp.srcRect = new Rect(0, 0, 1, 1);
			
			if (src == null)
			{
				src = new[] {new RenderTexture(1, 1, 0), new RenderTexture(1, 1, 0)};
				for (int i = 0; i < src.Length; i++)
				{
					src[i].Create();
					Graphics.Blit( i % 2 == 0 ? Texture2D.redTexture : Texture2D.grayTexture, src[i]);
				}
			}

			bp.destRect = new Rect(0, 0, 1 / (float)src.Length, 1);
			bp.srcTex = src[blitParameterIndex];
			blitParameter = bp;
		}
	}
}