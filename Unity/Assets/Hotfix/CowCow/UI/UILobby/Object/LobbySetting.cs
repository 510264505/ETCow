using UnityEngine;
using UnityEngine.UI;
using ETModel;

namespace ETHotfix
{
    public class LobbySetting
    {
        private ResourcesComponent res;
        private CanvasGroup parent;
        private Slider musicSlider;
        private Slider soundSlider;
        private Button musicBtn;
        private Button soundBtn;
        private Button determineBtn;

        public LobbySetting(GameObject parent)
        {
            res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            GameObject go = UnityEngine.Object.Instantiate((GameObject)res.GetAsset(UICowCowAB.CowCow_Prefabs, UICowCowType.CowCowLobbySetting));
            go.transform.SetParent(parent.transform, false);
            this.parent = go.GetComponent<CanvasGroup>();
            this.musicSlider = this.parent.transform.Find("Music/MusicSlider").GetComponent<Slider>();
            this.soundSlider = this.parent.transform.Find("Sound/SoundSlider").GetComponent<Slider>();

            this.musicBtn = this.parent.transform.Find("Music/MusicSwitchButton").GetComponent<Button>();
            this.soundBtn = this.parent.transform.Find("Sound/SoundSwitchButton").GetComponent<Button>();
            this.determineBtn = this.parent.transform.Find("DetermineBtn").GetComponent<Button>();

            this.musicSlider.onValueChanged.Add(OnMusicSlider);
            this.soundSlider.onValueChanged.Add(OnSoundSlider);
            this.musicBtn.onClick.Add(OnMusicSwitch);
            this.soundBtn.onClick.Add(OnSoundSwitch);
            this.determineBtn.onClick.Add(() => this.ShowHideLobbySetting(false));
        }

        public void ShowHideLobbySetting(bool isShow)
        {
            this.parent.alpha = isShow ? 1 : 0;
            this.parent.blocksRaycasts = isShow;
        }

        private void OnMusicSlider(float slider)
        {
            Game.Scene.GetComponent<MicrophoneComponent>().SetMusicVolume(slider);
        }

        private void OnSoundSlider(float slider)
        {
            Game.Scene.GetComponent<MicrophoneComponent>().SetSoundVolume(slider);
        }
        private void OnMusicSwitch()
        {
            if (musicSlider.value > 0)
            {
                // 当slider值改变时，会触发事件onValueChanged
                musicSlider.value = 0;
                musicBtn.GetComponent<Image>().sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, "set_btn_voice_off");
            }
            else
            {
                musicSlider.value = 1;
                musicBtn.GetComponent<Image>().sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, "set_btn_voice_on");
            }
        }
        private void OnSoundSwitch()
        {
            if (soundSlider.value > 0)
            {
                soundSlider.value = 0;
                soundBtn.GetComponent<Image>().sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, "set_btn_voice_off");
            }
            else
            {
                soundSlider.value = 1;
                soundBtn.GetComponent<Image>().sprite = (Sprite)res.GetAsset(UICowCowAB.CowCow_Texture, "set_btn_voice_on");
            }
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this.parent.gameObject);
        }
    }
}
