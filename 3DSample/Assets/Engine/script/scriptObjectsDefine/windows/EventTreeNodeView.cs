using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace BaseData
{
#if UNITY_EDITOR

    public class EventTreeNodeView : myTreeNodeView<myEvent>
    {
        private EnumField type;
        private ObjectField helper;
        private FloatField float1, float2;
        private TextField str1, str2, str3, str4;
        private IntegerField int1;
        private Toggle bool1, bool2;
        private Vector3Field vec1, vec2;
        private ColorField color1;
        private EnumField weather, howToCal, turn, findType, questStatus, independentSwitch, compare, conditionType;

        public EventTreeNodeView(EventTreeGraphView graph, object data) : base(graph,data)
        {
            e = new myEvent();
            e.id = GetTimeStamp().ToString();
            e.type = (eventType)data;

            addParent(null);

            EventType();
            showDetails();

            addChildren();

            RefreshExpandedState();
            RefreshPorts();
        }

        protected long GetTimeStamp()
        {
            TimeSpan timeStamp = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(timeStamp.TotalSeconds);
        }

        public EventTreeNodeView(EventTreeGraphView graph, myEvent node, Node parent): base(graph,node, parent)
        {
            e = node;

            var r = addParent(parent);
            if(r!=null)
                graph.AddElement(r);

            EventType();
            showDetails();

            addChildren();

            RefreshExpandedState();
            RefreshPorts();
        }

        private Edge addParent(Node parent)
        {
            Edge edge = null;
            if (e.getId() != "root")
            {
                var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
                inputPort.portName = "父节点";

                if (parent != null)
                {
                    myEvent p = ((EventTreeNodeView)parent).data;
                    if (p.type == eventType.If)
                    {
                        if (p.str3 == e.getId())
                            edge = inputPort.ConnectTo((Port)parent.outputContainer.ElementAt(0));
                        else
                            edge = inputPort.ConnectTo((Port)parent.outputContainer.ElementAt(1));
                    }
                    else if (p.type == eventType.choicePanel)
                    {
                        if (p.str2.Split('#')[1] == e.getId())
                            edge = inputPort.ConnectTo((Port)parent.outputContainer.ElementAt(0));
                        else if (p.str3.Split('#')[1] == e.getId())
                            edge = inputPort.ConnectTo((Port)parent.outputContainer.ElementAt(1));
                        else
                            edge = inputPort.ConnectTo((Port)parent.outputContainer.ElementAt(2));
                    }
                    else if (p.type == eventType.showButton)
                    {
                        if (p.str3 == e.getId())
                            edge = inputPort.ConnectTo((Port)parent.outputContainer.ElementAt(0));
                        else
                            edge = inputPort.ConnectTo((Port)parent.outputContainer.ElementAt(1));
                    }
                    else
                    {
                        edge = inputPort.ConnectTo((Port)parent.outputContainer.ElementAt(0));
                    }
                }

                inputContainer.Add(inputPort);
            }
            else // 根节点不可删除
            {
                capabilities -= Capabilities.Deletable;
            }

            return edge;
        }

        private void addChildren()
        {
            outputContainer.Clear();
            if (e.type == eventType.If)
            {
                addChild("符合");
                addChild("不符合");
            }
            else if (e.type == eventType.choicePanel)
            {
                addChild("选项1");
                addChild("选项2");
                addChild("选项3");
            }
            else if (e.type == eventType.showButton)
            {
                addChild("点击");
                addChild();
            }
            else
                addChild();
        }

        private void EventType()
        {
            title = myEvent.eventTypeToName(e.type);

            type = new EnumField();
            type.Init(e.type);
            var that = this;
            type.RegisterValueChangedCallback((evt) =>
            {
                e.type = (eventType)type.value;
                extensionContainer.Clear();
                EventType();
                showDetails();
                addChildren();
                RefreshExpandedState();
                RefreshPorts();
            });

            extensionContainer.Add(type);
        }

        private void showDetails()
        {
            extensionContainer.Add(new Label("节点id:" + e.getId()));

            if (e.type == eventType.text)
            {
                TextField1("名字");
                TextField2("文本");
                TextField3("立绘");
                BoolField1("是否显示立绘");
                TextField4("文字音效");
            }
            else if (e.type == eventType.textSpecial)// 会覆盖黑屏白屏
            {
                TextField1("文本");
                VecField1("位置坐标");
                HelperField();
                TextField2("文字音效");
            }
            else if (e.type == eventType.beBlack || e.type == eventType.beWhite || e.type == eventType.wait)
            {
                FloatField1("时间");
            }
            else if (e.type == eventType.changeGlobalInt)
            {
                TextField1("变量名");
                HowToCal();
                IntField1("值");
            }
            else if (e.type == eventType.changeGlobalDouble)
            {
                TextField1("变量名");
                HowToCal();
                FloatField1("值");
            }
            else if (e.type == eventType.changeGlobalVector3)
            {
                TextField1("变量名");
                HowToCal(); ;
                VecField1("值");
            }
            else if (e.type == eventType.changeGlobalSwith)
            {
                TextField1("变量名");
                BoolField1("值");
            }
            else if (e.type == eventType.debugLog)
            {
                TextField1("文本");
            }
            else if (e.type == eventType.playBgm)
            {
                TextField1("BGM名称");
            }
            else if (e.type == eventType.playBgs)
            {
                TextField1("BGS名称");
            }
            else if (e.type == eventType.playSE)
            {
                TextField1("SE名称");
            }
            else if (e.type == eventType.stopBgm)
            {
                FloatField1("时间");
            }
            else if (e.type == eventType.stopBgs)
            {
                FloatField1("时间");
            }
            else if (e.type == eventType.playVideo)
            {
                TextField1("视频名称");
            }
            else if (e.type == eventType.showPic)
            {
                TextField1("图片编号");
                TextField2("图片名");
                VecField1("位置坐标");
                HelperField();
                FloatField1("透明度");
            }
            else if (e.type == eventType.movePic)
            {
                TextField1("图片编号");
                VecField1("移动目标位置");
                HelperField();
                FloatField1("透明度");
                FloatField2("时间");
                BoolField1("卡进程");
            }
            else if (e.type == eventType.hidePic)
            {
                TextField1("图片编号");
            }
            else if (e.type == eventType.showButton)
            {
                TextField1("按钮编号");
                TextField2("按钮名");
                VecField1("位置坐标");
                HelperField();
            }
            else if (e.type == eventType.hideButton)
            {
                TextField1("按钮编号");
            }
            else if (e.type == eventType.changeScene)
            {
                TextField1("场景名");
                VecField1("位置");
                VecField2("新的朝向");
                HelperField(true);
                Turn();
            }
            else if (e.type == eventType.changeIndependentSwitch)
            {
                IndependentSwitch();
                BoolField1("开关");
            }
            else if (e.type == eventType.playAnima)
            {
                TextField1("预制体名");
                TextField2("动画名");
                VecField1("位置");
                HelperField();
                BoolField1("卡进程");
            }
            else if (e.type == eventType.actorPlayAnima)
            {
                FindType();
                TextField1("文本");
                TextField2("动画名");
                BoolField1("卡进程");
            }
            else if (e.type == eventType.moveCamera)
            {
                VecField1("移动目标位置");
                HelperField();
                FloatField1("时间");
            }
            else if (e.type == eventType.sbMoveToSw)
            {
                FindType();
                TextField1("文本");
                VecField1("移动目标位置");
                HelperField();
                Turn();
            }
            else if (e.type == eventType.changeTurn2D)
            {
                FindType();
                TextField1("文本");
                Turn();
            }
            else if (e.type == eventType.changeTurn3D)
            {
                FindType();
                TextField1("文本");
                VecField1("新的朝向");
            }
            else if (e.type == eventType.flashScreen)
            {
                ColorField1("颜色");
                FloatField1("时间");
                BoolField1("卡进程");
            }
            else if (e.type == eventType.shakeScreen)
            {
                FloatField1("时间");
                FloatField2("强度");
                BoolField1("卡进程");
            }
            else if (e.type == eventType.changeWeather)
            {
                Weather();
                FloatField1("强度");
            }
            else if (e.type == eventType.changeItem)
            {
                TextField1("道具id");
                IntField1("数量");
            }
            else if (e.type == eventType.changeThingPos)
            {
                FindType();
                TextField1("文本");
                VecField1("设置位置");
                HelperField();
            }
            else if (e.type == eventType.changeThingActive)
            {
                FindType();
                TextField1("文本");
                BoolField1("可见性");
            }
            else if (e.type == eventType.startQuest)
            {
                TextField1("任务id");
            }
            else if (e.type == eventType.changeQuest)
            {
                TextField1("任务id");
                QuestStatus();
            }
            else if (e.type == eventType.setFollow)
            {
                FindType();
                TextField1("文本");
                IntField1("对应PlayerPrefab编号");
                BoolField1("是否跟随");
            }
            else if (e.type == eventType.changeTeam)
            {
                IntField1("角色编号");
                BoolField1("入队出队");
            }
            else if (e.type == eventType.commonEvent)
            {
                TextField1("事件名");
            }
            else if (e.type == eventType.If)
            {
                Label("条件: ");
                ConditionType();
                TextField1("变量名");
                Compare();
                TextField2("值");
            }
            else if (e.type == eventType.choicePanel)
            {
                TextField1("菜单文本");

                str2 = new TextField("选项1文本");
                if (e.str2 == null)
                    e.str2 = "";
                string[] txts = e.str2.Split('#');
                if (txts.Length < 2)
                    txts = new string[] { e.str2, "" };
                str2.value = txts[0];
                str2.RegisterValueChangedCallback((evt) => { e.str2 = str2.value+"#"+ txts[1]; });
                extensionContainer.Add(str2);

                str3 = new TextField("选项2文本");
                if (e.str3 == null)
                    e.str3 = "";
                string[] txts2 = e.str3.Split('#');
                if (txts2.Length < 2)
                    txts2 = new string[] { e.str3, "" };
                str3.value = txts2[0];
                str3.RegisterValueChangedCallback((evt) => { e.str3 = str3.value + "#" + txts2[1]; });
                extensionContainer.Add(str3);

                str4 = new TextField("选项3文本");
                if (e.str4 == null)
                    e.str4 = "";
                string[] txts3 = e.str4.Split('#');
                if (txts3.Length < 2)
                    txts3 = new string[] { e.str4, "" };
                str4.value = txts3[0];
                str4.RegisterValueChangedCallback((evt) => { e.str4 = str4.value + "#" + txts3[1]; });
                extensionContainer.Add(str4);
            }
        }

        private void HelperField(bool rotate = false)
        {
            helper = new ObjectField();
            helper.objectType = typeof(Transform);
            Button btn = new Button();
            btn.text = "仅数值";
            btn.clicked += () => 
            {
                e.vec1 = ((Transform)helper.value).position;
                vec1.value = e.vec1;
                if(rotate)
                {
                    e.vec2 = ((Transform)helper.value).rotation.eulerAngles;
                    vec2.value = e.vec2;
                }
                helper.value = null;
            };

            extensionContainer.Add(helper);
            extensionContainer.Add(btn);
        }

        private void FloatField1(string label)
        {
            float1 = new FloatField(label);
            float1.value = e.float1;
            float1.RegisterValueChangedCallback((evt) => { e.float1 = float1.value; });
            extensionContainer.Add(float1);
        }

        private void FloatField2(string label)
        {
            float2 = new FloatField(label);
            float2.value = e.float2;
            float2.RegisterValueChangedCallback((evt) => { e.float2 = float2.value; });
            extensionContainer.Add(float2);
        }

        private void TextField1(string label)
        {
            str1 = new TextField(label);
            str1.value = e.str1;
            str1.RegisterValueChangedCallback((evt) => { e.str1 = str1.value; });
            extensionContainer.Add(str1);
        }

        private void TextField2(string label)
        {
            str2 = new TextField(label);
            str2.value = e.str2;
            str2.RegisterValueChangedCallback((evt) => { e.str2 = str2.value; });
            extensionContainer.Add(str2);
        }

        private void TextField3(string label)
        {
            str3 = new TextField(label);
            str3.value = e.str3;
            str3.RegisterValueChangedCallback((evt) => { e.str3 = str3.value; });
            extensionContainer.Add(str3);
        }

        private void TextField4(string label)
        {
            str4 = new TextField(label);
            str4.value = e.str4;
            str4.RegisterValueChangedCallback((evt) => { e.str4 = str4.value; });
            extensionContainer.Add(str4);
        }

        private void IntField1(string label)
        {
            int1 = new IntegerField(label);
            int1.value = e.int1;
            int1.RegisterValueChangedCallback((evt) => { e.int1 = int1.value; });
            extensionContainer.Add(int1);
        }

        private void BoolField1(string label)
        {
            bool1 = new Toggle(label);
            bool1.value = e.bool1;
            bool1.RegisterValueChangedCallback((evt) => { e.bool1 = bool1.value; });
            extensionContainer.Add(bool1);
        }

        private void BoolField2(string label)
        {
            bool2 = new Toggle(label);
            bool2.value = e.bool2;
            bool2.RegisterValueChangedCallback((evt) => { e.bool2 = bool2.value; });
            extensionContainer.Add(bool2);
        }

        private void VecField1(string label)
        {
            vec1 = new Vector3Field(label);
            vec1.value = e.vec1;
            vec1.RegisterValueChangedCallback((evt) => { e.vec1 = vec1.value; });
            extensionContainer.Add(vec1);
        }

        private void VecField2(string label)
        {
            vec2 = new Vector3Field(label);
            vec2.value = e.vec2;
            vec2.RegisterValueChangedCallback((evt) => { e.vec2 = vec2.value; });
            extensionContainer.Add(vec2);
        }

        private void ColorField1(string label)
        {
            color1 = new ColorField(label);
            color1.value = e.color1;
            color1.RegisterValueChangedCallback((evt) => { e.color1 = color1.value; });
            extensionContainer.Add(color1);
        }

        private void Label(string label)
        {
            extensionContainer.Add(new Label(label));
        }

        private void HowToCal()
        {
            howToCal = new EnumField("计算方式");
            howToCal.Init(e.howToCal);
            howToCal.RegisterValueChangedCallback((evt) => { e.howToCal = (calValue)howToCal.value; });
            extensionContainer.Add(howToCal);
        }

        private void Turn()
        {
            turn = new EnumField("新的朝向2D");
            turn.Init(e.turn);
            turn.RegisterValueChangedCallback((evt) => { e.turn = (turn)turn.value; });
            extensionContainer.Add(turn);
        }

        private void FindType()
        {
            findType = new EnumField("查找类型");
            findType.Init(e.findType);
            findType.RegisterValueChangedCallback((evt) => { e.findType = (findType)findType.value; });
            extensionContainer.Add(findType);
        }

        private void QuestStatus()
        {
            questStatus = new EnumField("任务状态");
            questStatus.Init(e.questStatus);
            questStatus.RegisterValueChangedCallback((evt) => { e.questStatus = (questStatus)questStatus.value; });
            extensionContainer.Add(questStatus);
        }

        private void IndependentSwitch()
        {
            independentSwitch = new EnumField("编号");
            independentSwitch.Init(e.independentSwitch);
            independentSwitch.RegisterValueChangedCallback((evt) => { e.independentSwitch = (IndependentSwitch)independentSwitch.value; });
            extensionContainer.Add(independentSwitch);
        }

        private void Compare()
        {
            compare = new EnumField("比较类型");
            compare.Init(e.compare);
            compare.RegisterValueChangedCallback((evt) => { e.compare = (compare)compare.value; });
            extensionContainer.Add(compare);
        }

        private void ConditionType()
        {
            conditionType = new EnumField("条件类型");
            conditionType.Init(e.conditionType);
            conditionType.RegisterValueChangedCallback((evt) => { e.conditionType = (conditionType)conditionType.value; });
            extensionContainer.Add(conditionType);
        }

        private void Weather()
        {
            weather = new EnumField("天气");
            weather.Init(e.weather);
            weather.RegisterValueChangedCallback((evt) => { e.weather = (weather)weather.value; });
            extensionContainer.Add(weather);
        }
    }

#endif
}
