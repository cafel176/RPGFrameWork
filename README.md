# RPGFrameWork
因兴趣而自己研究的一套基于unity的RPG解密游戏框架，目标是为无计算机基础的开发者朋友省去代码工作。<br/>
整体结构仿照RPGMaker而设计，用户通过使用写好的功能，在unity编辑器内自行设计相关的数据和功能文件而完成开发。<br/>
相关文件采用scriptobject，用户可在资源窗口内右键新建对应文件，之后使用设计好的编辑器界面来完成操作<br>
<span id="jump">主要功能：</span><br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#jump1">1.流程控制</a><br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#jump2">2.全局变量</a><br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#jump3">3.资源管理</a><br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#jump4">4.数据结构体</a><br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#jump5">5.多语言</a><br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#jump6">6.UI系统</a><br>
其余主要功能如存档，道具和任务系统，角色控制，场景可交互事件等等由Prefab提供。<br>

<br/>
<span id="jump1">1.流程控制： <a href="#jump">返回</a></span><br>
用户可为游戏添加用于控制流程的事件列表，事件列表支持以下用途。<br/>
a. 剧情，场景内主线和支线流程的控制，与全局变量结合使用。<br/>
b. 调查事件，调查场景内某些物品出现的重复事件，一次性事件等等。<br/>
c. 公共事件，可在任何地方触发的一组全局事件，可用于道具效果的设计等。<br/>
内置的事件目前未全部完成。<br/>

![图](pic/13.png)

![图](pic/14.png)

![图](pic/1.png)

<br/>
<span id="jump2">2.全局变量： <a href="#jump">返回</a></span><br/>
用户可自行添加对应的全局变量，包括整数浮点数和布尔型，之后可在流程文件中使用这些变量来进行解密流程的设计。<br/>
游戏从头开始时系统会读取这些变量用来初始化游戏，这些变量不会被系统修改<br/>

![图](pic/8.png)


![图](pic/9.png)

<br/>
<span id="jump3">3.资源管理： <a href="#jump">返回</a></span><br/>
用户将资源拖入unity编辑器，在资源管理文件内设置资源和名称，在其他地方通过名称调用对应资源。<br/>
因此类解密游戏流程复杂，资源重复使用相当频繁，采用这种管理方式为避免因替换某资源而导致大规模的修改原有流程文件。<br/>

![图](pic/6.png)


![图](pic/7.png)

<br/>
<span id="jump4">4.数据结构体： <a href="#jump">返回</a></span><br/>
目前内置的常用数据结构体包括道具和任务两种。添加后可以直接在系统中使用。<br/>
以后也在考虑支持用户自己定义结构数据体。<br/>

![图](pic/3.png)

![图](pic/11.png)

![图](pic/4.png)

![图](pic/12.png)

<br/>
<span id="jump5">5.多语言： <a href="#jump">返回</a></span><br/>
用户可为游戏添加多语言支持，在系统内可以随时切换。<br/>
如果认为在unity编辑器内编辑文本有所不便，也可以直接用文本编辑器编辑scriptobject文件来添加文本。<br/>

![图](pic/5.png)


![图](pic/10.png)



<br/>
<span id="jump6">6.UI系统： <a href="#jump">返回</a></span><br/>
采用仅支持键盘的多层级窗口UI，在父窗口选择分类后可进入子窗口。<br/>

![图](pic/2.png)
