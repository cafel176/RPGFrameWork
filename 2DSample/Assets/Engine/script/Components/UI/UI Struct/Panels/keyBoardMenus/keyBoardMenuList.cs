using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace UI
{
    public enum listAxis
    {
        none = -1,
        Horizontal = 0,
        Vertical = 1
    }

    public class keyBoardMenuList : keyBoardMenu, funcable
    {
        public listItemUI listItemPrefab;

        public bool spawnFirst = false;

        // 翻页朝向，none是单页循环
        public listAxis axis = listAxis.Vertical;

        public showType showType = showType.all;

        public delegate void moveItemFunc(string index);

        public event Func cancelCallback;

        public event moveItemFunc afterUpdateItems;

        public delegate void moveCallFunc(string i);

        public moveCallFunc callbackLeft, callbackRight, callbackUp, callbackDown;

        protected List<listItemUI> listItems = new List<listItemUI>();

        protected GameObject parent;

        [SerializeField]
        protected keyBoardMenuList child;

        protected string param = "";

        protected int page = 0;
        protected int allPage = 0;

        //===============================初始化操作=================================

        protected override void onAwake()
        {
            base.onAwake();
            cursor.SetActive(false);
        }

        virtual public void spawanItems()
        {
            if (listItemPrefab != null)
            {
                for (int j = 0; j < optionNum.y; j++)
                    for (int i = 0; i < optionNum.x; i++)
                    {
                        if(!spawnFirst)
                        {
                            if (i == 0 && j == 0)
                            {
                                listItemPrefab.NoneImg = data.NoneImg;
                                listItemPrefab.NoneText = data.NoneText;
                                listItems.Add(listItemPrefab);
                                continue;
                            }
                        }
                        GameObject g = Instantiate(listItemPrefab.gameObject) as GameObject;
                        g.transform.SetParent(gameObject.transform);
                        g.transform.localScale = listItemPrefab.transform.localScale;
                        g.transform.localPosition = startItemsPos + new Vector3(i * horizontalMove, j * verticalMove);
                        var f = g.GetComponent<listItemUI>();
                        f.NoneImg = data.NoneImg;
                        f.NoneText = data.NoneText;
                        listItems.Add(f);
                    }
            }
        }

        //==============================功能操作=================================
        public override void cancel()
        {
            base.cancel();

            cursor.SetActive(false);
            changeState(nowState.window, parent);
            if (showType == showType.tree)
                ableDis(afterDis.hide);
            parent.GetComponent<funcable>().doFunc();

            if (cancelCallback != null)
                cancelCallback();
        }

        public override void doOption()
        {
            base.doOption();

            if (child != null)
                child.doFunc(getItemKey(getCursorPosIndex()), gameObject);
            doItemFunc(getCursorPosIndex());
        }

        public override void left()
        {
            base.left();

            changeCursorPos(-1);
            if(callbackLeft!=null)
                callbackLeft(getItemKey(getCursorPosIndex()));

            updateItems();
        }

        public override void right()
        {
            base.right();

            changeCursorPos(1);
            if(callbackRight!=null)
                callbackRight(getItemKey(getCursorPosIndex()));

            updateItems();
        }

        public override void up()
        {
            base.up();

            changeCursorPos(0, -1);
            if(callbackUp!=null)
                callbackUp(getItemKey(getCursorPosIndex()));

            updateItems();


        }

        public override void down()
        {
            base.down();

            changeCursorPos(0, 1);
            if(callbackDown!=null)
                callbackDown(getItemKey(getCursorPosIndex()));

            updateItems();
        }

        override protected Vector2Int getNextPos(int horizontal, int vertical = 0)
        {
            int[] p = { 0, 0 };
            int[] move = { horizontal, vertical };
            int[] cp = { cursorPos.x, cursorPos.y };
            int[] on = { optionNum.x, optionNum.y };

            if(axis == listAxis.none)
            {
                p[0] = (cp[0] + move[0] + on[0]) % on[0];
                p[1] = (cp[1] + move[1] + on[1]) % on[1];
            }
            else
            {
                int main = (int)axis;
                int other = (main == 0 ? 1 : 0);

                p[other] = (cp[other] + move[other] + on[other]) % on[other];
                int next = cp[main] + move[main];
                p[main] = (next + on[main]) % on[main];
                if (next < 0)
                {
                    if (page == 0)
                    {
                        p[main] = 0;
                    }
                    else
                    {
                        page -= 1;
                    }
                }
                else if (next >= on[main])
                {
                    if (page == allPage - 1)
                    {
                        p[main] = cursorPos[main];
                    }
                    else
                    {
                        page += 1;
                    }
                }
            }

            return new Vector2Int(p[0], p[1]);
        }

        //===============================被父节点操作=================================
        public void doFunc(string _param, GameObject _parent)
        {
            parent = _parent;
            param = _param;

            doFunc();
        }

        public void updateItems(string _param = "")
        {
            doUpdateItems(_param);

            if (afterUpdateItems != null)
                afterUpdateItems(getItemKey(getCursorPosIndex()));
        }

        virtual protected void doUpdateItems(string _param = "")
        {
            string p = string.IsNullOrEmpty(_param) ? param : _param;
            List<ListItemData> datas = data.getDatas(index, p);
            List<string> keys = data.getKeys(index, p);

            allPage = (int)Math.Ceiling((float)datas.Count / getMaxNum());
            if (page > allPage)
            {
                page = 0;
                resetCurosr();
            }
            int startIndex = page * getMaxNum();
            clearAll();
            if (datas != null && keys != null)
                for (int i = 0; i < listItems.Count; i++)
                {
                    if (i + startIndex < datas.Count && i + startIndex < keys.Count)
                    {
                        setKey(i, keys[i + startIndex]);
                        setData(i, datas[i + startIndex]);
                    }
                    else
                        setData(i, null);
                }
        }

        override public void setDataSource(int _index, keyBoardMenusController dataSource)
        {
            base.setDataSource(_index, dataSource);
        }

        public List<listItemUI> getlistItems()
        {
            return listItems;
        }

        public int getlistLength()
        {
            return listItems.Count;
        }

        //===============================被子节点操作=================================
        public void doFunc()
        {
            cursor.SetActive(true);
            changeState(nowState.window, gameObject);
        }

        //===============================对子节点操作=================================
        public string getItemKey(int i)
        {
            return listItems[i].Key;
        }

        public void setKey(int index, string key)
        {
            if (index < 0 || index >= listItems.Count)
                return;

            listItems[index].setKeyIfNotNull(key);
        }

        public void setData(int index, ListItemData data)
        {
            if (index < 0 || index >= listItems.Count)
                return;

            listItems[index].setDataIfNotNull(data);
        }

        public void clearAll()
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                listItems[i].clearData();
                listItems[i].clearKey();
            }
        }

        public void doItemFunc(int _index, string _param)
        {
            listItems[_index].doFunc(_param, gameObject);
        }

        public void doItemFunc(int _index)
        {
            listItems[_index].doFunc();
        }

        public void changeBtn(int _index,bool able)
        {
            var b = listItems[_index].gameObject.GetComponent<Button>();
            b.interactable = able;
        }
    }
}
