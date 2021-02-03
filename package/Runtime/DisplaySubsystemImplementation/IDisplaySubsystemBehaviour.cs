using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	
	public interface IDisplaySubsystemBehaviour : IDisposable
	{
		void OnAttach();
		void OnDetach();
		
		void SetPreferredMirrorBlitMode(int blitMode);
		RenderTexture GetRenderTextureForRenderPass(int renderPass);
		void SetMSAALevel(int level);
		void SetFocusPlane_Injected(
			ref Vector3 point,
			ref Vector3 normal,
			ref Vector3 velocity);

		bool GetMirrorViewBlitDesc(RenderTexture mirrorRt, out XRDisplaySubsystem.XRMirrorViewBlitDesc outDesc, int mode);
		bool TryGetCullingParams(Camera camera, int cullingPassIndex, out ScriptableCullingParameters scriptableCullingParameters);
		bool TryGetRenderPass(int renderPassIndex, out XRDisplaySubsystem.XRRenderPass renderPass);
		int GetRenderPassCount();
		XRDisplaySubsystem.TextureLayout textureLayout { get; }
		float scaleOfAllRenderTargets { get; }
		float scaleOfAllViewports { get; }
		bool displayOpaque { get; }

		
		//  XRMirrorViewBlitDesc
		void GetBlitParameter(
			int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter);

		// render pass
		void GetRenderParameter(
			ref XRDisplaySubsystem.XRRenderPass pass, 
			Camera camera, 
			int renderParameterIndex, 
			out XRDisplaySubsystem.XRRenderParameter renderParameter);

		int GetRenderParameterCount(ref XRDisplaySubsystem.XRRenderPass pass);
	}
}