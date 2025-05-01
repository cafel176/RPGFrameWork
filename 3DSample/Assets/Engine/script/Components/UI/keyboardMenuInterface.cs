using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Func();

public interface keyboardMenuInterface
{
    void up();
    void down();
    void left();
    void right();
    void doOption();
    void cancel();
    void changeSize();
}
