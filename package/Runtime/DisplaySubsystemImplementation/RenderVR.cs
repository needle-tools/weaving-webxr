using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class RenderVR : DisplaySubsystemBehaviourBase
	{
		private Camera left, right;
		private readonly List<RenderTexture> camTargets = new List<RenderTexture>();

		protected override void EnsureTextures()
		{
			var expectedWidth = (int) (Screen.width * .5f);
			var expectedHeight = Screen.height;

			for (var i = 0; i < 2; i++)
			{
				if (i >= camTargets.Count)
				{
					var tex = EnsureTexture(null, "RT-" + i, expectedWidth, expectedHeight);
					camTargets.Add(tex);
				}
				else
					camTargets[i] = EnsureTexture(camTargets[i], "RT-" + i, expectedWidth, expectedHeight);
			}


			if (!IsAttached) return;
			EnsureCamera(ref left, "LeftEye", -0.032f, StereoTargetEyeMask.Left, camTargets[0], provider.ProjectionLeft);
			EnsureCamera(ref right, "RightEye", 0.032f, StereoTargetEyeMask.Right, camTargets[1], provider.ProjectionRight);
		}


		protected override void SetupRenderTarget(RenderTexture texture)
		{
			// texture.depth = 2;
			texture.dimension = TextureDimension.Tex2DArray;
			// texture.volumeDepth = 2;
		}

		public override void OnDetach(IDisplayDataProvider prov)
		{
			if (left) left.enabled = false;
			if (right) right.enabled = false;
			base.OnDetach(prov);
		}

		public override void Dispose()
		{
			base.Dispose();
			foreach (var t in camTargets)
				t.Release();
			camTargets.Clear();
			if (left)
				Object.Destroy(left.gameObject);
			if (right)
				Object.Destroy(right.gameObject);
			left = null;
			right = null;
		}


		public override XRDisplaySubsystem.XRMirrorViewBlitDesc GetMirrorViewBlitDesc()
		{
			EnsureTextures();
			var blitCount = 2;
			if (provider == null || !left || !right) blitCount = 0;
			var outDesc = new XRDisplaySubsystem.XRMirrorViewBlitDesc {blitParamsCount = blitCount};
			return outDesc;
		}
		
		public override XRDisplaySubsystem.TextureLayout textureLayout => XRDisplaySubsystem.TextureLayout.Texture2DArray;
		
		public override void OnGetBlitParameter(int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			EnsureTextures();
			var bp = new XRDisplaySubsystem.XRBlitParams();
			bp.srcRect = new Rect(0, 0, 1, 1);
			var width = .5f;
			bp.destRect = new Rect(blitParameterIndex * width, 0, width, 1);
			bp.srcTex = camTargets[blitParameterIndex];
			bp.srcTexArraySlice = blitParameterIndex;
			blitParameter = bp;
		}

		public override void OnSetMSAALevel(int level)
		{
			foreach (var t in camTargets)
				t.antiAliasing = level;
			
			base.OnSetMSAALevel(level);
		}
	}
}