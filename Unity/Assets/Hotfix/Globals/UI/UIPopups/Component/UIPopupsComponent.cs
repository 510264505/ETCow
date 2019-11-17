using System;
using DG.Tweening;
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
        private CanvasGroup shadow;
        private CanvasGroup loading;
        private Transform loadIcon;
        private Vector3 rotate = new Vector3(0, 0, -360);

        private Tween tween;
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            dec = rc.Get<GameObject>("Dec").GetComponent<Text>();
            Button cancelBtn = rc.Get<GameObject>("CancelBtn").GetComponent<Button>();
            determineBtn = rc.Get<GameObject>("DetermineBtn").GetComponent<Button>();

            shadow = rc.Get<GameObject>("Shadow").GetComponent<CanvasGroup>();
            loading = rc.Get<GameObject>("Loading").GetComponent<CanvasGroup>();
            loadIcon = rc.Get<GameObject>("LoadIcon").GetComponent<Transform>();

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
            this.shadow.alpha = isShow ? 1 : 0;
            this.shadow.blocksRaycasts = isShow;
            //this.GetParent<UI>().GameObject.SetActive(isShow);
        }

        public void ShowLoading(bool isShow)
        {
            this.loading.alpha = isShow ? 1 : 0;
            this.loading.blocksRaycasts = isShow;
            if (isShow)
            {
                tween = this.loadIcon.DOLocalRotate(rotate, 2, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
            }
            else
            {
                tween.Kill(false);
                this.loadIcon.localRotation = Quaternion.identity;
            }
        }
    }
}
