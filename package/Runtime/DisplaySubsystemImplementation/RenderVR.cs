﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class RenderVR : DisplaySubsystemBehaviourBase
	{
		// private RenderTexture srcTex;
		// private Camera left, right;
		// private readonly List<RenderTexture> camTargets = new List<RenderTexture>();

		protected override void EnsureTextures()
		{
			// EnsureTexture(srcTex, "Source", Screen.width, Screen.height);
			// var expectedWidth = (int) (Screen.width * .5f);
			// var expectedHeight = Screen.height;
			//
			// for (var i = 0; i < 2; i++)
			// {
			// 	if (i >= camTargets.Count)
			// 	{
			// 		var tex = EnsureTexture(null, "RT-" + i, expectedWidth, expectedHeight);
			// 		camTargets.Add(tex);
			// 	}
			// 	else
			// 		camTargets[i] = EnsureTexture(camTargets[i], "RT-" + i, expectedWidth, expectedHeight);
			// }
			//
			//
			// if (!IsAttached) return;
			// EnsureCamera(ref left, "LeftEye", -0.032f, StereoTargetEyeMask.Left, camTargets[0], provider.ProjectionLeft);
			// EnsureCamera(ref right, "RightEye", 0.032f, StereoTargetEyeMask.Right, camTargets[1], provider.ProjectionRight);
		}

		public override bool TryGetCullingParams(Camera camera, int cullingPassIndex, out ScriptableCullingParameters scriptableCullingParameters)
		{
			camera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, provider.ProjectionLeft);
			camera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, provider.ProjectionRight);
			camera.stereoSeparation = 0.032f;
			
			var res = base.TryGetCullingParams(camera, cullingPassIndex, out scriptableCullingParameters);
			// scriptableCullingParameters.stereoSeparationDistance = 0.032f;
			return res;
		}

		public override bool TryGetRenderPass(int renderPassIndex, out XRDisplaySubsystem.XRRenderPass renderPass)
		{
			return base.TryGetRenderPass(renderPassIndex, out renderPass);
		}


		public override int OnGetRenderParameterCount(ref XRDisplaySubsystem.XRRenderPass pass) => 2;

		public override void OnGetRenderParameter(ref XRDisplaySubsystem.XRRenderPass pass, Camera camera, int renderParameterIndex,
			out XRDisplaySubsystem.XRRenderParameter renderParameter)
		{
			renderParameter = new XRDisplaySubsystem.XRRenderParameter
			{
				projection = renderParameterIndex == 0 ? provider.ProjectionLeft : provider.ProjectionRight,
				viewport = new Rect(renderParameterIndex == 0 ? 0 : .5f, 0, .5f, 1),
				textureArraySlice = renderParameterIndex,
				view = camera.worldToCameraMatrix,
			};
		}

		protected override void SetupRenderTarget(RenderTexture texture)
		{
			// texture.depth = 2;
			// texture.dimension = TextureDimension.Tex2DArray;
			// texture.volumeDepth = 2;
			// texture.volumeDepth = 2;
		}

		public override void OnDetach(IDisplayDataProvider prov)
		{
			MainCamera.ResetStereoProjectionMatrices();
			// if (left) left.enabled = false;
			// if (right) right.enabled = false;
			base.OnDetach(prov);
		}

		public override void Dispose()
		{
			base.Dispose();
			// foreach (var t in camTargets)
			// 	t.Release();
			// camTargets.Clear();
			// if (left)
			// 	Object.Destroy(left.gameObject);
			// if (right)
			// 	Object.Destroy(right.gameObject);
			// left = null;
			// right = null;
		}

		// public override int GetRenderPassCount() => 2;

		public override void SetPreferredMirrorBlitMode(int blitMode)
		{
			Debug.Log(blitMode);
			base.SetPreferredMirrorBlitMode(blitMode);
		}

		public override XRDisplaySubsystem.XRMirrorViewBlitDesc GetMirrorViewBlitDesc()
		{
			EnsureTextures();
			var blitCount = 1;
			// if (provider == null || !left || !right) blitCount = 0;
			var outDesc = new XRDisplaySubsystem.XRMirrorViewBlitDesc
			{
				blitParamsCount = blitCount,
			};
			return outDesc;
		}

#if UNITY_2020_2_OR_NEWER
		public override XRDisplaySubsystem.TextureLayout textureLayout => XRDisplaySubsystem.TextureLayout.SingleTexture2D;
#endif

		public override void OnGetBlitParameter(int blitParameterIndex,
			out XRDisplaySubsystem.XRBlitParams blitParameter)
		{
			EnsureTextures();
			var bp = new XRDisplaySubsystem.XRBlitParams();
			bp.srcRect = new Rect(0, 0, 1, 1);
			// var x = blitParameterIndex == 0 ? 0 : .5f;
			// bp.destRect = new Rect(x, 0, .5f, 1);
			bp.destRect = new Rect(0, 0, 1, 1);
			bp.srcTexArraySlice = 0;// Time.frameCount % 2;
			bp.srcTex = RenderPassTexture;
			blitParameter = bp;
			// var width = .5f;
			// bp.destRect = new Rect(blitParameterIndex * width, 0, width, 1);
			// bp.srcTex = camTargets[blitParameterIndex];
			// bp.srcTexArraySlice = blitParameterIndex;
			// blitParameter = bp;
		}

		public override void OnSetMSAALevel(int level)
		{
			// foreach (var t in camTargets)
			// 	t.antiAliasing = level;

			base.OnSetMSAALevel(level);
		}
	}
}