using System;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(XRDisplaySubsystem))]
	public class XRDisplaySubsystem_Patch : XRDisplaySubsystem, ISubsystemLifecycleCallbacks
	{
		private static readonly Lazy<XRDisplaySubsystem_Patch> _instance = new Lazy<XRDisplaySubsystem_Patch>(() =>
			IntegratedSubsystemsHelper.CreateInstance<XRDisplaySubsystem_Patch, XRDisplaySubsystemDescriptor>("com.needle.webxr.display"));

		public static XRDisplaySubsystem_Patch Instance => _instance.Value;
		
		public void OnStart()
		{
		}

		public void OnStop()
		{
		}

		public void OnDestroy()
		{
		}
	}
}