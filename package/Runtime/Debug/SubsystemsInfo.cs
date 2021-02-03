using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class SubsystemsInfo : DebugInfo
	{
		private readonly List<ISubsystem> subs = new List<ISubsystem>();
		
		public override string GetInfo()
		{
			SubsystemManager.GetInstances(subs);
			// SubsystemManager.GetSubsystems(subs);
			var str = subs.Count + " XRInputSubsystems";
			foreach (var sub in subs)
			{
				str += "\n";
				str += sub + ", running: " + sub.running;

				void DescriptorInfo(ISubsystemDescriptor desc)
				{
					str += ", Has Descriptor? " + (desc != null);
					str += ", ID: " + desc?.id;
				}

				if (sub is XRDisplaySubsystem dp) DescriptorInfo(dp.SubsystemDescriptor);

				if (sub is XRInputSubsystem xrSubsystem)
				{
					DescriptorInfo(xrSubsystem.SubsystemDescriptor);
					str += ", disablesLegacyInput: " + xrSubsystem?.SubsystemDescriptor?.disablesLegacyInput;
					str += ", trackingMode: " + xrSubsystem.GetTrackingOriginMode() + ", supported trackingModes: " +
					       xrSubsystem.GetSupportedTrackingOriginModes();
				}
			}
			return str;
		}

		public override void GetActions(List<ButtonAction> list)
		{
			base.GetActions(list);
			list.Add(new ButtonAction()
			{
				Name = "Toggle WebXR Subsystem",
				Callback = OnToggle
			});
		}

		private static WebXRInputManager subsystem;

		private static void OnToggle()
		{
			if (!subsystem) subsystem = FindObjectOfType<WebXRInputManager>();
			if (!subsystem) return;
			subsystem.enabled = !subsystem.enabled;
		}
	}
}