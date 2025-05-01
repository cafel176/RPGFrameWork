using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UI;

namespace ManagerSpace
{
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour
    {
        //管理器的实例
        public static UIManager instance;

        [SerializeField]
        private GameObject attackPivot;

        [SerializeField]
        private GameObject GameUI;
        // 2选1 yes no panel
        [SerializeField]
        private GameObject choosePanel;
        // 事件的多选项panel
        [SerializeField]
        private GameObject eventchoosePanel;
        [SerializeField]
        private GameObject videoPanel;
        //用于淡入淡出的UI
        [SerializeField]
        private GameObject BlackPanel;
        [SerializeField]
        private GameObject WhitePanel;
        //用于显示文字的UI
        [SerializeField]
        private GameObject TextPanel;
        //用于显示文字的UI
        [SerializeField]
        private GameObject SpecialTextPanel;
        //用于提示的UI
        [SerializeField]
        private GameObject hint;
        //菜单UI
        [SerializeField]
        private GameObject menu;
        public bool canCallMenu
        {
            get
            {
                return menu != null;
            }
        }

        //存档UI
        [SerializeField]
        private GameObject save;
        //显示图片的UI
        [SerializeField]
        private GameObject picPanel;
        // 调查提示框
        [SerializeField]
        private GameObject canDoHint;

        // 显示文字时的音效
        [SerializeField]
        private AudioClip show;

        //=====================不可见变量===============================
        //控制文字显示进度的开关
        private bool showFinish = true;
        public bool ShowFinish
        {
            get
            {
                return showFinish;
            }
        }

        private bool canHide = false;
        public bool CanHide
        {
            get
            {
                return canHide;
            }
        }

        private Dictionary<string, GameObject> images = new Dictionary<string, GameObject>();
        private Dictionary<string, Button> btns = new Dictionary<string, Button>();

        //引用
        private GameObject gameUI = null;
        private GameObject blackPanel = null;
        private GameObject whitePanel = null;
        private TextPanel textPanel = null;
        private GameObject menuPanel = null;
        private Component textRequest = null;
        private GameObject nowPivot = null;
        private GameObject nowVideo = null;

        public delegate void StopSE();
        public delegate void PlaySE(AudioClip clip, float pitch = 1.0f, bool loop = false);
        public delegate void ChangeState(nowState state, GameObject _object = null);

        public StopSE stopSE;
        public PlaySE playSE;
        public ChangeState changeState;

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

        }

        public void showGameUI()
        {
            if (gameUI == null)
            {
                if (GameUI == null)
                    return;

                gameUI = GameObject.Instantiate(GameUI) as GameObject;
                RectTransform rect = GameObject.Find("Canvas").GetComponent<RectTransform>();
                gameUI.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
                gameUI.transform.SetParent(rect);
                gameUI.transform.localPosition = Vector2.zero;
            }
            else
            {
                gameUI.SetActive(true);
            }
        }

        public void hideGameUI()
        {
            if (gameUI != null)
            {
                gameUI.SetActive(false);
            }
        }

        public GameObject getGameUI()
        {
            return gameUI;
        }

        public void showVideoPanel(UnityEngine.Video.VideoClip clip,Func call)
        {
            if(nowVideo==null)
            {
                nowVideo = GameObject.Instantiate(videoPanel) as GameObject;
                RectTransform rect = GameObject.Find("Canvas").GetComponent<RectTransform>();
                nowVideo.GetComponent<RectTransform>().sizeDelta = rect.sizeDelta;
                nowVideo.transform.SetParent(rect);
                nowVideo.transform.localPosition = Vector2.zero;
                ForVideo f = nowVideo.GetComponent<ForVideo>();
                f.calls += delegate { nowVideo = null; };
                f.playVideo(clip, call);
            }
            else
            {
                nowVideo.SetActive(true);
            }
        }

        public void showPivot()
        {
            if(nowPivot==null)
            {
                nowPivot = GameObject.Instantiate(attackPivot) as GameObject;
                nowPivot.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                nowPivot.transform.localPosition = Vector2.zero;
            }
            else
            {
                nowPivot.SetActive(true);
            }
        }

        public void hidePivot()
        {
            if (nowPivot != null)
            {
                nowPivot.SetActive(false);
            }
        }

        //淡入，使用协程执行，可能需要手动写等待时间
        public void beBlack(float time, bool black)
        {
            if (black)
            {
                if (blackPanel == null)
                {
                    GameObject go = GameObject.Instantiate(BlackPanel) as GameObject;
                    blackPanel = go;
                    go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                    go.GetComponent<RectTransform>().localPosition = Vector2.zero;
                    go.GetComponent<BlackPanel>().beBlack(time);
                }
                else
                {
                    if (whitePanel != null)
                        blackPanel.transform.localPosition = new Vector3(blackPanel.transform.localPosition.x, blackPanel.transform.localPosition.y, whitePanel.transform.localPosition.z - 1);
                    blackPanel.GetComponent<BlackPanel>().beBlack(time);
                }
            }
            else
            {
                if (whitePanel == null)
                {
                    GameObject go = GameObject.Instantiate(WhitePanel) as GameObject;
                    whitePanel = go;
                    go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                    go.GetComponent<RectTransform>().localPosition = Vector2.zero;
                    go.GetComponent<BlackPanel>().beBlack(time);
                }
                else
                {
                    if (blackPanel != null)
                        whitePanel.transform.localPosition = new Vector3(whitePanel.transform.localPosition.x, whitePanel.transform.localPosition.y, blackPanel.transform.localPosition.z - 1);
                    whitePanel.GetComponent<BlackPanel>().beBlack(time);
                }
            }
        }

        //淡出，使用协程执行，可能需要手动写等待时间
        public void beWhite(float time, bool black)
        {
            if (black)
            {
                if (blackPanel == null)
                {
                    GameObject go = GameObject.Instantiate(BlackPanel) as GameObject;
                    blackPanel = go;
                    go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                    go.GetComponent<RectTransform>().localPosition = Vector2.zero;
                    Color color = go.GetComponent<RawImage>().color;
                    color.a = 1;
                    go.GetComponent<RawImage>().color = color;
                    go.GetComponent<BlackPanel>().beWhite(time);
                }
                else
                {
                    blackPanel.GetComponent<BlackPanel>().beWhite(time);
                }
            }
            else
            {
                if (whitePanel == null)
                {
                    GameObject go = GameObject.Instantiate(WhitePanel) as GameObject;
                    whitePanel = go;
                    go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                    go.GetComponent<RectTransform>().localPosition = Vector2.zero;
                    Color color = go.GetComponent<RawImage>().color;
                    color.a = 1;
                    go.GetComponent<RawImage>().color = color;
                    go.GetComponent<BlackPanel>().beWhite(time);
                }
                else
                {
                    whitePanel.GetComponent<BlackPanel>().beWhite(time);
                }
            }
        }

        //显示文字框
        public void showTextPanel(Component _object, string name, string text, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
        {
            AudioClip s;
            if (se == null)
                s = show;
            else
                s = se;
            textRequest = _object;
            if (textPanel == null)
            {
                GameObject go = GameObject.Instantiate(TextPanel) as GameObject;
                textPanel = go.GetComponent<TextPanel>();
                go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                go.transform.localPosition = Vector2.zero;
            }
            else if (_new)
            {
                Destroy(textPanel.gameObject);
                GameObject go = GameObject.Instantiate(TextPanel) as GameObject;
                textPanel = go.GetComponent<TextPanel>();
                go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                go.transform.localPosition = Vector2.zero;
            }
            else
            {
                textPanel.gameObject.SetActive(true);
                textPanel.changeSize();
            }
            changeState(nowState.text, textPanel.gameObject);
            textPanel.setNameText(name);
            textPanel.setAvator(sprite);
            showFinish = false;
            if (s != null)
                playSE(s, p, true);
            StartCoroutine(showText(text, 0.05f, s, p));
        }

        //显示文字框
        public void showSpecialTextPanel(Component _object, string text, Vector2 pos, AudioClip se = null, float p = 1.0f)
        {
            AudioClip s;
            if (se == null)
                s = show;
            else
                s = se;

            textRequest = _object;
            if (textPanel)
                Destroy(textPanel.gameObject);
            GameObject go = GameObject.Instantiate(SpecialTextPanel) as GameObject;
            textPanel = go.GetComponent<TextPanel>();
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.transform.localPosition = pos;
            changeState(nowState.text, textPanel.gameObject);

            showFinish = false;
            if (s != null)
                playSE(s, p, true);
            StartCoroutine(showText(text, 0.05f, s, p));
        }

        //隐藏文字框
        public void hideTextPanel()
        {
            if (textPanel != null)
            {
                if (textPanel.gameObject.tag == "specialText")
                {
                    Destroy(textPanel.gameObject);
                    textPanel = null;
                }
                else
                    textPanel.gameObject.SetActive(false);
                changeState(nowState.auto);
            }
            showFinish = false;
            canHide = false;
        }

        IEnumerator showText(string text, float showCharTime, AudioClip se, float p)
        {
            // 读两个字符麻烦，偷懒
            if (string.IsNullOrEmpty(text))
            {
                textPanel.setMainText("");
            }
            else
            {
                text = text.Replace("\\c[10]", "\\c[9]");

                char[] A = text.ToCharArray();
                string txt = "";
                int mark = 0;
                char type = '0';
                foreach (var a in A)
                {
                    if (!showFinish)
                    {
                        if (mark > 0)
                        {
                            switch (a)
                            {
                                case '\\': break;
                                case '|':
                                    if (se != null)
                                        stopSE();
                                    yield return new WaitForSeconds(0.5f);
                                    if (se != null)
                                        playSE(se, p, true);
                                    mark = 0; break;
                                case '.':
                                    if (se != null)
                                        stopSE();
                                    yield return new WaitForSeconds(0.25f);
                                    if (se != null)
                                        playSE(se, p, true);
                                    mark = 0; break;
                                case 'c': mark = 2; type = 'c'; break;
                                case '{':
                                    mark = 0;
                                    if (type == 's')
                                        txt += "</size>";
                                    else
                                        type = 's';
                                    txt += "<size=" + (int)(textPanel.mainText.fontSize * 1.3f) + ">"; break;
                                case '}':
                                    mark = 0;
                                    if (type == 's')
                                        txt += "</size>";
                                    else
                                        type = 's';
                                    txt += "<size=" + (int)(textPanel.mainText.fontSize * 0.8f) + ">"; break;
                                case '[': mark--; break;
                                case ']': mark--; break;
                                case '1': if (type == 'c') txt += "<color=#67c2ea>"; break;
                                case '2': if (type == 'c') txt += "<color=#f78f6d>"; break;
                                case '3': if (type == 'c') txt += "<color=#7f9782>"; break;
                                case '4': if (type == 'c') txt += "<color=#99ccff>"; break;
                                case '6': if (type == 'c') txt += "<color=#ffffa0>"; break;
                                case '9': if (type == 'c') txt += "<color=#f45331>"; break;
                                case '0': if (type == 'c') { type = '0'; txt += "</color>"; } break;
                                default: mark = 0; break;
                            }
                        }
                        else
                        {
                            if (a == '\\')
                            {
                                mark = 1;
                            }
                            else
                            {
                                txt += a;
                                if (type == 'c')
                                {
                                    textPanel.setMainText(txt + "</color>");
                                }
                                else if (type == 's')
                                {
                                    textPanel.setMainText(txt + "</size>");
                                }
                                else
                                {
                                    textPanel.setMainText(txt);
                                }
                                yield return new WaitForSeconds(showCharTime);
                            }
                        }
                    }
                    else
                    {
                        text = text.Replace("\\|", "");
                        text = text.Replace("\\^", "");
                        text = text.Replace("\\.", "");
                        text = text.Replace("\\!", "");
                        text = text.Replace("\\c[0]", "</color>");
                        text = text.Replace("\\c[1]", "<color=#67c2ea>");
                        text = text.Replace("\\c[2]", "<color=#f78f6d>");
                        text = text.Replace("\\c[3]", "<color=#7f9782>");
                        text = text.Replace("\\c[4]", "<color=#99ccff>");
                        text = text.Replace("\\c[6]", "<color=#ffffa0>");
                        text = text.Replace("\\c[9]", "<color=#f45331>");

                        int num = Regex.Split(text, "\\{|\\}").Length - 1;
                        if (text.Contains("\\{"))
                        {
                            text = text.Replace("\\{", "<size=" + (int)(textPanel.mainText.fontSize * 1.3f) + ">");
                        }
                        if (text.Contains("\\}"))
                        {
                            text = text.Replace("\\}", "<size=" + (int)(textPanel.mainText.fontSize * 0.8f) + ">");
                        }
                        for (int i = 0; i < num; i++)
                            text += "</size>";

                        textPanel.setMainText(text);
                        break;
                    }
                }
                if (se != null)
                    stopSE();
            }

            showFinish = true;
            canHide = true;
        }

        public void showHint(string text, int windowSize, int oriSize)
        {
            GameObject go = GameObject.Instantiate(hint) as GameObject;
            go.GetComponentInChildren<Text>().text = text;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.transform.localPosition = new Vector2(0, -50 / oriSize * windowSize);
            Destroy(go, 3);
        }

        //直接显示完成所有文字
        public void skip()
        {
            showFinish = true;
            stopSE();
        }

        public bool checkTextRequet(Component _object)
        {
            return (_object == textRequest);
        }

        public void showMenuPanel()
        {
            GameObject go = GameObject.Instantiate(menu) as GameObject;
            menuPanel = go;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.transform.localPosition = Vector2.zero;
            changeState(nowState.window, menuPanel.gameObject);
        }

        public void showSavePanel(Func call = null)
        {
            GameObject go = GameObject.Instantiate(save) as GameObject;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.transform.localPosition = Vector2.zero;
            go.GetComponent<loadPanel>().addAfterDisEvent(call);
            changeState(nowState.window, go);
        }

        //显示任意菜单
        public GameObject showAnyPanel(GameObject _panel, Vector2 _pos, bool mainUI, nowState nowstate)
        {
            if (nowstate != nowState.window || mainUI)
            {
                GameObject go = GameObject.Instantiate(_panel) as GameObject;
                changeState(nowState.window, go);
                go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                basePanel basepanel = _panel.GetComponent<basePanel>();
                if (basepanel != null)
                {
                    basepanel.changeStartPos(_pos.x, _pos.y);
                }
                go.transform.localPosition = _pos;
                return go;
            }
            else
                return null;
        }

        public GameObject showCanDoHint(Vector3 _pos, float scale = 1)
        {
            GameObject go = GameObject.Instantiate(canDoHint) as GameObject;
            var canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
            go.transform.SetParent(canvas);

            CanDoHint basepanel = go.GetComponent<CanDoHint>();
            basepanel.changeScale(scale);
            if (basepanel != null)
            {
                basepanel.Pos = _pos;
            }

            return go;
        }

        public GameObject showFlash(Color c, float time)
        {
            GameObject go = GameObject.Instantiate(picPanel) as GameObject;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.transform.localPosition = Vector2.zero;
            PicPanel panel = go.GetComponent<PicPanel>();
            panel.setColor(c);
            panel.movePic(go.transform.localPosition, 0, time, true);
            return go;
        }

        public GameObject showPicPanel(string index, Vector2 _pos, Sprite sp, float a)
        {
            GameObject go = GameObject.Instantiate(picPanel) as GameObject;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            PicPanel panel = go.GetComponent<PicPanel>();
            panel.setPic(sp);
            panel.setSize(sp.texture.width, sp.texture.height);
            panel.setAlpha(a);
            panel.changeStartPos(_pos.x, _pos.y);
            go.transform.localPosition = _pos;
            if(images.ContainsKey(index))
            {
                hidePicPanel(index);
            }
            images.Add(index, go);
            if(blackPanel)
            {
                blackPanel.transform.SetAsLastSibling();
            }
            return go;
        }

        public void movePicPanel(string index, Vector2 pos, float a, float time)
        {
            images[index].GetComponent<PicPanel>().movePic(pos, a, time);
        }

        public void hidePicPanel(string index)
        {
            if (!images.ContainsKey(index))
                return;

            GameObject g = images[index];
            images.Remove(index);
            Destroy(g);
        }

        public GameObject showButton(string index, Vector2 _pos, GameObject btn, Func call)
        {
            GameObject go = GameObject.Instantiate(btn) as GameObject;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.transform.localPosition = _pos;
            Button b = go.GetComponent<Button>();
            b.onClick.AddListener(delegate { call(); });
            if (btns.ContainsKey(index))
                hideButton(index);
            btns.Add(index, b);
            if (blackPanel)
            {
                blackPanel.transform.SetAsLastSibling();
            }
            return go;
        }

        public void hideButton(string index)
        {
            if (!btns.ContainsKey(index))
                return;

            Button g = btns[index];
            btns.Remove(index);
            Destroy(g.gameObject);
        }

        public void showChoosePanel(string text, Func yes, Func no, nowState nowstate, GameObject mainPanel = null, bool black = false)
        {
            GameObject g = showAnyPanel(choosePanel, Vector2.zero, true, nowstate) as GameObject;
            var c = g.GetComponent<choosePanel>();
            c.ChooseText = text;
            c.yesFunc = yes;
            c.noFunc = no;
            c.setMenuPanel(mainPanel);
            if (black)
                c.BlackBackground();
        }

        public void showEventChoosePanel(string maintext, Func[] choices, string[] texts, nowState nowstate, GameObject mainPanel = null, bool black = false)
        {
            GameObject g = showAnyPanel(eventchoosePanel, Vector2.zero, true, nowstate) as GameObject;
            int num = choices.Length;
            var t = g.GetComponentInChildren<keyBoardMenuText>();
            t.optionNum.y = num;
            for(int i=0;i<num;i++)
            {
                t.texts[i].text = texts[i];
            }
            
            var c = g.GetComponent<eventChoosePanel>();
            c.ChooseText = maintext;
            c.Funcs = choices;
            c.setMenuPanel(mainPanel);
            if (black)
                c.BlackBackground();
        }

        public GameObject getMainMenu()
        {
            if (menuPanel != null && menuPanel.activeInHierarchy)
            {
                return menuPanel;
            }
            else
                return null;
        }

        public void setMainMenu(GameObject menu)
        {
            menuPanel = menu;
        }
    }
}
