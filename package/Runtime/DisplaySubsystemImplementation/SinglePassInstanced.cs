using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class SinglePassInstanced : IDisplaySubsystemBehaviour
	{
		private RenderTexture target;
		private Camera main;
		private Camera left, right;
		private readonly List<RenderTexture> camTargets = new List<RenderTexture>();

		private RenderTexture RenderPassTexture
		{
			get
			{
				if (!target || target.width != Screen.width || target.height != Screen.height)
				{
					if (target && target.IsCreated())
					{
						target.Release();
#if DEVELOPMENT_BUILD
						Debug.Log("Resize RenderPass " + new Vector2(Screen.width, Screen.height));
#endif
					}

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
			var expectedWidth = (int) (Screen.width * .5f);
			var expectedHeight = Screen.height;

			for (var i = 0; i < 2; i++)
			{
				RenderTexture CreateNewTexture(RenderTexture prev, string name)
				{
					if (prev) prev.Release();
					var t = new RenderTexture(expectedWidth, expectedHeight, 0);
					t.name = name;
					t.Create();
#if DEVELOPMENT_BUILD
					Debug.Log("Created RenderTarget " + name + ": " + expectedWidth + "x" + expectedHeight);
#endif
					return t;
				}

				if (i >= camTargets.Count)
				{
					var tex = CreateNewTexture(null, "RT-" + i);
					camTargets.Add(tex);
				}
				else if (camTargets[i].width != expectedWidth || camTargets[i].height != expectedHeight)
				{
					camTargets[i] = CreateNewTexture(camTargets[i], "RT-" + i);
				}
			}

			void EnsureCamera(ref Camera cam, string name, float x, StereoTargetEyeMask targetEye, RenderTexture tex, Matrix4x4 projection)
			{
				if (!cam)
				{
					if (!main)
					{
						main = Camera.main;
						if (!main) main = Object.FindObjectOfType<Camera>();
						if (!main)
						{
#if DEVELOPMENT_BUILD
							Debug.Log("No camera found");
#endif
							return;
						}
					}

					var go = new GameObject(name);
					go.transform.SetParent(main.transform, false);
					go.transform.localPosition = new Vector3(x, 0, 0);
					go.transform.localRotation = Quaternion.identity;
					cam = go.AddComponent<Camera>();
					cam.fieldOfView = 50;
					cam.stereoTargetEye = targetEye;
#if DEVELOPMENT_BUILD
					Debug.Log("Created eye camera " + name, go);
#endif
				}

#if DEVELOPMENT_BUILD
				Debug.Log(projection);
#endif

				// if (projection == Matrix4x4.zero)
				// {
				// 	projection = main ? main.projectionMatrix : Matrix4x4.identity;
				// }
				if (projection != Matrix4x4.zero)
					cam.projectionMatrix = projection;
				cam.targetTexture = tex;
			}

			if (provider == null) return;
			EnsureCamera(ref left, "LeftEye", -0.032f, StereoTargetEyeMask.Left, camTargets[0], provider.ProjectionLeft);
			EnsureCamera(ref right, "RightEye", 0.032f, StereoTargetEyeMask.Right, camTargets[1], provider.ProjectionRight);
		}

		private IDisplayDataProvider provider;

		public void OnAttach(IDisplayDataProvider prov)
		{
			this.provider = prov;
			EnsureTextures();
		}

		public void OnDetach(IDisplayDataProvider prov)
		{
			if (prov != provider) return;
			provider = null;
		}

		public void Dispose()
		{
			if (target) target.Release();
			target = null;
			foreach (var t in camTargets)
				t.Release();
			camTargets.Clear();
			if (left)
				Object.Destroy(left.gameObject);
			if (right)
				Object.Destroy(right.gameObject);
			left = null;
			right = null;
		}

		public void SetPreferredMirrorBlitMode(int blitMode)
		{
		}

		public RenderTexture GetRenderTextureForRenderPass(int renderPass) => RenderPassTexture;

		public void SetMSAALevel(int level)
		{
		}

		public void SetFocusPlane_Injected(ref Vector3 point, ref Vector3 normal, ref Vector3 velocity)
		{
		}

		public XRDisplaySubsystem.XRMirrorViewBlitDesc GetMirrorViewBlitDesc()
		{
			EnsureTextures();
			var blitCount = 2;
			if (provider == null) blitCount = 0;
			// else if (provider.ProjectionLeft == Matrix4x4.zero || provider.ProjectionRight == Matrix4x4.zero) 
			// 	blitCount = 0;
			// else if (!left || !right) blitCount = 0;

#if DEVELOPMENT_BUILD
			Debug.Log("Blits: " + blitCount);
#endif
			var outDesc = new XRDisplaySubsystem.XRMirrorViewBlitDesc {blitParamsCount = blitCount};
			return outDesc;
		}

		public bool TryGetCullingParams(Camera camera, int cullingPassIndex, out ScriptableCullingParameters scriptableCullingParameters)
		{
			return camera.TryGetCullingParameters(true, out scriptableCullingParameters);
		}

		public bool TryGetRenderPass(int renderPassIndex, out XRDisplaySubsystem.XRRenderPass renderPass)
		{
#if DEVELOPMENT_BUILD
			Debug.Log("Get render pass index " + renderPassIndex);
#endif
			renderPass = new XRDisplaySubsystem.XRRenderPass
			{
				renderTarget = RenderPassTexture,
				renderTargetDesc = RenderPassTexture.descriptor,
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

		public void OnGetBlitParameter(int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			EnsureTextures();

			var bp = new XRDisplaySubsystem.XRBlitParams();
			bp.srcRect = new Rect(0, 0, 1, 1);
			var width = .5f;
			bp.destRect = new Rect(blitParameterIndex * width, 0, width, 1);
			bp.srcTex = camTargets[blitParameterIndex];
			bp.srcTexArraySlice = blitParameterIndex;
			blitParameter = bp;
		}

		public void OnGetRenderParameter(ref XRDisplaySubsystem.XRRenderPass pass, Camera camera, int renderParameterIndex,
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

		public int OnGetRenderParameterCount(ref XRDisplaySubsystem.XRRenderPass pass) => 1;
	}
}