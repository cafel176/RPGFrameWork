using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //管理器的实例
    public static UIManager instance;

    //用于淡入淡出的UI
    public GameObject BlackPanel;
    public GameObject WhitePanel;
    //用于显示文字的UI
    public GameObject TextPanel;
    //用于提示的UI
    public GameObject hint;
    //菜单UI
    public GameObject menu;
    // 显示文字时的音效
    public AudioClip show;

    //=====================不可见变量===============================
    //控制文字显示进度的开关
    [HideInInspector]
    public bool showFinish = true;

    //引用
    private GameObject blackPanel = null;
    private GameObject whitePanel = null;
    private TextPanel textPanel = null;
    private GameObject menuPanel = null;
    private GameObject textRequest = null;

    private void Awake()
    {
        //创造管理器实例
        if (UIManager.instance == null)
        {
            UIManager.instance = this;
            DontDestroyOnLoad(UIManager.instance.gameObject);
        }
        else if (UIManager.instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    //淡入，使用协程执行，可能需要手动写等待时间
    public void beBlack(float smoothing, bool black)
    {
        if (black)
        {
            if (blackPanel == null)
            {
                GameObject go = GameObject.Instantiate(BlackPanel) as GameObject;
                blackPanel = go;
                go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
                go.GetComponent<RectTransform>().localPosition = Vector2.zero;
                go.GetComponent<BlackPanel>().beBlack(smoothing);
            }
            else
            {
                if (whitePanel != null)
                    blackPanel.transform.localPosition = new Vector3(blackPanel.transform.localPosition.x, blackPanel.transform.localPosition.y, whitePanel.transform.localPosition.z - 1);
                blackPanel.GetComponent<BlackPanel>().beBlack(smoothing);
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
                go.GetComponent<BlackPanel>().beBlack(smoothing);
            }
            else
            {
                if (blackPanel != null)
                    whitePanel.transform.localPosition = new Vector3(whitePanel.transform.localPosition.x, whitePanel.transform.localPosition.y, blackPanel.transform.localPosition.z - 1);
                whitePanel.GetComponent<BlackPanel>().beBlack(smoothing);
            }
        }
    }

    //淡出，使用协程执行，可能需要手动写等待时间
    public void beWhite(float smoothing, bool black)
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
                go.GetComponent<BlackPanel>().beWhite(smoothing);
            }
            else
            {
                blackPanel.GetComponent<BlackPanel>().beWhite(smoothing);
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
                go.GetComponent<BlackPanel>().beWhite(smoothing);
            }
            else
            {
                whitePanel.GetComponent<BlackPanel>().beWhite(smoothing);
            }
        }
    }

    //显示文字框
    public void showTextPanel(GameObject _object, string name, string text, Sprite sprite, bool _new = false)
    {
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
        gameManager.instance.changeState(nowState.text, textPanel.gameObject);
        textPanel.setNameText(name);
        textPanel.setAvator(sprite);
        showFinish = false;
        gameManager.instance.playSE(show, true);
        StartCoroutine(showText(text, 0.05f));
    }

    //隐藏文字框
    public void hideTextPanel()
    {
        showFinish = true;
        if (textPanel != null)
        {
            textPanel.gameObject.SetActive(false);
            gameManager.instance.changeState(nowState.move);
        }
    }

    IEnumerator showText(string text, float showCharTime)
    {
        char[] A = text.ToCharArray();
        string txt = "";
        foreach (var a in A)
        {
            if (!showFinish)
            {
                txt += a;
                textPanel.setMainText(txt);
                yield return new WaitForSeconds(showCharTime);
            }
            else
            {
                textPanel.setMainText(text);
                break;
            }
        }
        gameManager.instance.stopSE();
        showFinish = true;
    }

    public void showHint(string text)
    {
        GameObject go = GameObject.Instantiate(hint) as GameObject;
        go.GetComponentInChildren<Text>().text = text;
        go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        go.transform.localPosition = new Vector2(0, -50 / gameManager.instance.oriSize * gameManager.instance.setting.windowSize);
        Destroy(go, 3);
    }

    //直接显示完成所有文字
    public void skip()
    {
        gameManager.instance.stopSE();
        showFinish = true;
    }

    public bool checkTextRequet(GameObject _object)
    {
        return (_object == textRequest);
    }

    public void showMenuPanel()
    {
            GameObject go = GameObject.Instantiate(menu) as GameObject;
            menuPanel = go;
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.transform.localPosition = Vector2.zero;
            gameManager.instance.changeState(nowState.window, menuPanel.gameObject);
    }

    //显示任意菜单
    public GameObject showAnyPanel(GameObject _panel, Vector2 _pos, bool mainUI)
    {
        if (gameManager.instance.nowstate != nowState.window || mainUI)
        {
            GameObject go = GameObject.Instantiate(_panel) as GameObject;
            gameManager.instance.changeState(nowState.window, go);
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

    public GameObject getMainMenu()
    {
        if(menuPanel!=null&&menuPanel.activeInHierarchy)
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
