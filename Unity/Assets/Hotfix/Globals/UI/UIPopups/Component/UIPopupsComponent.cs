using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIPopupsComponentAwake : AwakeSystem<UIPopupsComponent>
    {
        public override void Awake(UIPopupsComponent self)
        {
            self.Awake();
        }
    }
    public class UIPopupsComponent : Component
    {
        private Text dec;
        private Button determineBtn;
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            dec = rc.Get<GameObject>("Dec").GetComponent<Text>();
            Button cancelBtn = rc.Get<GameObject>("CancelBtn").GetComponent<Button>();
            determineBtn = rc.Get<GameObject>("DetermineBtn").GetComponent<Button>();

            cancelBtn.onClick.Add(() => { this.ClosePopups(false); });
        }

        public void ShowPopups(string dec, Action action = null)
        {
            this.dec.text = dec;
            determineBtn.onClick.RemoveAllListeners();
            determineBtn.onClick.Add(() =>
            {
                action?.Invoke();
                this.ClosePopups(false);
            });

            this.ClosePopups(true);
        }

        private void ClosePopups(bool isShow)
        {
            this.GetParent<UI>().GameObject.SetActive(isShow);
        }
    }
}
