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

		private static IDisplaySubsystemBehaviour _behaviour;

		internal static IDisplaySubsystemBehaviour behaviour
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

		public new bool displayOpaque => behaviour.displayOpaque;

		public new float scaleOfAllViewports => behaviour.scaleOfAllViewports;

		public new float scaleOfAllRenderTargets => behaviour.scaleOfAllRenderTargets;


		public new TextureLayout textureLayout
		{
			get => behaviour.textureLayout;
			set => Debug.Log("Todo: check if the current behaviour is supports " + value + " and if not switch it");
		}

		public new TextureLayout supportedTextureLayouts
		{
			get
			{
				Debug.Log("Todo: loop through list of interface implementation instances and get all supported layouts");
				return behaviour.textureLayout;
			}
		}

		public new int GetRenderPassCount() => behaviour.GetRenderPassCount();

		private bool Internal_TryGetRenderPass(
			int renderPassIndex,
			out XRRenderPass renderPass)
		{
			Debug.Log("Get render pass index " + renderPassIndex);
			return behaviour.Internal_TryGetRenderPass(renderPassIndex, out renderPass);
		}

		private bool Internal_TryGetCullingParams(
			Camera camera,
			int cullingPassIndex,
			out ScriptableCullingParameters scriptableCullingParameters)
		{
			Debug.Log("Get culling index " + cullingPassIndex);
			return behaviour.Internal_TryGetCullingParams(camera, cullingPassIndex, out scriptableCullingParameters);
		}

		public new bool GetMirrorViewBlitDesc(
			RenderTexture mirrorRt,
			out XRDisplaySubsystem.XRMirrorViewBlitDesc outDesc,
			int mode)
		{
			Debug.Log("GetMirrorViewBlitDesc");
			return behaviour.GetMirrorViewBlitDesc(mirrorRt, out outDesc, mode);
		}

		private void SetFocusPlane_Injected(
			ref Vector3 point,
			ref Vector3 normal,
			ref Vector3 velocity)
		{
			behaviour.SetFocusPlane_Injected(ref point, ref normal, ref velocity);
		}

		public new void SetPreferredMirrorBlitMode(int blitMode)
		{
			Debug.Log(nameof(SetPreferredMirrorBlitMode) + ": " + blitMode);
			behaviour.SetPreferredMirrorBlitMode(blitMode);
		}

		public new RenderTexture GetRenderTextureForRenderPass(int renderPass)
		{
			Debug.Log(nameof(GetRenderTextureForRenderPass) + ": " + renderPass);
			return behaviour.GetRenderTextureForRenderPass(renderPass);
		}

		public new void SetMSAALevel(int level)
		{
			behaviour.SetMSAALevel(level);
		}
	}
}