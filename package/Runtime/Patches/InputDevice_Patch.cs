using needle.Weaver;
using UnityEngine.XR;

namespace needle.Weavers.InputDevicesPatch
{
	[NeedlePatch(typeof(InputDevice))]
	public class InputDevice_Patch
	{
		public XRInputSubsystem subsystem => XRInputSubsystem_Patch.Instance;
		// the rest is automatically set correctly because we implement XRInputSubsystem.TryGetDeviceIds_AsList
	}
}