using System;
using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;

namespace needle.weaver.webxr
{
	public class ExitDisplaySubsystemHelper : MonoBehaviour
	{
		public Behaviour[] Behaviours;
		public Action Callback;

		private IEnumerator Start()
		{
			yield return null;
			yield return null;
			yield return null;
			if (Behaviours == null) yield break;
			foreach (var c in Behaviours)
			{
				Debug.Log("enable " + c);
				c.enabled = true;
			}
			Callback?.Invoke();
		}
	}
}