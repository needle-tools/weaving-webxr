using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	internal static class DisplaySubsystemMenuItems
	{
		[MenuItem(Constants.MenuItemBase + nameof(StartXRDisplaySubsystem))]
		private static void StartXRDisplaySubsystem()
		{
			var list = new List<XRDisplaySubsystem>();
			SubsystemManager.GetInstances(list);
			foreach (var l in list)
			{
				Debug.Log("Start " + l);
				l.Start();
			}
		}
		
		[MenuItem(Constants.MenuItemBase + nameof(StopXRDisplaySubsystem))]
		private static void StopXRDisplaySubsystem()
		{
			var list = new List<XRDisplaySubsystem>();
			SubsystemManager.GetInstances(list);
			foreach (var l in list)
			{
				Debug.Log("Stop " + l);
				l.Stop();
			}

			Camera.main.stereoTargetEye = StereoTargetEyeMask.None;
		}
		
		[MenuItem(Constants.MenuItemBase + nameof(DestroyXRDisplaySubsystem))]
		private static void DestroyXRDisplaySubsystem()
		{
			var list = new List<XRDisplaySubsystem>();
			SubsystemManager.GetInstances(list);
			foreach (var l in list)
			{
				Debug.Log("Destroy " + l);
				l.Destroy();
			}
		}
	}
}