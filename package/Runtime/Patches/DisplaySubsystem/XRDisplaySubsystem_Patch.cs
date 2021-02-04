using System;
using System.Collections.Generic;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

// ReSharper disable UnusedMember.Global
namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(XRDisplaySubsystem))]
	public class XRDisplaySubsystem_Patch : XRDisplaySubsystem, IDisplayDataProvider, ISubsystemLifecycleCallbacks
	{
		public static string Id => "com.needle.webxr.display";

		private static readonly Lazy<XRDisplaySubsystem_Patch> _instance = new Lazy<XRDisplaySubsystem_Patch>(() =>
			IntegratedSubsystemsHelper.CreateInstance<XRDisplaySubsystem_Patch, XRDisplaySubsystemDescriptor>(Id));

		public static XRDisplaySubsystem_Patch Instance => _instance.Value;

		public static bool DebugLog;
		public Matrix4x4 ProjectionLeft { get; set; }
		public Matrix4x4 ProjectionRight { get; set; }


		public void OnStart()
		{
			if (isRunning) return;
			isRunning = true;
		}

		public void OnStop()
		{
			if (!isRunning) return;
			isRunning = false;
			DetachDisplayBehaviour();
			Debug.Log($"Stopped {this} - Behaviours:\n" + string.Join("\n", availableBehaviours) + "\n" + 
			          "Any active? " + (CurrentBehaviour != null));
		}

		public void OnDestroy()
		{
			isRunning = false;
			if (availableBehaviours != null)
			{
				foreach (var av in availableBehaviours)
				{
					av.OnDetach(this);
					av.Dispose();
				}

				availableBehaviours.Clear();
			}

			CurrentBehaviour = null;
		}

		private bool isRunning;
		public bool GetIsRunning() => isRunning;

		internal static IDisplaySubsystemBehaviour CurrentBehaviour { get; private set; }

		public static void AttachDisplayBehaviour<T>() where T : IDisplaySubsystemBehaviour, new()
		{
			if (CurrentBehaviour != null)
				if (CurrentBehaviour.GetType() == typeof(T))
					return;
			DetachDisplayBehaviour();

			// try see if any behaviour is already registered
			if (availableBehaviours != null)
			{
				foreach (var av in availableBehaviours)
				{
					if (av == null) continue;
					if (av is T)
					{
						CurrentBehaviour = av;
						Debug.Log("Attached DisplayBehaviour " + CurrentBehaviour);
					}
				}
			}

			if (availableBehaviours == null) availableBehaviours = new List<IDisplaySubsystemBehaviour>();

			if (CurrentBehaviour == null)
			{
				CurrentBehaviour = new T();
				availableBehaviours.Add(CurrentBehaviour);
				Debug.Log("Attached DisplayBehaviour new instance of " + CurrentBehaviour);
			}

			CurrentBehaviour.OnAttach(Instance);
			renderPassCount = CurrentBehaviour.GetRenderPassCount();
		}

		public static void DetachDisplayBehaviour()
		{
			if (CurrentBehaviour != null)
				Debug.Log("Detach DisplayBehaviour " + CurrentBehaviour);
			CurrentBehaviour?.OnDetach(Instance);
			CurrentBehaviour = null;
		}

		private static int renderPassCount;
		private static List<IDisplaySubsystemBehaviour> availableBehaviours;


		// ------------------------ patched methods

		public new bool displayOpaque => CurrentBehaviour?.displayOpaque ?? true;

		public new float scaleOfAllViewports => CurrentBehaviour?.scaleOfAllViewports ?? 1;

		public new float scaleOfAllRenderTargets => CurrentBehaviour?.scaleOfAllRenderTargets ?? 1;


		public new TextureLayout textureLayout => isRunning ? CurrentBehaviour?.textureLayout ?? 0 : TextureLayout.SingleTexture2D;

		public new TextureLayout supportedTextureLayouts
		{
			get
			{
				TextureLayout layout = 0;
				if (!isRunning) return layout;
				if (availableBehaviours == null) return (TextureLayout) ~0;
				foreach (var beh in availableBehaviours) layout |= beh.textureLayout;
				return layout;
			}
		}

		public new int GetRenderPassCount() => isRunning && CurrentBehaviour != null ? renderPassCount : 0;

		private bool Internal_TryGetRenderPass(
			int renderPassIndex,
			out XRRenderPass renderPass)
		{
			if (CurrentBehaviour != null)
			{
				return CurrentBehaviour.TryGetRenderPass(renderPassIndex, out renderPass);
			}

			renderPass = new XRRenderPass
			{
				shouldFillOutDepth = true,
				renderPassIndex = renderPassIndex,
				cullingPassIndex = 0,
				renderTargetDesc = new RenderTextureDescriptor(Screen.width, Screen.height)
			};
			return false;
		}

		private bool Internal_TryGetCullingParams(
			Camera camera,
			int cullingPassIndex,
			out ScriptableCullingParameters scriptableCullingParameters)
		{
			if (CurrentBehaviour != null)
			{
				return CurrentBehaviour.TryGetCullingParams(camera, cullingPassIndex, out scriptableCullingParameters);
			}

			camera.TryGetCullingParameters(out scriptableCullingParameters);

			// camera.ResetProjectionMatrix();
			// scriptableCullingParameters = new ScriptableCullingParameters();
			return true;
		}


		public new bool GetMirrorViewBlitDesc(
			RenderTexture mirrorRt,
			out XRMirrorViewBlitDesc outDesc,
			int mode)
		{
			outDesc = CurrentBehaviour.GetMirrorViewBlitDesc();
			return outDesc.blitParamsCount > 0;
		}

		private void SetFocusPlane_Injected(
			ref Vector3 point,
			ref Vector3 normal,
			ref Vector3 velocity)
		{
			CurrentBehaviour?.SetFocusPlane_Injected(ref point, ref normal, ref velocity);
		}

		public new void SetPreferredMirrorBlitMode(int blitMode)
		{
			CurrentBehaviour?.SetPreferredMirrorBlitMode(blitMode);
		}

		public new RenderTexture GetRenderTextureForRenderPass(int renderPass)
		{
			return CurrentBehaviour?.GetRenderTextureForRenderPass(renderPass);
		}

		public new void SetMSAALevel(int level)
		{
			// TODO
		}
	}
}