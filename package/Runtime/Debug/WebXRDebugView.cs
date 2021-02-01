using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace needle.weaver.webxr
{
	public class WebXRDebugView : MonoBehaviour
	{
		public Transform Content;
		[FormerlySerializedAs("Template")] public GameObject TextTemplate;
		public GameObject ButtonTemplate;

		private DebugInfo[] views;
		private List<Text> texts;

		private void Update()
		{
			if (views == null)
			{
				views = GetComponentsInChildren<DebugInfo>();
				texts = new List<Text>();
				TextTemplate.SetActive(true);
				ButtonTemplate.SetActive(true);
				var actions = new List<ButtonAction>();
				foreach (var info in views)
				{
					var instance = Instantiate(TextTemplate, Content);
					var textInstance = instance.GetComponentInChildren<Text>();
					texts.Add(textInstance);
					
					actions.Clear();
					info.GetActions(actions);
					foreach (var act in actions)
					{
						var buttonInstance = Instantiate(ButtonTemplate, Content);
						buttonInstance.GetComponentInChildren<Text>().text = act.Name;
						buttonInstance.GetComponentInChildren<Button>().onClick.AddListener(() => act.Callback());
						LayoutRebuilder.MarkLayoutForRebuild(buttonInstance.transform as RectTransform);
					}
				}
				TextTemplate.SetActive(false);
				ButtonTemplate.SetActive(false);
			}

			if (views.Length <= 0)
			{
				enabled = false;
			}

			for (var i = 0; i < views.Length; i++)
			{
				var text = texts[i];
				try
				{
					var vw = views[i];
					text.enabled = vw.enabled;
					text.text = views[i].GetInfo();
					LayoutRebuilder.MarkLayoutForRebuild(text.transform as RectTransform);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					text.enabled = false;
				}
			}
			
			LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
		}
	}
}