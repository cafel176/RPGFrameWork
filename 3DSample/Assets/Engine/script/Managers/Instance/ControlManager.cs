using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagerSpace
{
    /*
     * 管理用户输入和相关响应的控制器
     */
    [DisallowMultipleComponent]
    public class ControlManager : MonoBehaviour
    {
        public static ControlManager instance;

        //每帧的用户输入列表
        private List<keyInput> inputs = new List<keyInput>();
        public List<keyInput> Inputs
        {
            get
            {
                return inputs;
            }
        }

        private List<mouseInput> mouseInputs = new List<mouseInput>();
        public List<mouseInput> MouseInputs
        {
            get
            {
                return mouseInputs;
            }
        }

        private float mouseX;
        public float MouseX
        {
            get
            {
                return mouseX;
            }
        }

        private float mouseY;
        public float MouseY
        {
            get
            {
                return mouseY;
            }
        }

        private float mouseScrollWheel;
        public float MouseScrollWheel
        {
            get
            {
                return mouseScrollWheel;
            }
        }

        public event changeStateCallbaks callbackState;

        //记录当前操作状态
        private nowState nowstate = nowState.window;
        public nowState NowState
        {
            get
            {
                return nowstate;
            }
        }

        private controlMode nowMode = controlMode.control_2D;
        public controlMode NowMode
        {
            get
            {
                return nowMode;
            }
            set
            {
                nowMode = value;
            }
        }

        //记录当前操作窗口的引用
        private GameObject nowWindow = null;
        public GameObject NowWindow
        {
            get
            {
                return nowWindow;
            }
        }

        //记录当前操作角色
        private GameObject player = null;
        public GameObject NowPlayer
        {
            get
            {
                return player;
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

        }

        public void clearInput()
        {
            inputs.Clear();
            mouseInputs.Clear();
        }

        //玩家输入控制
        private void checkInput()
        {
            clearInput();

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetMouseButton(0))
                mouseInputs.Add(mouseInput.left);

            if (Input.GetMouseButtonDown(0))
                mouseInputs.Add(mouseInput.leftDown);

            // 可连续
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                inputs.Add(keyInput.up);
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                inputs.Add(keyInput.down);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                inputs.Add(keyInput.left);
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                inputs.Add(keyInput.right);
            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                inputs.Add(keyInput.shift);
            }

            //仅单次
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                inputs.Add(keyInput.upOnce);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                inputs.Add(keyInput.downOnce);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                inputs.Add(keyInput.leftOnce);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                inputs.Add(keyInput.rightOnce);
            }
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                inputs.Add(keyInput.confirm);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                inputs.Add(keyInput.jump);
            }
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                inputs.Add(keyInput.cancel);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                inputs.Add(keyInput.menu);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                inputs.Add(keyInput.ctrl);
            }
        }



        //用于改变当前的交互状态
        public void changeState(nowState state, GameObject _object = null)
        {
            if (state == nowState.none)
                return;

            if(Cursor.lockState != CursorLockMode.None)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            nowstate = state;
            if (callbackState != null)
                callbackState(nowstate);
            if (state == nowState.window)
            {
                if (_object != null)
                {
                    nowWindow = _object;
                }
                if(nowWindow!=null)
                {
                    nowWindow.SetActive(true);
                }
            }
            else if (state == nowState.move)
            {
                if (_object != null)
                    player = _object;
                if (player != null)
                {
                    Camera3DFPS cfps = player.GetComponentInChildren<Camera3DFPS>();
                    if (cfps != null)
                    {
                        nowMode = controlMode.control_3DFPS;
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        return;
                    }

                    // 3d
                    Camera3D c3d = Camera.main.GetComponent<Camera3D>();
                    if (c3d != null)
                    {
                        nowMode = controlMode.control_3D;
                        c3d.player = player.gameObject;
                        return;
                    }

                    // 2d版
#if use2D
                    CameraFollow2D c2d = Camera.main.GetComponent<CameraFollow2D>();
                    if (c2d != null)
                    {
                        nowMode = controlMode.control_2D;
                        c2d.player = player.gameObject;
                        return;
                    }
#endif
                }
            }
        }

        public void startUserControl(GameObject _player = null)
        {
            changeState(nowState.move, _player);
        }

        //停止玩家控制，不卡进程
        public void stopUserControl()
        {
            changeState(nowState.auto);
        }

        // 在Update中每帧执行的操作
        public void doEveryFrame()
        {
            checkInput();
        }
    }
}


