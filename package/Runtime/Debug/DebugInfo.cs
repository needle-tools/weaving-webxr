using System;
using System.Collections.Generic;
using UnityEngine;

namespace needle.weaver.webxr
{
	public abstract class DebugInfo : MonoBehaviour, IButtonActionProvider
	{
		public abstract string GetInfo();
		
		private void OnEnable()
		{
			
		}

		public virtual void GetActions(List<ButtonAction> list)
		{
		}
	}
}