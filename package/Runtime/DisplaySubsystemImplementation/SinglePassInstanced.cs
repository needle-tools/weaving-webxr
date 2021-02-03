using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class SinglePassInstanced : IDisplaySubsystemBehaviour
	{
		public static Camera Left;
		public static Camera Right;
		
		private RenderTexture target;
		private RenderTexture[] src;

		private RenderTexture Target
		{
			get
			{
				if (!target)
				{
					target = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, 0);
					target.depth = 2;
					target.dimension = TextureDimension.Tex2DArray;
					target.Create();
				}

				return target;
			}
		}

		private void EnsureTextures()
		{
			if (src == null)
			{
				src = new[]
				{
					new RenderTexture((int) (Screen.width * .5f), Screen.height, 0),
					new RenderTexture((int) (Screen.width * .5f), Screen.height, 0)
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
					cam.stereoTargetEye = i == 0 ? StereoTargetEyeMask.Left : StereoTargetEyeMask.Right;
					cam.targetTexture = src[i];
					if (i == 0) Left = cam;
					else Right = cam;
				}
			}
		}

		public void Dispose()
		{
			if (target) target.Release();
			target = null;
		}

		public void OnAttach()
		{
		}

		public void OnDetach()
		{
		}

		public void SetPreferredMirrorBlitMode(int blitMode)
		{
		}

		public RenderTexture GetRenderTextureForRenderPass(int renderPass) => Target;

		public void SetMSAALevel(int level)
		{
		}
		
		public void SetFocusPlane_Injected(ref Vector3 point, ref Vector3 normal, ref Vector3 velocity)
		{
		}

		public bool GetMirrorViewBlitDesc(RenderTexture mirrorRt, out XRDisplaySubsystem.XRMirrorViewBlitDesc outDesc, int mode)
		{
			Debug.Log("GetMirrorViewBlitDesc");
			outDesc = new XRDisplaySubsystem.XRMirrorViewBlitDesc();
			outDesc.blitParamsCount = 2;
			return true;
		}

		public bool Internal_TryGetCullingParams(Camera camera, int cullingPassIndex, out ScriptableCullingParameters scriptableCullingParameters)
		{
			return camera.TryGetCullingParameters(true, out scriptableCullingParameters);
		}

		public bool Internal_TryGetRenderPass(int renderPassIndex, out XRDisplaySubsystem.XRRenderPass renderPass)
		{
			Debug.Log("Get render pass index " + renderPassIndex);
			renderPass = new XRDisplaySubsystem.XRRenderPass
			{
				renderTarget = Target,
				renderTargetDesc = Target.descriptor,
				shouldFillOutDepth = true,
				cullingPassIndex = 0,
				renderPassIndex = renderPassIndex
			};
			return true;
		}

		public int GetRenderPassCount() => 1;

		public XRDisplaySubsystem.TextureLayout textureLayout => XRDisplaySubsystem.TextureLayout.Texture2DArray;

		public float scaleOfAllRenderTargets => 1;
		public float scaleOfAllViewports => 1;
		public bool displayOpaque => true;

		public void GetBlitParameter_Injected(ref XRDisplaySubsystem.XRMirrorViewBlitDesc desc, int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			var bp = new XRDisplaySubsystem.XRBlitParams();
			bp.srcRect = new Rect(0, 0, 1, 1);
			var width = 1 / (float) src.Length;
			bp.destRect = new Rect(blitParameterIndex * width, 0, width, 1);
			bp.srcTex = src[blitParameterIndex];
			bp.srcTexArraySlice = blitParameterIndex;
			blitParameter = bp;
		}

		public void GetRenderParameter_Injected(ref XRDisplaySubsystem.XRRenderPass pass, Camera camera, int renderParameterIndex,
			out XRDisplaySubsystem.XRRenderParameter renderParameter)
		{
			renderParameter = new XRDisplaySubsystem.XRRenderParameter
			{
				projection = camera.projectionMatrix, 
				viewport = new Rect(0, 0, 1, 1), 
				textureArraySlice = 0, 
				view = camera.worldToCameraMatrix
			};
		}

		public int GetRenderParameterCount_Injected(ref XRDisplaySubsystem.XRRenderPass pass) => 1;
	}
}