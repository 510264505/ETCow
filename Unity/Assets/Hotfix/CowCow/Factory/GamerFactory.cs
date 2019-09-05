using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class GamerFactory
    {
        /// <summary>
        /// 创建玩家对象
        /// </summary>
        public static Gamer Create(GamerInfo info)
        {
            Gamer gamer = ComponentFactory.Create<Gamer>();
            gamer.UserID = info.UserID;
            return gamer;
        }

        /// <summary>
        /// 创建预制体
        /// </summary>
        public static GameObject Create(string abName)
        {
            ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            rc.LoadBundle(UICowCowAB.CowCow_Prefabs);
            GameObject ab = (GameObject)rc.GetAsset(UICowCowAB.CowCow_Prefabs, abName);
            return UnityEngine.Object.Instantiate(ab);
        }
    }
}

