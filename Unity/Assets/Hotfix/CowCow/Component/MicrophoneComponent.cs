using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class MicrophoneComponentAwake : AwakeSystem<MicrophoneComponent>
    {
        public override void Awake(MicrophoneComponent self)
        {
            self.Awake();
        }
    }
    public class MicrophoneComponent : Component
    {
        private AudioSource audioSource;
        private AudioSource soundSource;
        private bool isGetMicrophone = false;
        private string device;
        private readonly int maxRecordTime = 10;
        private readonly int samplingRate = 44100;

        public void Awake()
        {
            audioSource = ETModel.Component.Global.GetComponent<AudioSource>();
            soundSource = ETModel.Component.Global.AddComponent<AudioSource>();
            InitGetMicrophone();
        }

        public void InitGetMicrophone()
        {
            if (isGetMicrophone)
            {
                return;
            }
            string[] devices = Microphone.devices;
            if (devices.Length > 0)
            {
                isGetMicrophone = true;
                device = devices[0];
                Log.Debug($"获取到麦克风{device}");
            }
            else
            {
                isGetMicrophone = false;
                Log.Debug("没有获取到麦克风");
            }
        }

        //开始录音按钮
        public void OnButtonDown()
        {
            InitGetMicrophone();

            audioSource.Stop();
            audioSource.mute = true;
            audioSource.clip = Microphone.Start(device, false, maxRecordTime, samplingRate);
            Debug.Log("开始录音");
            LimitRecordTime().Coroutine();
        }

        public void OnButtonUp()
        {
            Microphone.End(device);
            audioSource.mute = false;
            byte[] bytes = GetData(audioSource.clip);
        }

        private async ETVoid LimitRecordTime()
        {
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(maxRecordTime);
            OnButtonUp();
            //将消息发送给服务器
        }

        public byte[] GetData(AudioClip clip)
        {
            var data = new float[clip.samples * clip.channels];
            clip.GetData(data, 0);
            byte[] bytes = new byte[data.Length * 4];
            Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public void SetData(AudioClip clip, byte[] bytes)
        {
            float[] data = new float[bytes.Length / 4];
            Buffer.BlockCopy(bytes, 0, data, 0, data.Length);
            clip.SetData(data, 0);
        }
    }
}
