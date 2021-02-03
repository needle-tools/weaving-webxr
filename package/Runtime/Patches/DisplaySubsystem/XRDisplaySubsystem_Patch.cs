using System;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;
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

		private static IDisplaySubsystemBehaviour _behaviour;

		internal static IDisplaySubsystemBehaviour Behaviour
		{
			get
			{
				if (_behaviour == null)
				{
					_behaviour = new SinglePassInstanced();
					_behaviour.OnAttach();
				}
				return _behaviour;
			}
		}
		
		// ------------------------ patched methods

		public new bool displayOpaque => Behaviour.displayOpaque;

		public new float scaleOfAllViewports => Behaviour.scaleOfAllViewports;

		public new float scaleOfAllRenderTargets => Behaviour.scaleOfAllRenderTargets;


		public new TextureLayout textureLayout
		{
			get => Behaviour.textureLayout;
			set => Debug.Log("Todo: check if the current behaviour is supports " + value + " and if not switch it");
		}

		public new TextureLayout supportedTextureLayouts
		{
			get
			{
				Debug.Log("Todo: loop through list of interface implementation instances and get all supported layouts");
				return Behaviour.textureLayout;
			}
		}

		internal int renderPassCount;
		public new int GetRenderPassCount() => renderPassCount;// Behaviour.GetRenderPassCount();

		private bool Internal_TryGetRenderPass(
			int renderPassIndex,
			out XRRenderPass renderPass)
		{
			Debug.Log("Get render pass index " + renderPassIndex);
			return Behaviour.TryGetRenderPass(renderPassIndex, out renderPass);
		}

		private bool Internal_TryGetCullingParams(
			Camera camera,
			int cullingPassIndex,
			out ScriptableCullingParameters scriptableCullingParameters)
		{
			Debug.Log("Get culling index " + cullingPassIndex);
			return Behaviour.TryGetCullingParams(camera, cullingPassIndex, out scriptableCullingParameters);
		}
		
		
		public new bool GetMirrorViewBlitDesc(
			RenderTexture mirrorRt,
			out XRMirrorViewBlitDesc outDesc,
			int mode)
		{
			outDesc = new XRMirrorViewBlitDesc();
			outDesc.blitParamsCount = 2;
			return true;
			// return Behaviour.GetMirrorViewBlitDesc(mirrorRt, out outDesc, mode);
		}

		private void SetFocusPlane_Injected(
			ref Vector3 point,
			ref Vector3 normal,
			ref Vector3 velocity)
		{
			Behaviour.SetFocusPlane_Injected(ref point, ref normal, ref velocity);
		}

		public new void SetPreferredMirrorBlitMode(int blitMode)
		{
			Debug.Log(nameof(SetPreferredMirrorBlitMode) + ": " + blitMode);
			Behaviour.SetPreferredMirrorBlitMode(blitMode);
		}

		public new RenderTexture GetRenderTextureForRenderPass(int renderPass)
		{
			Debug.Log(nameof(GetRenderTextureForRenderPass) + ": " + renderPass);
			return Behaviour.GetRenderTextureForRenderPass(renderPass);
		}

		public new void SetMSAALevel(int level)
		{
			Behaviour.SetMSAALevel(level);
		}
	}
}