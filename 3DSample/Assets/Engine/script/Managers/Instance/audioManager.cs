using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagerSpace
{
    [DisallowMultipleComponent]
    public class audioManager : MonoBehaviour
    {
        public static audioManager instance;

        [SerializeField]
        private AudioSource musicAudio;
        [SerializeField]
        private AudioSource BGSAudio;
        [SerializeField]
        private AudioSource SEAudio;

        //用于关闭bgm
        private float time = 1f;
        private bool finishBgm = true;
        private bool finishBgs = true;

        private float musicValue;
        public float MusicValue
        {
            set
            {
                musicValue = value;
            }
        }

        private float seValue;
        public float SEValue
        {
            set
            {
                seValue = value;
            }
        }

        private void Awake()
        {
            //创造管理器实例
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance.gameObject);
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        public void init()
        {
            AudioSource[] a = gameObject.GetComponents<AudioSource>();
            if (musicAudio == null)
                musicAudio = a[0];
            if (SEAudio == null)
                SEAudio = a[1];
            if (BGSAudio == null)
                BGSAudio = a[2];

            if (musicAudio == null)
                Debug.LogError("未找到bgm播放器");
            else
            {
                musicAudio.volume = musicValue;
                musicAudio.loop = true;
            }
            if (BGSAudio == null)
                Debug.LogError("未找到bgs播放器");
            else
            {
                BGSAudio.volume = musicValue;
                BGSAudio.loop = true;
            }
            if (SEAudio == null)
                Debug.LogError("未找到se播放器");
            else
            {
                SEAudio.volume = seValue;
            }
        }

        //设置音乐音量
        public void changeMusicValue(float num)
        {
            musicValue += num;
            musicAudio.volume = musicValue;
            BGSAudio.volume = musicValue;
        }

        //改变音乐音量，用于淡入淡出
        public bool reduceMusicVolume(float num)
        {
            if (musicAudio.volume > 0)
            {
                musicAudio.volume -= num * musicValue;
                return false;
            }
            else
            {
                musicAudio.Stop();
                musicAudio.volume = musicValue;
                return true;
            }
        }

        public bool reduceBGSVolume(float num)
        {
            if (BGSAudio.volume > 0)
            {
                BGSAudio.volume -= num * musicValue;
                return false;
            }
            else
            {
                BGSAudio.Stop();
                BGSAudio.volume = musicValue;
                return true;
            }
        }

        //改变音效音量
        public void changeSEValue(float num)
        {
            seValue += num;
            SEAudio.volume = seValue;
        }



        public void playMusic(AudioClip clip)
        {
            if (musicAudio.isPlaying)
            {
                musicAudio.Stop();
                musicAudio.volume = musicValue;
                finishBgm = true;
            }

            musicAudio.clip = clip;
            changeMusicValue(0);
            musicAudio.Play();
        }

        public void stopMusic(bool now = false)
        {
            if (now)
                musicAudio.Stop();
            else
                finishBgm = false;
        }

        public void playBGS(AudioClip clip)
        {
            if (BGSAudio.isPlaying)
            {
                BGSAudio.Stop();
                BGSAudio.volume = musicValue;
                finishBgs = true;
            }

            BGSAudio.clip = clip;
            changeMusicValue(0);
            BGSAudio.Play();
        }

        public void stopBGS(bool now = false)
        {
            if (now)
                BGSAudio.Stop();
            else
                finishBgs = false;
        }

        public void playSE(AudioClip clip, float pitch = 1.0f, bool loop = false)
        {
            SEAudio.loop = loop;
            SEAudio.pitch = pitch;
            SEAudio.clip = clip;
            SEAudio.Play();
        }

        public void stopSE()
        {
            SEAudio.loop = false;
            SEAudio.Stop();
        }

        public void doEveryFrame(float _musicValue, float _SEValue)
        {
            musicValue = _musicValue;
            seValue = _SEValue;

            if (!finishBgm)
            {
                finishBgm = reduceMusicVolume(musicValue * Time.deltaTime / time);
            }
            if (!finishBgs)
            {
                finishBgs = reduceBGSVolume(musicValue * Time.deltaTime / time);
            }
        }

        public void resetForNewScene(bool stopMusic)
        {
            if (stopMusic)
                finishBgm = false;
            stopSE();
        }

        public void changeValue(settings set)
        {
            MusicValue = set.musicValue;
            SEValue = set.SEValue;
        }
    }
}
