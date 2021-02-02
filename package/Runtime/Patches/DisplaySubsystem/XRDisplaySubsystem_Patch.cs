using System;
using System.Collections.Generic;
using System.Security.Permissions;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.XR;

// ReSharper disable UnusedMember.Global

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(XRDisplaySubsystem))]
	public class XRDisplaySubsystem_Patch : XRDisplaySubsystem, ISubsystemLifecycleCallbacks
	{
		public static string Id => "com.needle.webxr.display";

		private static readonly Lazy<XRDisplaySubsystem_Patch> _instance = new Lazy<XRDisplaySubsystem_Patch>(() =>
			IntegratedSubsystemsHelper.CreateInstance<XRDisplaySubsystem_Patch, XRDisplaySubsystemDescriptor>(Id));

		public static XRDisplaySubsystem_Patch Instance => _instance.Value;

		public void OnStart()
		{
			Debug.Log("Started display subsystem");
		}

		public void OnStop()
		{
		}

		public void OnDestroy()
		{
		}

		private readonly Lazy<RenderTexture> RenderTexture = new Lazy<RenderTexture>(() =>
		{
			Debug.Log("Create render target");
			var rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, 0);
			rt.Create();
			return rt;
		});

		private readonly List<RenderTexture> rts = new List<RenderTexture>();

		// ------------------------ patched methods

		public new bool displayOpaque => true;

		public new float scaleOfAllViewports => 1;

		public new float scaleOfAllRenderTargets => 1;


		public new TextureLayout textureLayout { get; set; } = TextureLayout.Texture2DArray;

		public new TextureLayout supportedTextureLayouts => TextureLayout.Texture2DArray;

		public new int GetRenderPassCount() => 1;

		public static Texture2DArray array;
		private bool Internal_TryGetRenderPass(
			int renderPassIndex,
			out XRRenderPass renderPass)
		{
			Debug.Log("Get render pass index " + renderPassIndex);
			renderPass = new XRRenderPass();
			
			if(rts.Count <= 0)
			// if (renderPassIndex >= rts.Count)
			{
				var rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, 0);
				rt.depth = 2;
				rt.dimension = TextureDimension.Tex2DArray;
				rt.Create();
				rts.Add(rt);
			}

			renderPass.renderTarget = rts[0];
			renderPass.renderTargetDesc = rts[0].descriptor;
			renderPass.shouldFillOutDepth = true;
			renderPass.cullingPassIndex = 0;
			renderPass.renderPassIndex = renderPassIndex;
			return true;
		}

		private bool Internal_TryGetCullingParams(
			Camera camera,
			int cullingPassIndex,
			out ScriptableCullingParameters scriptableCullingParameters)
		{
			Debug.Log("Get culling index " + cullingPassIndex);
			camera.TryGetCullingParameters(out scriptableCullingParameters);
			return true;
		}

		public new bool GetMirrorViewBlitDesc(
			RenderTexture mirrorRt,
			out XRDisplaySubsystem.XRMirrorViewBlitDesc outDesc,
			int mode)
		{
			Debug.Log("GetMirrorViewBlitDesc");
			outDesc = new XRMirrorViewBlitDesc();
			outDesc.blitParamsCount = 2;
			return true;
		}

		private void SetFocusPlane_Injected(
			ref Vector3 point,
			ref Vector3 normal,
			ref Vector3 velocity)
		{
		}

		private bool didLog = false;
		private float _zFar;

		public new float zFar
		{
			get => _zFar;
			set
			{
				_zFar = value;
				if (didLog) return;
				didLog = true;
				Debug.Log("set zFar " + zFar);
			}
		}

		public new void SetPreferredMirrorBlitMode(int blitMode)
		{
			Debug.Log(nameof(SetPreferredMirrorBlitMode) + ": " + blitMode);
		}


		public new RenderTexture GetRenderTextureForRenderPass(int renderPass)
		{
			if (renderPass < 0 || renderPass > rts.Count)
			{
				Debug.LogError("Requested RenderPass out of range: " + renderPass);
				return null;
			}
			Debug.Log(nameof(GetRenderTextureForRenderPass) + ": " + renderPass);
			return rts[renderPass];
		}

		public new void SetMSAALevel(int level)
		{
			Debug.Log(nameof(SetMSAALevel) + ", " + level);
		}
	}
}