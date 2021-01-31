using System;
using System.Collections.Generic;
using needle.Weaver;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local

namespace needle.Weavers.InputDevicesPatch
{
	[NeedlePatch(typeof(InputTracking))]
	public class InputTracking_Patch
	{
		private static void GetNodeStates_Internal(List<XRNodeState> nodeStates)
		{
			var devices = XRInputSubsystem_Patch.InputDevices;
			foreach (var device in devices)
			{
				device.TryGetNodes(nodeStates);
			}
		}

		private static ulong GetDeviceIdAtXRNode(XRNode node)
		{
			var devices = XRInputSubsystem_Patch.InputDevices;
			foreach (var device in devices)
			{
				if (device.Node == node)
				{
					Debug.Log("found " + node + ", " + device.Id);
					return device.Id;
				}
			}
			Debug.Log("could not find " + node + ", " + devices);
			return 0;
		}

		internal static void GetDeviceIdsAtXRNode_Internal(XRNode node, List<ulong> deviceIds)
		{
			var devices = XRInputSubsystem_Patch.InputDevices;
			foreach (var device in devices)
			{
				if (device.Node == node)
				{
					Debug.Log("Found " + device.Id);
					deviceIds.Add(device.Id);
				}
			}
			Debug.Log(node + " found " + deviceIds.Count);
		}

		public static void Recenter()
		{
			XRInputSubsystem_Patch.Instance.TryRecenter();
		}

		public static string GetNodeName(ulong uniqueId)
		{
			var devices = XRInputSubsystem_Patch.InputDevices;
			foreach (var device in devices)
			{
				if (device.Id == uniqueId) return device.Name;
			}

			return string.Empty;
		}

		private static void GetLocalPosition_Injected(XRNode node, out Vector3 ret)
		{
			Debug.Log("TODO " + nameof(GetLocalPosition_Injected) + ", " + node);
			ret = Vector3.zero;
		}

		private static void GetLocalRotation_Injected(XRNode node, out Quaternion ret)
		{
			Debug.Log("TODO " + nameof(GetLocalRotation_Injected) + ", " + node);
			ret = Quaternion.identity;
		}

	}
}