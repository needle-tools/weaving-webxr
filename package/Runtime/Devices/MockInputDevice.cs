using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using Debug = UnityEngine.Debug;
using InputDevice = UnityEngine.InputSystem.InputDevice;

namespace needle.weaver.webxr
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
		public ulong Id { get; }
		public string Name { get; private set; }
		public string Manufacturer { get; set; } = "Needle";
		public string SerialNumber { get; set; } = "1.0.0";
		public bool DebugLog = false;
		public string Layout { get; private set; }

		public XRNode Node { get; }
		public InputDeviceCharacteristics DeviceCharacteristics { get; set; }

		private static ulong _idCounter = 0;

		public MockInputDevice(string name, InputDeviceCharacteristics characteristics, XRNode node, string layoutName)
		{
			this.Id = _idCounter++;
			this.Name = name;
			this.DeviceCharacteristics = characteristics;
			this.Node = node;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log("Created new MockDevice: " + name + ", id=" + Id);
#endif

			this.Layout = layoutName;

#if UNITY_INPUT_SYSTEM
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log("Add device " + name + " with layout " + layoutName);
#endif
			device = InputSystem.AddDevice(layoutName, name, null);
			if (node == XRNode.LeftHand)
				InputSystem.AddDeviceUsage(device, UnityEngine.InputSystem.CommonUsages.LeftHand);
			else if (node == XRNode.RightHand)
				InputSystem.AddDeviceUsage(device, UnityEngine.InputSystem.CommonUsages.RightHand);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log(name + " usages:\n" + string.Join("\n", device.usages));
#endif
#endif
		}


		public MockInputDevice Connect()
		{
#if UNITY_INPUT_SYSTEM
			InputSystem.EnableDevice(this.device);
			device.MakeCurrent();
#endif

			if (XRInputSubsystem_Patch.InputDevices.Contains(this)) return this;
			XRInputSubsystem_Patch.InputDevices.Add(this);
#if DEVELOPMENT_BUILD
			Debug.Log("Registered input device " + this.Id + " - " + this.Node);
#endif
			return this;
		}

		public MockInputDevice Disconnect()
		{
#if UNITY_INPUT_SYSTEM
			InputSystem.DisableDevice(this.device);
#endif

			if (!XRInputSubsystem_Patch.InputDevices.Contains(this)) return this;
			XRInputSubsystem_Patch.InputDevices.Remove(this);
#if DEVELOPMENT_BUILD
			Debug.Log("Removed input device " + this.Id + " - " + this.Node);
#endif
			return this;
		}

#if UNITY_INPUT_SYSTEM
		private readonly InputDevice device;
		/// <summary>
		/// cached controls, make sure to clear when changing features
		/// </summary>
		private List<(InputControl control, Delegate callback)> newInputSystemControls = null;
#endif

		/// <summary>
		/// currently only necessary for NewInputSystem
		/// </summary>
		public void UpdateDevice()
		{
#if UNITY_INPUT_SYSTEM
			using (StateEvent.From(device, out var ptr))
			{
				// check if controls have changed/never set
				if (newInputSystemControls == null)
				{
					// collect controls from registered usages
					newInputSystemControls = new List<(InputControl control, Delegate callback)>();
					foreach (var kvp in _registry)
					{
						var name = kvp.Key.name;
						var control = device.TryGetChildControl(name);
						if (control == null)
						{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
							Debug.LogWarning("Control " + name + " does not exist in " + device.layout +
							                 ". You may need to choose a different device layout or update your usages. This usage will be ignored in New Input System. Device: " +
							                 device.name);
#endif
							continue;
						}

						newInputSystemControls.Add((control, kvp.Value));
					}
#if UNITY_EDITOR || DEVELOPMENT_BUILD
					Debug.Log("Found " + newInputSystemControls.Count + " controls for " + this.Name + "\n" + string.Join("\n", newInputSystemControls));
#endif
				}

				// loop input controls to update values
				foreach (var (control, callback) in newInputSystemControls)
				{
					try
					{
						control.WriteValueFromObjectIntoEvent(ptr, callback.DynamicInvoke());
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
				}

				InputSystem.QueueEvent(ptr);
				// InputSystem.Update();
			}
#endif
		}


		public void AddFeature<T>(InputFeatureUsage<T> usage, Func<T> getValue, XRNodeUsage xrNodeUsage = null)
		{
#if UNITY_INPUT_SYSTEM
			newInputSystemControls?.Clear();
#endif

			var usg = (InputFeatureUsage) usage;
			if (!_registry.ContainsKey(usg))
			{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				if (DebugLog) Debug.Log(Name + " add feature usage: " + usg.name);
#endif
				_registry.Add(usg, getValue);
			}
			else
			{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				if (DebugLog) Debug.Log(Name + " update feature usage: " + usg.name);
#endif
				_registry[usg] = getValue;
			}

			if (xrNodeUsage == null)
				xrNodeUsage = usg.GetXRNodeUsage(getValue, Node);
			if (xrNodeUsage == null)
			{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				if (DebugLog) Debug.LogWarning("Could not derive XRNode usage from " + usage.name + ". Please provide explicitly");
#endif
				return;
			}

			if (!_xrNodeUsages.Contains(xrNodeUsage))
			{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				if (DebugLog) Debug.Log(Name + " add node usage: " + xrNodeUsage.Node + ", " + xrNodeUsage.InputType);
#endif

				_xrNodeUsages.Add(xrNodeUsage);
			}
		}


		private readonly List<XRNodeUsage> _xrNodeUsages = new List<XRNodeUsage>();
		private readonly Dictionary<InputFeatureUsage, Delegate> _registry = new Dictionary<InputFeatureUsage, Delegate>();

		public IEnumerable<(string name, Delegate callback)> EnumerateUsages()
		{
			foreach (var usg in _registry) yield return (usg.Key.name, usg.Value);
		}

		public bool TryGetUsage<T>(string name, out T value)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			if (DebugLog)
				Debug.LogFormat(LogType.Log, LogOption.None, null, Name + " - " + Node + " - Try Get Usage " + name + " - " + typeof(T));
#endif
			foreach (var kvp in _registry)
			{
				var usage = kvp.Key;
				if (usage.name == name)
				{
					try
					{
						value = (T) kvp.Value.DynamicInvoke();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
						if (DebugLog)
							Debug.Log("Value = " + value);
#endif
						return true;
					}
					catch (Exception e)
					{
						Debug.LogException(e);
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

		public void TryGetNodes(List<XRNodeState> states)
		{
			var state = new XRNodeState()
			{
				nodeType = Node,
				uniqueID = this.Id,
				tracked = true
			};
			states.Add(state);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
			if (DebugLog)
				Debug.LogFormat(LogType.Log, LogOption.None, null, "GET NODES " + Name);
#endif

			foreach (var node in _xrNodeUsages)
			{
				try
				{
					state.nodeType = node.Node;
					state.uniqueID = this.Id;
					state.tracked = true;
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

					states.Add(state);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}

#if UNITY_EDITOR || DEVELOPMENT_BUILD
			if (DebugLog)
			{
				try
				{
					var info = states.Select(s => s.nodeType + " - " + s.uniqueID + " - " + s);
					var log = string.Join("\n", info);
					Debug.Log(Name + ", " + Id + "\n" + log);
				}
				catch
				{
					// ignored
				}
			}
#endif
		}
	}
}