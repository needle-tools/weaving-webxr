namespace needle.weaver.webxr.Utils
{
	internal interface ISubsystemLifecycleCallbacks
	{
		void OnStart();
		void OnStop();
		void OnDestroy();
		bool GetIsRunning();
	}
}