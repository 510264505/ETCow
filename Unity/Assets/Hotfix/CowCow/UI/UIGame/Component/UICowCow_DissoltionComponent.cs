using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_DissoltionComponentAwake : AwakeSystem<UICowCow_DissoltionComponent, GameObject>
    {
        public override void Awake(UICowCow_DissoltionComponent self, GameObject parent)
        {
            self.Awake(parent);
        }
    }
    public class UICowCow_DissoltionComponent : Component
    {
        private class GamerDissInfo
        {
            public GameObject parent;
            public Image headIcon;
            public Text gamerName;
            public Text vote;
            public GamerDissInfo(GameObject parent, GameObject prefab, string name)
            {
                this.parent = UnityEngine.Object.Instantiate(prefab);
                this.parent.transform.SetParent(parent.transform, false);
                headIcon = this.parent.transform.Find("HeadIcon").GetComponent<Image>();
                gamerName = this.parent.transform.Find("Name").GetComponent<Text>();
                vote = this.parent.transform.Find("Condition").GetComponent<Text>();
                gamerName.text = name;
                vote.text = "...";
            }
            
        }

        private GameObject gamerInfo;
        private Dictionary<int, GamerDissInfo> gamerDissInfos;

        public void Awake(GameObject parent)
        {
            ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            GameObject ab = (GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowDissoltion);
            this.GameObject = UnityEngine.Object.Instantiate(ab);
            this.GameObject.transform.SetParent(parent.transform, false);
            this.GameObject.name = UICowCowType.CowCowDissoltion;

            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();
            gamerInfo = rc.Get<GameObject>("GamerInfo");
            Button agreeBtn = rc.Get<GameObject>("Agree").GetComponent<Button>();
            Button disagreeBtn = rc.Get<GameObject>("Disagree").GetComponent<Button>();

            agreeBtn.onClick.Add(OnAgree);
            disagreeBtn.onClick.Add(OnDisagree);
        }

        /// <summary>
        /// 显示发起解散
        /// </summary>
        public void ShowInitiateDissoltion(int seatId)
        {
            if (gamerDissInfos == null)
            {
                ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                res.LoadBundle(UICowCowAB.CowCow_Prefabs);
                GameObject ab = (GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowGamerDissInfo);
                gamerDissInfos = new Dictionary<int, GamerDissInfo>();
                UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
                Dictionary<int, Gamer> gamers = room.GamerComponent.GetDictAll();
                foreach (var gamer in gamers)
                {
                    gamerDissInfos.Add(gamer.Key, new GamerDissInfo(this.gamerInfo, ab, gamer.Value.GetComponent<UICowCow_GamerInfoComponent>().gamerName));
                    if (gamer.Key == seatId)
                    {
                        gamerDissInfos[seatId].vote.text = "发起人";
                    }
                }
            }
            else
            {
                ShowHideDissoltion(false);
            }
        }

        /// <summary>
        /// 其他人投票表决
        /// </summary>
        public void ShowOtherVote(int seatId, string vote)
        {
            gamerDissInfos[seatId].vote.text = vote;
        }

        public void CloseDissoltion()
        {
            ShowHideDissoltion(false);
        }

        private void ShowHideDissoltion(bool isShow)
        {
            this.GameObject.GetComponent<CanvasGroup>().DOFade(isShow ? 1 : 0, 0.5f);
            this.GameObject.GetComponent<CanvasGroup>().blocksRaycasts = isShow;
        }

        private void OnAgree()
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            Actor_DissoltionHelper.OnSendVOte(room.GamerComponent.LocalSeatID, true).Coroutine();
        }
        private void OnDisagree()
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            Actor_DissoltionHelper.OnSendVOte(room.GamerComponent.LocalSeatID, false).Coroutine();
        }
    }
}
