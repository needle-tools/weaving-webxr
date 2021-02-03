using UnityEngine;

namespace needle.weaver.webxr
{
	public interface IDisplayDataProvider
	{
		Matrix4x4 ProjectionLeft { get; set; }
		Matrix4x4 ProjectionRight { get; set; }
	}
}