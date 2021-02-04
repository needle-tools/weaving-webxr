using needle.Weaver;
using needle.weaver.webxr.Utils;
using UnityEngine;

// ReSharper disable SuspiciousTypeConversion.Global
#pragma warning disable 108,114

namespace needle.weaver.webxr
{	
	[NeedlePatch(typeof(IntegratedSubsystem))]
	internal class IntegratedSubsystem_Patch
	{
		public void Start()
		{
			if (this is ISubsystemLifecycleCallbacks ic)
			{
				ic.OnStart();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				Debug.Log("STARTED " + this + ", running? " + ic.GetIsRunning());
#endif
			}
		}
		
		public void Stop()
		{
			if (this is ISubsystemLifecycleCallbacks ic)
			{
				ic.OnStop();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				Debug.Log("STOPPED " + this + ", running? " + ic.GetIsRunning());
#endif
			}
		}
		
		public void Destroy()
		{
			if (this is ISubsystemLifecycleCallbacks ic)
			{
				ic.OnDestroy();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				Debug.Log("DESTROYED " + this + ", running? " + ic.GetIsRunning());
#endif
			}
		}

		internal bool valid => true;
		internal bool IsRunning()
		{
			if (this is ISubsystemLifecycleCallbacks ic)
				return ic.GetIsRunning();
			return false;
		}
	}
}