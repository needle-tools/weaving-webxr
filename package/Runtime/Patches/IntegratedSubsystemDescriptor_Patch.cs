using System;
using System.Reflection;
using System.Runtime.InteropServices;
using needle.Weaver;
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

		public string id
		{
			get
			{
				if (XRInputSubsystem_Patch.IsDescriptorId(m_Ptr)) return XRInputSubsystem_Patch.DescriptorId;

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