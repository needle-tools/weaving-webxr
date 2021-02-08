// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Rendering;
// using UnityEngine.XR;
//
// namespace needle.weaver.webxr
// {
// 	public class RenderAR : DisplaySubsystemBehaviourBase
// 	{
// 		private Camera camera;
// 		private RenderTexture target;
// 		
// 		public override void Dispose()
// 		{
// 			if(target && target.IsCreated()) target.Release(); 
// 			base.Dispose();
//
// 			if (camera)
// 			{
// 				Object.Destroy(camera.gameObject);
// 				camera = null;
// 			}
// 		}
//
// 		public override void OnAttach(IDisplayDataProvider prov)
// 		{
// 			base.OnAttach(prov);
// 			if (camera) camera.enabled = true;
// 		}
//
// 		public override void OnDetach(IDisplayDataProvider prov)
// 		{
// 			base.OnDetach(prov);
// 			if (camera) camera.enabled = false;
// 		}
//
// 		protected override void SetupRenderTarget(RenderTexture texture)
// 		{
// 			texture.dimension = TextureDimension.Tex2D;
// 			texture.format = RenderTextureFormat.ARGB32;
// 		}
//
// 		public override bool displayOpaque => false;
//
// 		public override XRDisplaySubsystem.XRMirrorViewBlitDesc GetMirrorViewBlitDesc()
// 		{
// 			return new XRDisplaySubsystem.XRMirrorViewBlitDesc {blitParamsCount = 1};
// 		}
//
// 		public override XRDisplaySubsystem.TextureLayout textureLayout => XRDisplaySubsystem.TextureLayout.SingleTexture2D;
//
// 		public override void OnGetBlitParameter(int blitParameterIndex, out XRDisplaySubsystem.XRBlitParams blitParameter)
// 		{
// 			EnsureTextures();
// 			var bp = new XRDisplaySubsystem.XRBlitParams();
// 			bp.srcRect = new Rect(0, 0, 1, 1);
// 			bp.destRect = new Rect(0, 0, 1, 1);
// 			bp.srcTex = target;
// 			bp.srcTexArraySlice = 0;
// 			blitParameter = bp;
// 		}
//
//
// 		protected override void EnsureTextures()
// 		{
// 			target = EnsureTexture(target, nameof(RenderAR), Screen.width, Screen.height);
// 			if (!camera && MainCamera)
// 			{
// 				var go = new GameObject("RenderAR Camera");
// 				go.transform.SetParent(MainCamera.transform, false);
// 				camera = go.AddComponent<Camera>();
// 				camera.clearFlags = CameraClearFlags.Nothing;
// 				camera.targetTexture = target;
// 			}
// 		}
// 	}
// }