using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public interface funcable
    {
        // 被父节点操作，i是类型参数
        void doFunc(string param, GameObject parent);
        // 被子节点操作
        void doFunc();
    }
}
