using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class WebXRSubsystem : SubsystemLifecycleManager<XRInputSubsystem, XRInputSubsystemDescriptor>
	{
		protected override XRInputSubsystem GetActiveSubsystemInstance()
		{
			return XRInputSubsystem_Patch.Instance;
		}
	}
}