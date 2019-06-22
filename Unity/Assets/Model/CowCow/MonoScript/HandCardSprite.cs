using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ETModel
{
    /// <summary>
    /// 每张牌下挂的脚本
    /// </summary>
    public class HandCardSprite : MonoBehaviour
    {
        public Card Card;
        private bool isSelect;
        private void Start()
        {
            EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(new UnityAction<BaseEventData>(OnClick));
            eventTrigger.triggers.Add(entry);
        }
        public void OnClick(BaseEventData data)
        {
            if (isSelect)
            {
                Game.EventSystem.Run(CowCowEventIdType.SelectHandCard);
            }
            else
            {
                Game.EventSystem.Run(CowCowEventIdType.CancelHandCard);
            }
            //这张牌的移动Pos
        }
    }
}
