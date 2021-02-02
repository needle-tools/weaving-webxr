﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;

namespace needle.weaver.webxr
{
	[NeedlePatch(typeof(IntegratedSubsystemDescriptor))]
	public class IntegratedSubsystemDescriptor_Patch
	{
#pragma warning disable 649
		private IntPtr m_Ptr;
#pragma warning restore 649

		private MethodInfo getBindingMethod;

#if DEVELOPMENT_BUILD
		private static readonly List<IntPtr> failedList = new List<IntPtr>();
#endif

		public string id
		{
			get
			{
				for (var index = ManagedDescriptor.Instances.Count - 1; index >= 0; index--)
				{
					var di = ManagedDescriptor.Instances[index];
					if (di.TryGetDescriptorId(m_Ptr, out var _id))
						return _id;
				}
				

#if DEVELOPMENT_BUILD
				if (!failedList.Contains(m_Ptr))
				{
					failedList.Add(m_Ptr);
					Debug.LogWarning("Could not find descriptor for " + m_Ptr + ". Available:\n" + string.Join("\n", ManagedDescriptor.Instances));
				}
#endif

				if (getBindingMethod == null)
				{
#if UNITY_2020_2_OR_NEWER
					var type = Type.GetType("UnityEngine.SubsystemDescriptorBindings");
#else
					var type = Type.GetType("UnityEngine.Internal_SubsystemDescriptors");
#endif
					if (type != null)
					{
						getBindingMethod = type.GetMethod("GetId", (BindingFlags) ~0, null, CallingConventions.Any, new Type[] {typeof(IntPtr)}, null);
					}
				}

				if (getBindingMethod != null) return (string) getBindingMethod.Invoke(null, new[] {(object) m_Ptr});

				return null;
			}
		}
	}
}