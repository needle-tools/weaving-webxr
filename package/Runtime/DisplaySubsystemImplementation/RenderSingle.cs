// using UnityEngine;
// using UnityEngine.Rendering;
// using UnityEngine.XR;
//
// namespace needle.weaver.webxr
// {
// 	public class RenderSingle : IDisplaySubsystemBehaviour
// 	{
// 		public void Dispose()
// 		{
// 			
// 		}
//
// 		public void OnAttach(IDisplayDataProvider prov)
// 		{
// 		}
//
// 		public void OnDetach(IDisplayDataProvider prov)
// 		{
// 		}
//
// 		public void SetPreferredMirrorBlitMode(int blitMode)
// 		{
// 		}
//
// 		public RenderTexture GetRenderTextureForRenderPass(int renderPass)
// 		{
// 		}
//
// 		public void SetMSAALevel(int level)
// 		{
// 		}
//
// 		public void SetFocusPlane_Injected(ref Vector3 point, ref Vector3 normal, ref Vector3 velocity)
// 		{
// 		}
//
// 		public XRDisplaySubsystem.XRMirrorViewBlitDesc GetMirrorViewBlitDesc()
// 		{
// 		}
//
// 		public bool TryGetCullingParams(Camera camera, int cullingPassIndex, out ScriptableCullingParameters scriptableCullingParameters)
// 		{
// 		}
//
// 		public bool TryGetRenderPass(int renderPassIndex, out XRDisplaySubsystem.XRRenderPass renderPass)
// 		{
// 		}
//
// 		public int GetRenderPassCount()
// 		{
// 		}
//
// 		public XRDisplaySubsystem.TextureLayout textureLayout { get; }
// 		public float scaleOfAllRenderTargets { get; }
// 		public float scaleOfAllViewports { get; }
// 		public bool displayOpaque { get; }
// 		public void OnGetBlitParameter(int blitParameterIndex, out XRDisplaySubsystem.XRBlitParams blitParameter)
// 		{
// 		}
//
// 		public void OnGetRenderParameter(ref XRDisplaySubsystem.XRRenderPass pass, Camera camera, int renderParameterIndex, out XRDisplaySubsystem.XRRenderParameter renderParameter)
// 		{
// 		}
//
// 		public int OnGetRenderParameterCount(ref XRDisplaySubsystem.XRRenderPass pass)
// 		{
// 		}
// 	}
// }