using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 文字聊天信息处理
    /// </summary>
    [MessageHandler]
    public class Actor_ChatMessageHandler : AMHandler<Actor_CowCowChatFont>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowChatFont message)
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            UICowCow_GamerInfoComponent gameInfo = room.GamerComponent.Get(message.SeatID).GetComponent<UICowCow_GamerInfoComponent>();
            gameInfo.ShowChatFont(message.ChatIndex, message.ChatMessage, gameInfo.Sex);
        }
    }

    /// <summary>
    /// 表情聊天信息处理
    /// </summary>
    [MessageHandler]
    public class Actor_chatEmojiMessageHandler : AMHandler<Actor_CowCowEmoji>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowEmoji message)
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            UICowCow_GamerInfoComponent gameInfo = room.GamerComponent.Get(message.SeatID).GetComponent<UICowCow_GamerInfoComponent>();
            gameInfo.ShowEmoji(message.Index);
        }
    }
}
