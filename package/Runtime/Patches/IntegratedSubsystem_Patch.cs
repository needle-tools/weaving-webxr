using needle.Weaver;
using UnityEngine;

#pragma warning disable 108,114

namespace needle.weaver.webxr
{
	// TODO: figure out why patching base types does not work yet
	
	[NeedlePatch(typeof(IntegratedSubsystem))]
	internal class IntegratedSubsystem_Patch
	{
		private bool isRunning;
		
		public void Start()
		{
			isRunning = true;
			// Debug.Log(this);
			if (this.GetType() == typeof(XRInputSubsystem_Patch))
			{
				XRInputSubsystem_Patch.Instance.OnStart();
			}
		}
		
		public void Stop()
		{
			isRunning = false;
			if (this.GetType() == typeof(XRInputSubsystem_Patch))
			{
				XRInputSubsystem_Patch.Instance.OnStop();
			}
		}
		
		public void Destroy()
		{
			isRunning = false;
			if (this.GetType() == typeof(XRInputSubsystem_Patch))
			{
				XRInputSubsystem_Patch.Instance.OnDestroy();
			}
		}
		
		
		internal bool valid => true;
		
		internal bool IsRunning() => isRunning;
	}
}