using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class eventChoosePanel : keyBoardMenusController
    {
        public Func[] Funcs;

        public string ChooseText
        {
            set
            {
                setShow(new ListItemData(new string[] { value }), showPanels[0]);
            }
        }

        public void BlackBackground()
        {
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, (168.0f / 255));
        }

        public void choice1()
        {
            exit();
            if (Funcs.Length>0 && Funcs[0] != null)
                Funcs[0]();
        }

        public void choice2()
        {
            exit();
            if (Funcs.Length > 1 && Funcs[1] != null)
                Funcs[1]();
        }

        public void choice3()
        {
            exit();
            if (Funcs.Length > 2 && Funcs[2] != null)
                Funcs[2]();
        }

        override public List<ListItemData> getDatas(int index, string param)
        {
            return null;
        }

        override public List<string> getKeys(int index, string param)
        {
            return null;
        }
    }
}

