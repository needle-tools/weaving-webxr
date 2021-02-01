using System;
using System.Collections.Generic;

namespace needle.weaver.webxr
{
	public struct ButtonAction
	{
		public string Name;
		public Action Callback;
	}
	
	public interface IButtonActionProvider
	{
		void GetActions(List<ButtonAction> list);
	}
}