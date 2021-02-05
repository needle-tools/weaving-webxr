using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public abstract class DisplaySubsystemBehaviourBase : IDisplaySubsystemBehaviour
	{
		private RenderTexture target;
		private Camera _mainCamera;
		private Matrix4x4 _originalProjectionMatrix;

		protected Camera MainCamera
		{
			get
			{
				if (_mainCamera) return _mainCamera;
				_mainCamera = Camera.main;
				if (!_mainCamera) _mainCamera = Object.FindObjectOfType<Camera>();
				if (!_mainCamera)
				{
#if DEVELOPMENT_BUILD
					Debug.Log("No camera found");
#endif
					return null;
				}
				_originalProjectionMatrix = _mainCamera.projectionMatrix;
				Debug.Log(_originalProjectionMatrix);
				return _mainCamera;
			}
		}

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
					SetupRenderTarget(target);
					target.Create();
				}

				return target;
			}
		}

		protected abstract void SetupRenderTarget(RenderTexture texture);

		protected static RenderTexture EnsureTexture(RenderTexture prev, string name, int expectedWidth, int expectedHeight)
		{
			if (prev)
			{
				if (prev.width == expectedWidth && prev.height == expectedHeight) return prev;
				prev.Release();
			}

			var t = new RenderTexture(expectedWidth, expectedHeight, 24);
			t.name = name;
			t.Create();
#if DEVELOPMENT_BUILD
			Debug.Log("Created RenderTarget " + name + ": " + expectedWidth + "x" + expectedHeight);
#endif
			return t;
		}

		protected void EnsureCamera(ref Camera cam, string name, float x, StereoTargetEyeMask targetEye, RenderTexture tex, Matrix4x4 projection)
		{
			if (!cam)
			{
				if (projection == Matrix4x4.zero) return;
				
				if (!MainCamera) return;

				var go = new GameObject(name);
				go.transform.SetParent(_mainCamera.transform, false);
				go.transform.localPosition = new Vector3(x, 0, 0);
				go.transform.localRotation = Quaternion.identity;
				cam = go.AddComponent<Camera>();
				cam.fieldOfView = 50;
				cam.stereoTargetEye = targetEye;
#if DEVELOPMENT_BUILD
				Debug.Log("Created eye camera " + name, go);
#endif
			}

			if (projection != Matrix4x4.zero)
				cam.projectionMatrix = projection;
			cam.targetTexture = tex;
			cam.enabled = IsAttached;
		}


		protected IDisplayDataProvider provider;
		protected bool IsAttached => provider != null;

		public virtual void OnAttach(IDisplayDataProvider prov)
		{
			if (this.provider != null)
			{
				Debug.LogWarning("Called OnAttach while already attached: " + this + ", CurrentProvider: " + this.provider + ", attach: " + prov);
			}
			this.provider = prov;
			EnsureTextures();
		}

		public virtual void OnDetach(IDisplayDataProvider prov)
		{
			if (prov != provider) return;
			provider = null;
			
			if (_originalProjectionMatrix != Matrix4x4.zero && MainCamera)
			{
				Debug.Log("Set main camera projection matrix\n" + _originalProjectionMatrix);
				var components = MainCamera.GetComponents<Behaviour>().Where(b => !(b is Camera)).ToArray();
				var c = MainCamera.gameObject.AddComponent<ExitDisplaySubsystemHelper>();
				c.Behaviours = components;
				foreach (var comp in components)
				{
					Debug.Log("disable " + comp);
					comp.enabled = false;
				}
				c.Callback = () =>
				{
					MainCamera.projectionMatrix = _originalProjectionMatrix;
					MainCamera.fieldOfView = 60;
					MainCamera.ResetProjectionMatrix();
				};
			}
		}
		
		public virtual void Dispose()
		{
			if (target) target.Release();
			target = null;
		}
		
		public virtual void SetPreferredMirrorBlitMode(int blitMode)
		{
		}

		public virtual void OnGetRenderParameter(ref XRDisplaySubsystem.XRRenderPass pass, Camera camera, int renderParameterIndex,
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

		public virtual bool TryGetCullingParams(Camera camera, int cullingPassIndex, out ScriptableCullingParameters scriptableCullingParameters)
		{
			return camera.TryGetCullingParameters(true, out scriptableCullingParameters);
		}
		
		public virtual bool TryGetRenderPass(int renderPassIndex, out XRDisplaySubsystem.XRRenderPass renderPass)
		{
			Debug.Log("get RenderPass " + renderPassIndex);
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

		public virtual int GetRenderPassCount() => 1;
		public virtual float scaleOfAllRenderTargets => 1;
		public virtual float scaleOfAllViewports => 1;
		
		public virtual RenderTexture GetRenderTextureForRenderPass(int renderPass) => RenderPassTexture;
		public virtual int OnGetRenderParameterCount(ref XRDisplaySubsystem.XRRenderPass pass) => 1;
		public virtual bool displayOpaque => true;
		
		public virtual void SetMSAALevel(int level)
		{
		}

		public virtual void SetFocusPlane_Injected(ref Vector3 point, ref Vector3 normal, ref Vector3 velocity)
		{
		}
		

		public abstract XRDisplaySubsystem.XRMirrorViewBlitDesc GetMirrorViewBlitDesc();
		public abstract XRDisplaySubsystem.TextureLayout textureLayout { get; }
		public abstract void OnGetBlitParameter(int blitParameterIndex, out XRDisplaySubsystem.XRBlitParams blitParameter);

		protected abstract void EnsureTextures();
		
		

	}
}