using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private struct Sound
        {
            public byte[] bytes;
            public float time;
            public int seatId;
        }
        private AudioSource musicSource; //背景音乐
        private AudioSource soundSource; //音效和播放语音
        private bool isGetMicrophone = false;
        private string device;
        private readonly int maxRecordTime = 10;
        private readonly int samplingRate = 44100;
        private CancellationTokenSource tokenSource;
        private CancellationToken cancellationToken;
        private Queue<Sound> soundQueue = new Queue<Sound>();
        private Queue<AudioClip> clipQueue = new Queue<AudioClip>();
        private bool isPlaying = false; //是否正在播放语音
        public Action<int> playingSound;

        public void Awake()
        {
            musicSource = ETModel.Component.Global.GetComponent<AudioSource>();
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

            musicSource.Stop();
            musicSource.mute = true;
            soundSource.clip = Microphone.Start(device, false, maxRecordTime, samplingRate);
            Debug.Log("开始录音");
            LimitRecordTime().Coroutine();
        }

        public void OnButtonUp()
        {
            if (this.tokenSource != null)
            {
                this.tokenSource.Cancel();
                this.tokenSource = null;
            }
            Microphone.End(device);
            soundSource.mute = false;
            byte[] bytes = GetData(soundSource.clip);
            bytes = GzipHelper.CompressBytes(bytes);
            //发送数据给服务器
        }

        private async ETVoid LimitRecordTime()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;
            this.tokenSource = tokenSource;
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(maxRecordTime, cancellationToken);
            OnButtonUp();
            //将消息发送给服务器
        }

        private async ETVoid PlaySound()
        {
            this.isPlaying = true;
            while (soundQueue.Count > 0)
            {
                Sound sound = soundQueue.Dequeue();
                sound.bytes = GzipHelper.Decompress(sound.bytes);
                SetData(soundSource.clip, sound.bytes);
                float volume = musicSource.volume;
                musicSource.volume = volume > 0.1f ? 0.1f : volume;
                soundSource.mute = false;
                soundSource.Play();
                playingSound?.Invoke(sound.seatId);
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync((long)(sound.time * 1000));
                musicSource.volume = volume;
                
            }
            while(clipQueue.Count > 0)
            {
                soundSource.clip = clipQueue.Dequeue();
                soundSource.Play();
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
            }
            this.isPlaying = false;
        }
        /// <summary>
        /// 播放语音
        /// </summary>
        public void PlaySound(byte[] bytes, float time, int seatId)
        {
            Sound sound;
            sound.bytes = bytes;
            sound.time = time;
            sound.seatId = seatId;
            soundQueue.Enqueue(sound);
            if (!this.isPlaying)
            {
                PlaySound().Coroutine();
            }
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        public void PlaySound(AudioClip clip)
        {
            if (!isPlaying)
            {
                soundSource.clip = clip;
                soundSource.Play();
            }
            else
            {
                this.clipQueue.Enqueue(clip);
            }
        }

        /// <summary>
        /// 设置背景音乐声音大小
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        /// <summary>
        /// 设置音效大小
        /// </summary>
        public void SetSoundVolume(float volume)
        {
            soundSource.volume = volume;
        }

        public float MusicVolume()
        {
            return musicSource.volume;
        }

        public float SoundVolume()
        {
            return soundSource.volume;
        }

        /// <summary>
        /// 讲录音转成byte
        /// </summary>
        public byte[] GetData(AudioClip clip)
        {
            var data = new float[clip.samples * clip.channels];
            clip.GetData(data, 0);
            byte[] bytes = new byte[data.Length * 4];
            Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);

            return bytes;
        }
        /// <summary>
        /// 讲byte录入AudioClip语音组件
        /// </summary>
        public void SetData(AudioClip clip, byte[] bytes)
        {
            float[] data = new float[bytes.Length / 4];
            Buffer.BlockCopy(bytes, 0, data, 0, data.Length);
            clip.SetData(data, 0);
        }
    }
}
