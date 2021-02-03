using UnityEngine.XR;

namespace needle.weaver.webxr
{
	public class WebXRDisplayManager : SubsystemLifecycleManager<XRDisplaySubsystem, XRDisplaySubsystemDescriptor>
	{
		protected override XRDisplaySubsystem GetActiveSubsystemInstance()
		{
			return XRDisplaySubsystem_Patch.Instance;
		}
	}
}