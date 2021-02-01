using System.Collections.Generic;
using System.Reflection;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class InputDevicesInfo : DebugInfo
	{
		private readonly List<XRNodeState> nodeStates = new List<XRNodeState>();
		private FieldInfo inputDeviceIdField;
		
		public override string GetInfo()
		{
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