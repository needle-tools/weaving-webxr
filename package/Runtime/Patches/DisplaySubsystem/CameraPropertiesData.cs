using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace needle.weaver.webxr
{
	internal static class Test
	{
		public static void FieldsMatch(CameraPropertiesData source, CameraProperties result)
		{
			var t = result.GetType();
			void AssertEqual<T>(string fieldName, T expected)
			{
				var val = (T) t?.GetField(fieldName, (BindingFlags) ~0)?.GetValue(result);
				// Debug.Log("test " + fieldName + ": " + val + " ==? " + expected);
				Assert.AreEqual(val, expected, fieldName + " did not match");
			}
			var fields = result.GetType().GetFields((BindingFlags)~0);
			foreach (var field in fields)
			{
				AssertEqual(field.Name, field.GetValue(result));
			}
		}
	}
	
	// layout MUST match CameraProperties
	public struct CameraPropertiesData
	{
		public const int k_NumLayers = 32;
		public Rect screenRect;
		public Vector3 viewDir;
		public float projectionNear;
		public float projectionFar;
		public float cameraNear;
		public float cameraFar;
		public float cameraAspect;
		public Matrix4x4 cameraToWorld;
		public Matrix4x4 actualWorldToClip;
		public Matrix4x4 cameraClipToWorld;
		public Matrix4x4 cameraWorldToClip;
		public Matrix4x4 implicitProjection;
		public Matrix4x4 stereoWorldToClipLeft;
		public Matrix4x4 stereoWorldToClipRight;
		public Matrix4x4 worldToCamera;
		public Vector3 up;
		public Vector3 right;
		public Vector3 transformDirection;
		public Vector3 cameraEuler;
		public Vector3 velocity;
		public float farPlaneWorldSpaceLength;
		public uint rendererCount;
		public const int k_PlaneCount = 6;
		public byte[] m_ShadowCullPlanes; // 96
		public byte[] m_CameraCullPlanes; // 96
		public float baseFarDistance;
		public Vector3 shadowCullCenter;
		public float[] layerCullDistances; // 32;
		public int layerCullSpherical;
		public CameraCorePropertiesData coreCameraValues; // CoreCameraValues
		public uint cameraType;
		public int projectionIsOblique;
		public int isImplicitProjectionMatrix;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct CameraPropertiesMapper
	{
		[FieldOffset(0)] public CameraProperties target;
		[FieldOffset(0)] public CameraPropertiesData source;
	}
	
	// layout MUST match CameraCoreProperties
	public struct CameraCorePropertiesData
	{
		public int filterMode;
		public uint cullingMask;
		public int instanceID;
	}
	
	internal static class CameraPropertiesExtensions
	{
		public static CameraProperties Create(CameraPropertiesData source)
		{
			source.m_ShadowCullPlanes = EnsureArray(source.m_ShadowCullPlanes, 96);
			source.m_CameraCullPlanes = EnsureArray(source.m_CameraCullPlanes, 96);
			source.layerCullDistances = EnsureArray(source.layerCullDistances, 32);
			var map = new CameraPropertiesMapper {target = new CameraProperties(), source = source};
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Test.FieldsMatch(source, map.target);
			#endif
			return map.target;
		}

		private static T[] EnsureArray<T>(T[] arr, int length)
		{
			if (arr == null || arr.Length != length)
				return new T[length];
			return arr;
		}
	}
}