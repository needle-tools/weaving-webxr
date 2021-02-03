using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class WebXRInputManager : SubsystemLifecycleManager<XRInputSubsystem, XRInputSubsystemDescriptor>
	{
		protected override XRInputSubsystem GetActiveSubsystemInstance()
		{
			return XRInputSubsystem_Patch.Instance;
		}
	}
}