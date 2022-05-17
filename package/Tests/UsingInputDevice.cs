using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace needle.weaver.webxr
{
	[ExecuteInEditMode]
	public class UsingInputDevice : MonoBehaviour
	{
		private void Update()
		{
			PrintDeviceList();
		}

		private int lastFrameReceivedEvent;


		[ContextMenu(nameof(PrintDeviceList))]
		private void PrintDeviceList()
		{
			var list = new List<InputDevice>();
			InputDevices.GetDevices(list);
			var headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);


			Debug.Log("InputDevices: " + list.Count + "\n" +
			          string.Join("\n", list.Select(e => e.name + ", " + e.GetType().GetField("m_DeviceId", (BindingFlags) ~0)?.GetValue(e))));
		}
	}
}