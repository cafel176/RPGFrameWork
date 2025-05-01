using UnityEngine;

public interface ActorInterface
{
    void Move(Vector2 vec, bool run = false, bool _rush = false);
    void changeTurn(turn turn);
    turn getTurn();
    void MoveTo(Vector2 pos, bool run = false);
    void setStep(string clip);
    void setRunStep(string clip);
    void changeFollowsPos(turn e, Vector2 t, string step = "", string runStep = "");
    void addFollow(GameObject g);
    void removeFollow(GameObject g);
}
