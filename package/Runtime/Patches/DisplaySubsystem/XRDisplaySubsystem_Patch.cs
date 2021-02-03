using System;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using Object = UnityEngine.Object;

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
			EnsureDisplayBehaviour();
		}

		public void OnStop()
		{
		}

		public void OnDestroy()
		{
		}

		private static IDisplaySubsystemBehaviour _behaviour;
		internal static IDisplaySubsystemBehaviour Behaviour
		{
			get
			{
				Instance.EnsureDisplayBehaviour();
				return _behaviour;
			}
		}

		private void EnsureDisplayBehaviour()
		{
			if (_behaviour == null)
			{
				_behaviour = new SinglePassInstanced();
				_behaviour.OnAttach();
				renderPassCount = _behaviour.GetRenderPassCount();
			}
		}

		private static int renderPassCount;

		
		// ------------------------ patched methods

		public new bool displayOpaque => Behaviour?.displayOpaque ?? true;

		public new float scaleOfAllViewports => Behaviour?.scaleOfAllViewports ?? 1;

		public new float scaleOfAllRenderTargets => Behaviour?.scaleOfAllRenderTargets ?? 1;


		public new TextureLayout textureLayout => Behaviour?.textureLayout ?? 0;

		public new TextureLayout supportedTextureLayouts
		{
			get
			{
				Debug.Log("Todo: loop through list of interface implementation instances and get all supported layouts");
				return Behaviour?.textureLayout ?? 0;
			}
		}

		public new int GetRenderPassCount() => renderPassCount;

		private bool Internal_TryGetRenderPass(
			int renderPassIndex,
			out XRRenderPass renderPass)
		{
			Debug.Log("Get render pass index " + renderPassIndex);
			if (Behaviour != null)
				return Behaviour.TryGetRenderPass(renderPassIndex, out renderPass);
			renderPass = new XRRenderPass();
			return false;
		}

		private bool Internal_TryGetCullingParams(
			Camera camera,
			int cullingPassIndex,
			out ScriptableCullingParameters scriptableCullingParameters)
		{
			Debug.Log("Get culling index " + cullingPassIndex);
			if (Behaviour != null)
			{
				return Behaviour.TryGetCullingParams(camera, cullingPassIndex, out scriptableCullingParameters);
			}
			scriptableCullingParameters = new ScriptableCullingParameters();
			return false;
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
			Behaviour?.SetFocusPlane_Injected(ref point, ref normal, ref velocity);
		}

		public new void SetPreferredMirrorBlitMode(int blitMode)
		{
			Debug.Log(nameof(SetPreferredMirrorBlitMode) + ": " + blitMode);
			Behaviour?.SetPreferredMirrorBlitMode(blitMode);
		}

		public new RenderTexture GetRenderTextureForRenderPass(int renderPass)
		{
			Debug.Log(nameof(GetRenderTextureForRenderPass) + ": " + renderPass);
			return Behaviour?.GetRenderTextureForRenderPass(renderPass);
		}

		public new void SetMSAALevel(int level)
		{
			
		}
	}
}