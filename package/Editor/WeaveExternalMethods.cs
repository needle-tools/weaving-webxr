using System;
using System.Collections.Generic;
using Fody;
using needle.Weaver;
using UnityEngine;

namespace needle.weaver.webxr
{
	public class WeaveExternalMethods : BaseModuleWeaver
	{
		public override void Execute()
		{
			int count = 0;
			var patchedMethods = "";

			ModuleDefinition.ForEachMethod(method =>
			{
				try
				{
					if (method.AddExternalMethodBody())
					{
						patchedMethods += method.FullName + "\n";
						++count;
					}
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			});
			
			Debug.Log("Patched " + count + " external methods:\n" + patchedMethods);
		}

		public override IEnumerable<string> GetAssembliesForScanning()
		{
			yield break;
		}
	}
}