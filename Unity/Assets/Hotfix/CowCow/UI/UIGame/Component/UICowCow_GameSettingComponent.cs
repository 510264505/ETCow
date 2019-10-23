using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowCow_GameSettingComponentAwake : AwakeSystem<UICowCow_GameSettingComponent, GameObject>
    {
        public override void Awake(UICowCow_GameSettingComponent self, GameObject parent)
        {
            self.Awake(parent);
        }
    }
    public class UICowCow_GameSettingComponent : Component
    {
        private ResourcesComponent res;
        private MicrophoneComponent microphone;
        private CanvasGroup uiVoiceSetting;
        private CanvasGroup uiHelp;
        private Slider musicSlider;
        private Slider soundSlider;
        private Button musicSwitchBtn;
        private Button soundSwitchBtn;

        public void Awake(GameObject parent)
        {
            microphone = Game.Scene.GetComponent<MicrophoneComponent>();
            res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UICowCowAB.CowCow_Prefabs);
            res.LoadBundle(UICowCowAB.CowCow_Texture);
            GameObject ab = (GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowGameSetting);
            this.GameObject = UnityEngine.Object.Instantiate(ab);
            this.GameObject.transform.SetParent(parent.transform, false);
            this.GameObject.name = UICowCowType.CowCowGameSetting;

            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();
            uiVoiceSetting = rc.Get<GameObject>("UIVoiceSetting").GetComponent<CanvasGroup>();
            musicSlider = rc.Get<GameObject>("MusicSlider").GetComponent<Slider>();
            soundSlider = rc.Get<GameObject>("SoundSlider").GetComponent<Slider>();
            musicSwitchBtn = rc.Get<GameObject>("MusicSwitchButton").GetComponent<Button>();
            soundSwitchBtn = rc.Get<GameObject>("SoundSwitchButton").GetComponent<Button>();
            Button helpBtn = rc.Get<GameObject>("HelpBtn").GetComponent<Button>();
            Button comfirmBtn = rc.Get<GameObject>("ComfirmBtn").GetComponent<Button>();

            uiHelp = rc.Get<GameObject>("UIHelp").GetComponent<CanvasGroup>();
            Button hCloseBtn = rc.Get<GameObject>("HCloseBtn").GetComponent<Button>();

            musicSlider.onValueChanged.Add(OnMusicSlider);
            soundSlider.onValueChanged.Add(OnSoundSlider);
            musicSwitchBtn.onClick.Add(OnMusicSwitch);
            soundSwitchBtn.onClick.Add(OnSoundSwitch);
            helpBtn.onClick.Add(OnHelp);
            comfirmBtn.onClick.Add(OnComfirm);
            hCloseBtn.onClick.Add(OnHClose);
        }
        private void OnMusicSlider(float slider)
        {
            microphone.SetMusicVolume(slider);
        }
        private void OnSoundSlider(float slider)
        {
            microphone.SetSoundVolume(slider);
        }
        private void OnMusicSwitch()
        {
            if (musicSlider.value > 0)
            {
                // 当slider值改变时，会触发事件onValueChanged
                musicSlider.value = 0;
                musicSwitchBtn.GetComponent<Image>().sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, "set_btn_voice_off");
            }
            else
            {
                musicSlider.value = 1;
                musicSwitchBtn.GetComponent<Image>().sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, "set_btn_voice_on");
            }
        }
        private void OnSoundSwitch()
        {
            if (soundSlider.value > 0)
            {
                soundSlider.value = 0;
                soundSwitchBtn.GetComponent<Image>().sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, "set_btn_voice_off");
            }
            else
            {
                soundSlider.value = 1;
                soundSwitchBtn.GetComponent<Image>().sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, "set_btn_voice_on");
            }
        }
        private void OnHelp()
        {
            this.ShowHideUIHelp(true);
        }
        private void OnComfirm()
        {
            this.ShowHideUIGameSetting(false);
        }
        
        private void OnHClose()
        {
            this.ShowHideUIHelp(false);
        }

        private void ShowHideUIHelp(bool isShow)
        {
            uiHelp.alpha = isShow ? 1 : 0;
            uiHelp.blocksRaycasts = isShow;
        }

        public void ShowHideUIGameSetting(bool isShow)
        {
            this.GameObject.GetComponent<CanvasGroup>().alpha = isShow ? 1 : 0;
            this.GameObject.GetComponent<CanvasGroup>().blocksRaycasts = isShow;
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();

            res.UnloadBundle(UICowCowAB.CowCow_Prefabs);
            res.UnloadBundle(UICowCowAB.CowCow_Texture);
            UnityEngine.Object.Destroy(this.GameObject);
        }
    }
}
