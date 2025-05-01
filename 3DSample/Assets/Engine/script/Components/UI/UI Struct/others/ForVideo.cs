using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace UI
{
    public class ForVideo : basePanel
    {
        public event Func calls;

        private VideoPlayer _video;
        private AudioSource _audio;

        protected override void onAwake()
        {
            base.onAwake();

            _video = gameObject.GetComponent<VideoPlayer>();
            _audio = gameObject.GetComponent<AudioSource>();
        }

        public void playVideo(VideoClip clip, Func action)
        {
            if (_video != null)
            {
                _video.clip = clip;
                if (_audio != null)
                    _audio.volume = getSetting().SEValue;
                _video.Play();
                calls += action;
                StartCoroutine(VideoCallBack());
            }
        }

        public void stopVideo()
        {
            _video.Stop();
        }

        private IEnumerator VideoCallBack()
        {
            yield return new WaitForSeconds(0.5f);
            while (_video.isPlaying)
            {
                yield return new WaitForFixedUpdate(); //跟FixedUpdate 一样根据固定帧 更新
            }
            calls();
            Destroy(gameObject);
        }
    }
}

