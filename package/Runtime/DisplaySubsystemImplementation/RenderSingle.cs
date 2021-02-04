using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class RenderSingle : DisplaySubsystemBehaviourBase
	{
		private Camera camera;
		private RenderTexture target;
		
		public override void Dispose()
		{
			if(target && target.IsCreated()) target.Release(); 
			base.Dispose();
		}

		protected override void SetupRenderTarget(RenderTexture texture)
		{
			texture.dimension = TextureDimension.Tex2D;
		}

		public override XRDisplaySubsystem.XRMirrorViewBlitDesc GetMirrorViewBlitDesc()
		{
			return new XRDisplaySubsystem.XRMirrorViewBlitDesc {blitParamsCount = 1};
		}

		public override XRDisplaySubsystem.TextureLayout textureLayout => XRDisplaySubsystem.TextureLayout.SingleTexture2D;

		public override void OnGetBlitParameter(int blitParameterIndex, out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			EnsureTextures();
			var bp = new XRDisplaySubsystem.XRBlitParams();
			bp.srcRect = new Rect(0, 0, 1, 1);
			bp.destRect = new Rect(0, 0, 1, 1);
			bp.srcTex = target;
			bp.srcTexArraySlice = blitParameterIndex;
			blitParameter = bp;
		}


		protected override void EnsureTextures()
		{
			target = EnsureTexture(target, nameof(RenderSingle), Screen.width, Screen.height);
		}
	}
}