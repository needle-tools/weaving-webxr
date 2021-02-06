using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class InputDevicesInfo : DebugInfo
	{
		private readonly List<XRNodeState> nodeStates = new List<XRNodeState>();
		private FieldInfo inputDeviceIdField;
		private readonly List<InputFeatureUsage> usages = new List<InputFeatureUsage>();


		public override string GetInfo()
		{
			if (!enabled) return "";
			var list = new List<InputDevice>();
			InputDevices.GetDevices(list);

			if (inputDeviceIdField == null)
			{
				inputDeviceIdField = typeof(InputDevice).GetField("m_DeviceId", (BindingFlags) ~0);
			}

			var str = list.Count + " devices";
			foreach (var dev in list)
			{
				str += "\n" + dev.name + ", id=" + inputDeviceIdField?.GetValue(dev);
				usages.Clear();
				if (dev.TryGetFeatureUsages(usages))
				{
					str += usages.Count + " usages:";
					foreach (var usage in usages)
					{
						str += "\n";
						str += usage.name;
						if (usage.type == typeof(Vector3))
						{
							if (dev.TryGetFeatureValue(usage.As<Vector3>(), out var val))
								str += ": " + val.ToString("0.00");
						}
						else if(usage.type == typeof(bool))
						{
							if (dev.TryGetFeatureValue(usage.As<bool>(), out var val))
								str += ": " + val;
						}
						else if(usage.type == typeof(float))
						{
							if (dev.TryGetFeatureValue(usage.As<float>(), out var val))
								str += ": " + val.ToString("0.00");
						}
					}
				}
				str += "\n----";
			}
			var headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
			str += "\nHead: " + headDevice.isValid + ", name: " + headDevice.name + ", serial: " + headDevice.serialNumber + ", manufacturer: " +
			       headDevice.manufacturer;

			InputTracking.GetNodeStates(nodeStates);
			str += "\n";
			str += "Found " + nodeStates.Count + " nodes";
			for (var index = 0; index < nodeStates.Count; index++)
			{
				var node = nodeStates[index];
				str += "\n[" + index + "] " + node.nodeType + "@id=" + node.uniqueID;
				// if (index < nodeStates.Count - 1) str += ", ";
			}

			return str;
		}
	}
}