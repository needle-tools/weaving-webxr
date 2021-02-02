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
			return Time.frameCount % 2 + 1;
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
			if (_unity_self.nativeBlitAvailable)
			{
				_unity_self.GetBlitParameter(0, out blitParameter);
				return;
			}
			
			Debug.Log("Get blit parameter " + blitParameterIndex);
			var bp = new XRDisplaySubsystem.XRBlitParams();
			bp.srcRect = new Rect(0, 0, 1, 1);
			
			if (src == null)
			{
				src = new[]
				{
					new RenderTexture((int)(Screen.width * .5f), Screen.height, 0), 
					new RenderTexture((int)(Screen.width * .5f), Screen.height, 0)
				};
				for (int i = 0; i < src.Length; i++)
				{
					src[i].Create();
					var main = Camera.main;
					var go = new GameObject("cam-" + i);
					go.transform.SetParent(main.transform, false);
					go.transform.localPosition = new Vector3(i == 0 ? -0.032f : 0.032f, 0, 0);
					go.transform.localRotation = Quaternion.identity;
					var cam = go.AddComponent<Camera>();
					cam.fieldOfView = 50;
					cam.targetTexture = src[i];
				}
			}

			var width = 1 / (float) src.Length;
			bp.destRect = new Rect(blitParameterIndex * width, 0, width, 1);
			bp.srcTex = src[blitParameterIndex];
			blitParameter = bp;
		}
	}
}