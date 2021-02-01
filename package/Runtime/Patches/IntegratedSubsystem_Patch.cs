using System;
using needle.Weaver;
using UnityEngine;
using UnityEngine.Subsystems;
using UnityEngine.XR;

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
			Debug.Log("Starting 1 " + this);
			isRunning = true;
		}
		
		public void Stop()
		{
			isRunning = false;
		}
		
		public void Destroy()
		{
			isRunning = false;
		}
		
		
		internal bool valid => true;
		
		internal bool IsRunning() => isRunning;
	}
}