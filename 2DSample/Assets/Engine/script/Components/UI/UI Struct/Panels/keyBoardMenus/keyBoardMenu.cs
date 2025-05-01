using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum showType
    {
        all,
        tree
    }

    public enum afterDis
    {
        destroy,
        hide
    }

    public abstract class keyBoardMenu : basePanel, keyboardMenuInterface
    {
        //光标
        [SerializeField]
        protected GameObject cursor;

        public Vector3 startCursorPos;
        public Vector3 startItemsPos;
        //每次移动的方向
        public float horizontalMove;
        public float verticalMove;
        //这个菜单选项总数
        public Vector2Int optionNum;

        //记录当前光标位置
        protected Vector2Int cursorPos;

        protected keyBoardMenusController data;
        protected int index = -1;

        protected bool changeScale = false;
        protected Vector3 nowScale = Vector3.one;
        protected Vector3 targetScale = Vector3.one;

        protected float ableTime = 0.1f, ableTimer = 0;

        protected afterDis afterDis = afterDis.destroy;

        // 相关音效
        protected AudioClip yes;
        protected AudioClip no;
        protected AudioClip unable;
        protected AudioClip move;

        override protected void onStart()
        {
            base.onStart();

            resetCurosr();

            yes = findAudio("确定");
            no = findAudio("取消");
            unable = findAudio("无效");
            move = findAudio("光标");
        }


        private void Update()
        {
            doEveryFrame();
        }

        public override void doEveryFrame()
        {
            base.doEveryFrame();

            if (changeScale)
            {
                gameObject.transform.localScale += (targetScale - nowScale) / ableTime * Time.deltaTime;
                ableTimer += Time.deltaTime;
                if (ableTimer >= ableTime)
                {
                    gameObject.transform.localScale = targetScale;
                    ableTimer = 0;
                    changeScale = false;
                    if (Mathf.Abs(targetScale.y) <= 0.001f)
                    {
                        if (afterDis == afterDis.destroy)
                        {
                            Destroy(data.gameObject);
                            Destroy(gameObject);
                        }
                        else if (afterDis == afterDis.hide)
                            hide();
                    }
                }
            }
        }

        void hide()
        {
            transform.localScale = nowScale;
            gameObject.SetActive(false);
        }

        override protected void ableInit()
        {
            base.ableInit();

            nowScale = gameObject.transform.localScale;
            nowScale.y = 0;
            targetScale = startScale;
            gameObject.transform.localScale = nowScale;
            ableTimer = 0;
            changeScale = true;
        }

        public void ableDis(afterDis ad)
        {
            afterDis = ad;

            nowScale = startScale;
            targetScale = gameObject.transform.localScale;
            targetScale.y = 0;
            ableTimer = 0;
            changeScale = true;
        }

        protected void resetCurosr()
        {
            cursorPos = Vector2Int.zero;
            cursor.transform.localPosition = startCursorPos;
        }

        public int getMaxNum()
        {
            return optionNum.x * optionNum.y;
        }

        public int getCursorPosIndex()
        {
            return cursorPos.x + (optionNum.x * cursorPos.y);
        }

        protected void changeCursorPos(int horizontal, int vertical = 0)
        {
            if (getMaxNum() > 1)
            {
                setCursorPos(getNextPos(horizontal, vertical));
            }
        }

        virtual protected Vector2Int getNextPos(int horizontal, int vertical = 0)
        {
            Vector2Int p = Vector2Int.zero;
            p.x = (cursorPos.x + horizontal + optionNum.x) % optionNum.x;
            p.y = (cursorPos.y + vertical + optionNum.y) % optionNum.y;
            return p;
        }

        public void setCursorPos(Vector2Int pos)
        {
            setCursorPos(pos.x, pos.y);
        }

        public void setCursorPos(int horizontal, int vertical)
        {
            cursorPos.x = horizontal;
            cursorPos.y = vertical;
            cursor.transform.localPosition = startCursorPos;
            cursor.transform.localPosition += new Vector3(horizontal * horizontalMove, vertical * verticalMove);
        }

        public virtual void doOption()
        {
            data.playSE(yes, true);
        }

        public virtual void cancel()
        {
            data.playSE(no, true);
        }

        public virtual void left()
        {
            data.playSE(move);
        }

        public virtual void right()
        {
            data.playSE(move);
        }

        public virtual void up()
        {
            data.playSE(move);
        }

        public virtual void down()
        {
            data.playSE(move);
        }

        virtual public void setDataSource(int _index, keyBoardMenusController dataSource)
        {
            data = dataSource;

            index = _index;
        }
    }
}
