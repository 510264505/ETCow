using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace ETModel
{
	public static class ActionHelper
	{
		public static void Add(this Button.ButtonClickedEvent buttonClickedEvent, Action action)
		{
			buttonClickedEvent.AddListener(() => { action(); });
		}
        public static void Add(this Slider.SliderEvent sliderEvent, Action<float> action)
        {
            sliderEvent.AddListener((slider) => { action(slider); });
        }
        public static void Add(this TriggerEvent triggerEvent, UnityAction<BaseEventData> action)
        {
            triggerEvent.AddListener((data) => { action(data); });
        }
	}
}