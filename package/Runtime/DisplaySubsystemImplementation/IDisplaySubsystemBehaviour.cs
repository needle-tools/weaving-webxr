using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	
	public interface IDisplaySubsystemBehaviour : IDisposable
	{
		void OnAttach(IDisplayDataProvider prov);
		void OnDetach(IDisplayDataProvider prov);
		
		void SetPreferredMirrorBlitMode(int blitMode);
		RenderTexture GetRenderTextureForRenderPass(int renderPass);
		void OnSetMSAALevel(int level);
		void SetFocusPlane_Injected(
			ref Vector3 point,
			ref Vector3 normal,
			ref Vector3 velocity);

		XRDisplaySubsystem.XRMirrorViewBlitDesc GetMirrorViewBlitDesc();
		
		bool TryGetCullingParams(Camera camera, int cullingPassIndex, out ScriptableCullingParameters scriptableCullingParameters);
		bool TryGetRenderPass(int renderPassIndex, out XRDisplaySubsystem.XRRenderPass renderPass);
		int GetRenderPassCount();
#if UNITY_2020_2_OR_NEWER
		XRDisplaySubsystem.TextureLayout textureLayout { get; }
#endif
		float scaleOfAllRenderTargets { get; }
		float scaleOfAllViewports { get; }
		bool displayOpaque { get; }

		
		//  XRMirrorViewBlitDesc
		void OnGetBlitParameter(
			int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter);

		// render pass
		void OnGetRenderParameter(
			ref XRDisplaySubsystem.XRRenderPass pass, 
			Camera camera, 
			int renderParameterIndex, 
			out XRDisplaySubsystem.XRRenderParameter renderParameter);

		int OnGetRenderParameterCount(ref XRDisplaySubsystem.XRRenderPass pass);
	}
}