using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ManagerSpace
{
    [DisallowMultipleComponent]
    public class scenesManager : MonoBehaviour
    {
        public static string firstScene = "MainScene";

        //管理器的实例
        public static scenesManager instance;

        public delegate void callbackAfterLoadScene();

        public event callbackAfterLoadScene callbacks;

        [Header("UI首页的场景名")]
        [SerializeField]
        private string nowScene;
        public string NowScene
        {
            get
            {
                return nowScene;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void loadAllScenes()
        {
            if (SceneManager.GetActiveScene().name == firstScene)
            {
                return;
            }
            SceneManager.LoadScene(firstScene);
        }

        private void Awake()
        {
            //创造管理器实例
            if (scenesManager.instance == null)
            {
                scenesManager.instance = this;
                DontDestroyOnLoad(scenesManager.instance.gameObject);
            }
            else if (scenesManager.instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        public void init()
        {
            nowScene = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded += loadedEve;
        }

        private IEnumerator StartLoading(string scene, GameObject Mask)
        {
            yield return new WaitForSeconds(1);
            Mask.SetActive(true);
            nowScene = string.Empty;

            /*
            //异步加载
            AsyncOperation op = SceneManager.LoadSceneAsync(scene);
            op.allowSceneActivation = false;
            while(op.progress < 0.9f)
            {
            }
            op.allowSceneActivation = true;
            */

            // 直接加载
            SceneManager.LoadScene(scene);

        }

        private void loadedEve(Scene s, LoadSceneMode l)
        {
            if (nowScene != SceneManager.GetActiveScene().name)
            {
                nowScene = SceneManager.GetActiveScene().name;
                callbacks();
            }
        }

        public void loadScene(string scene, GameObject Mask)
        {
            StartCoroutine(StartLoading(scene, Mask));
        }
    }
}
