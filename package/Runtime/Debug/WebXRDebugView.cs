using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace needle.weaver.webxr
{
	public class WebXRDebugView : MonoBehaviour
	{
		public GameObject Template;
		public Transform Content;

		private DebugInfo[] views;
		private List<Text> texts;

		private void Update()
		{
			if (views == null)
			{
				views = GetComponentsInChildren<DebugInfo>();
				texts = new List<Text>();
				Template.SetActive(true);
				foreach (var unused in views)
				{
					var instance = Instantiate(Template, Content);
					var textInstance = instance.GetComponentInChildren<Text>();
					texts.Add(textInstance);
				}
				Template.SetActive(false);
			}

			if (views.Length <= 0)
			{
				enabled = false;
			}

			for (var i = 0; i < views.Length; i++)
			{
				try
				{
					var text = texts[i];
					var vw = views[i];
					text.enabled = vw.enabled;
					text.text = views[i].GetInfo();
					LayoutRebuilder.MarkLayoutForRebuild(text.transform as RectTransform);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
			
			LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
		}
	}
}