using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;

// ReSharper disable SuspiciousTypeConversion.Global
#pragma warning disable 108,114

namespace needle.weaver.webxr
{	
	[NeedlePatch(typeof(IntegratedSubsystem))]
	internal class IntegratedSubsystem_Patch : IntegratedSubsystem
	{
		private bool isRunning;
		
		public void Start()
		{
			isRunning = true;
			if (this is ISubsystemLifecycleCallbacks ic)
			{
				ic.OnStart();
			}
		}
		
		public void Stop()
		{
			isRunning = false;
			if (this is ISubsystemLifecycleCallbacks ic)
			{
				ic.OnStop();
			}
		}
		
		public void Destroy()
		{
			isRunning = false;
			if (this is ISubsystemLifecycleCallbacks ic)
			{
				ic.OnDestroy();
			}
		}
		
		
		internal bool valid => true;
		
		internal bool IsRunning() => isRunning;
	}
}