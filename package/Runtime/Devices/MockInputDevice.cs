using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;
using Debug = UnityEngine.Debug;

namespace needle.Weavers.InputDevicesPatch
{
	public class XRNodeUsage
	{
		public XRNode Node;
		public InputType InputType;
		public readonly Delegate ValueCallback;

		public XRNodeUsage(XRNode node, InputType type, Delegate @delegate)
		{
			this.Node = node;
			this.InputType = type;
			this.ValueCallback = @delegate;
		}
	}

	public class MockInputDevice
	{
		public ulong Id { get; private set; }
		public string Name { get; private set; }
		public string Manufacturer { get; set; }
		public string SerialNumber { get; set; }

		public XRNode Node { get; }
		public InputDeviceCharacteristics DeviceCharacteristics { get; set; }

		private static ulong _idCounter;

		public MockInputDevice(string name, XRNode node)
		{
			this.Id = _idCounter++;
			this.Name = name;
			this.Node = node;
		}

		public void AddFeature<T>(InputFeatureUsage<T> usage, Func<T> getValue, XRNodeUsage xrNodeUsage = null)
		{
			var usg = (InputFeatureUsage) usage;
			if (!_registry.ContainsKey(usg))
				_registry.Add(usg, getValue);
			else
				_registry[usg] = getValue;

			xrNodeUsage ??= usg.GetXRNodeUsage(getValue, Node);
			if (xrNodeUsage == null)
			{
				Debug.LogWarning("Could not derive XRNode usage from " + usage.name + ". Please provide explicitly");
				return;
			}
			if (!_xrNodeUsages.Contains(xrNodeUsage))
				_xrNodeUsages.Add(xrNodeUsage);
		}


		private readonly List<XRNodeUsage> _xrNodeUsages = new List<XRNodeUsage>();
		private readonly Dictionary<InputFeatureUsage, Delegate> _registry = new Dictionary<InputFeatureUsage, Delegate>();


		public bool TryGetUsage<T>(string name, out T value)
		{
			// Debug.Log("try Get usage " + name);
			foreach (var kvp in _registry)
			{
				var usage = kvp.Key;
				if (usage.name == name && usage.type == typeof(T))
				{
					if (kvp.Value is Func<T> callback)
					{
						try
						{
							value = callback.Invoke();
							return true;
						}
						catch (Exception e)
						{
							Debug.LogException(e);
						}
					}
				}
			}

			value = default;
			return false;
		}

		public bool TryGetUsages(List<InputFeatureUsage> list)
		{
			list.AddRange(_registry.Keys);
			return true;
		}

		public void TryGetNodes(List<XRNodeState> nodes)
		{
			var state = new XRNodeState()
			{
				nodeType = Node,
				uniqueID = this.Id,
			};
			nodes.Add(state);
			
			foreach (var node in _xrNodeUsages)
			{
				try
				{
					state.nodeType = node.Node;
					var val = node.ValueCallback.DynamicInvoke();
					switch (node.InputType)
					{
						case InputType.Unknown:
							break;
						case InputType.Position:
							state.position = (Vector3) val;
							break;
						case InputType.Rotation:
							state.rotation = (Quaternion) val;
							break;
						case InputType.Velocity:
							state.velocity = (Vector3) val;
							break;
						case InputType.AngularVelocity:
							state.angularVelocity = (Vector3) val;
							break;
						case InputType.Acceleration:
							state.acceleration = (Vector3) val;
							break;
						case InputType.AngularAcceleration:
							state.angularAcceleration = (Vector3) val;
							break;
						case InputType.Tracked:
							state.tracked = (bool) val;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					nodes.Add(state);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
				

				nodes.Add(state);
			}
		}
	}
}