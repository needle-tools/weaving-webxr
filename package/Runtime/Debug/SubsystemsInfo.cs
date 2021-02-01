﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class SubsystemsInfo : DebugInfo
	{
		private readonly List<ISubsystem> subs = new List<ISubsystem>();
		
		public override string GetInfo()
		{
			SubsystemManager.GetSubsystems(subs);
			var str = subs.Count + " XRInputSubsystems";
			foreach (var sub in subs)
			{
				str += "\n";
				str += sub + ", running: " + sub.running;
				
				if (sub is XRInputSubsystem xrSubsystem)
				{
					str += ", Has Descriptor? " + (xrSubsystem?.SubsystemDescriptor != null);
					str += ", ID: " + xrSubsystem?.SubsystemDescriptor?.id;
					str += ", disablesLegacyInput: " + xrSubsystem?.SubsystemDescriptor?.disablesLegacyInput;
					str += ", trackingMode: " + xrSubsystem.GetTrackingOriginMode() + ", supported trackingModes: " +
					       xrSubsystem.GetSupportedTrackingOriginModes();
				}
			}
			return str;
		}
	}
}