using ETModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_ChatComponentAwake : AwakeSystem<UICowCow_ChatComponent, GameObject>
    {
        public override void Awake(UICowCow_ChatComponent self, GameObject parent)
        {
            self.Awake(parent);
        }
    }

    /// <summary>
    /// 聊天组件
    /// </summary>
    public class UICowCow_ChatComponent : Component
    {
        private CanvasGroup chatContent;
        private CanvasGroup emoji;

        private int emojiLen = 15;
        private int customMessage = 1001; //只要大于8即可
        private List<string> systemText = new List<string>()
        {
            "快点啊，都等得我花都谢啦！",
            "不要吵了，不要吵了，吵啥嘛吵，专心玩游戏吧！",
            "各位，真不好意思，我要离开一会！",
            "和你合作真是太愉快了啊！",
            "我们交个朋友吧，能不能告诉我你的联系方式？",
            "大家好，很高兴见到各位！",
            "你的牌打得太好啦！",
            "下次再玩吧，我先走啦！"
        };

        public void Awake(GameObject parent)
        {
            ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            GameObject ab = (GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowcowChat);
            this.GameObject = UnityEngine.Object.Instantiate(ab);
            this.GameObject.transform.SetParent(parent.transform, false);
            this.GameObject.name = UICowCowType.CowcowChat;

            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();

            chatContent = rc.Get<GameObject>("ChatContent").GetComponent<CanvasGroup>();
            GameObject content = rc.Get<GameObject>("Content");
            emoji = rc.Get<GameObject>("Emoji").GetComponent<CanvasGroup>();
            InputField inputChatContent = rc.Get<GameObject>("InputChatContent").GetComponent<InputField>();

            Button sendBtn = rc.Get<GameObject>("SendBtn").GetComponent<Button>();
            Button[] emojiBtn = new Button[emojiLen];
            int seatId = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>().GamerComponent.LocalSeatID;
            for (int i = 0; i < emojiBtn.Length; i++)
            {
                int n = i;
                emojiBtn[i] = rc.Get<GameObject>($"EmojiBtn{i}").GetComponent<Button>();
                emojiBtn[i].onClick.Add(() => {
                    Actor_SendChatMessageHelper.SendEmoji(n, seatId).Coroutine();
                    this.ShowHideEmoji(false);
                });
            }
            sendBtn.onClick.Add(() => {
                Actor_SendChatMessageHelper.SendFont(customMessage, seatId, inputChatContent.text).Coroutine();
                inputChatContent.text = string.Empty;
                this.ShowHideChatFont(false);
            });
            this.GameObject.GetComponent<Button>().onClick.Add(() => {
                this.ShowHideChatFont(false);
                this.ShowHideEmoji(false);
            });

            Button[] chatContentBtn = new Button[systemText.Count];
            GameObject prefab = (GameObject)ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowChatContentBtn);
            for (int i = 0; i < chatContentBtn.Length; i++)
            {
                int n = i;
                chatContentBtn[i] = UnityEngine.Object.Instantiate(prefab).GetComponent<Button>();
                chatContentBtn[i].transform.SetParent(content.transform, false);
                chatContentBtn[i].transform.GetChild(0).GetComponent<Text>().text = systemText[i];
                chatContentBtn[i].onClick.Add(() => {
                    Actor_SendChatMessageHelper.SendFont(n, seatId, systemText[n]).Coroutine();
                    this.ShowHideChatFont(false);
                });
            }
        }

        /// <summary>
        /// 显示隐藏文字聊天面板
        /// </summary>
        public void ShowHideChatFont(bool isShow)
        {
            emoji.alpha = !isShow ? 1 : 0;
            emoji.blocksRaycasts = !isShow;
            chatContent.alpha = isShow ? 1 : 0;
            chatContent.blocksRaycasts = isShow;
            this.GameObject.GetComponent<CanvasGroup>().alpha = isShow ? 1 : 0;
            this.GameObject.GetComponent<CanvasGroup>().blocksRaycasts = isShow;
        }

        /// <summary>
        /// 显示隐藏表情聊天面板
        /// </summary>
        public void ShowHideEmoji(bool isShow)
        {
            emoji.alpha = isShow ? 1 : 0;
            emoji.blocksRaycasts = isShow;
            chatContent.alpha = !isShow ? 1 : 0;
            chatContent.blocksRaycasts = !isShow;
            this.GameObject.GetComponent<CanvasGroup>().alpha = isShow ? 1 : 0;
            this.GameObject.GetComponent<CanvasGroup>().blocksRaycasts = isShow;
        }
    }
}
