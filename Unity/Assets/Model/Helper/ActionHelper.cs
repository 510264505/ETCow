using System;
using UnityEngine.UI;

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
	}
}