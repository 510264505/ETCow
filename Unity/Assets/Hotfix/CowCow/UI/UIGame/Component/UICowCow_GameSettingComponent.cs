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
        private MicrophoneComponent microphone;
        private CanvasGroup uiVoiceSetting;
        private CanvasGroup uiHelp;
        private Slider musicSlider;
        private Slider soundSlider;

        private float tempMusicSlider;
        private float tempSoundSlider;

        public void Awake(GameObject parent)
        {
            microphone = Game.Scene.GetComponent<MicrophoneComponent>();
            tempMusicSlider = microphone.MusicVolume();
            tempMusicSlider = microphone.SoundVolume();
            ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            GameObject ab = (GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowGameSetting);
            this.GameObject = UnityEngine.Object.Instantiate(ab);
            this.GameObject.transform.SetParent(parent.transform, false);
            this.GameObject.name = UICowCowType.CowCowGameSetting;

            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();
            uiVoiceSetting = rc.Get<GameObject>("UIVoiceSetting").GetComponent<CanvasGroup>();
            musicSlider = rc.Get<GameObject>("MusicSlider").GetComponent<Slider>();
            soundSlider = rc.Get<GameObject>("SoundSlider").GetComponent<Slider>();
            Button musicSwitchBtn = rc.Get<GameObject>("MusicSwitchButton").GetComponent<Button>();
            Button soundSwitchBtn = rc.Get<GameObject>("SoundSwitchButton").GetComponent<Button>();
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
            this.SetMusicVolume(slider);
        }
        private void OnSoundSlider(float slider)
        {
            this.SetSoundVolume(slider);
        }
        private void OnMusicSwitch()
        {
            if (microphone.MusicVolume() > 0)
            {
                microphone.SetMusicVolume(0);
            }
            else
            {
                this.SetMusicVolume(tempMusicSlider);
            }
        }
        private void OnSoundSwitch()
        {
            if (microphone.SoundVolume() > 0)
            {
                microphone.SetSoundVolume(0);
            }
            else
            {
                this.SetSoundVolume(tempSoundSlider);
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

        private void SetMusicVolume(float volume)
        {
            microphone.SetMusicVolume(volume);
            this.tempMusicSlider = volume;
        }
        private void SetSoundVolume(float volume)
        {
            microphone.SetSoundVolume(volume);
            this.tempSoundSlider = volume;
        }

        public void ShowHideUIGameSetting(bool isShow)
        {
            this.GameObject.GetComponent<CanvasGroup>().alpha = isShow ? 1 : 0;
            this.GameObject.GetComponent<CanvasGroup>().blocksRaycasts = isShow;
        }
    }
}
