using UnityEngine;

public interface ActorMoveInterface
{
    void setStep(string clip);
    void setRunStep(string clip);
    void Move(Vector2 vec, bool run = false, bool _rush = false);
    void MoveTo(Vector2 pos, bool run = false);
    void Jump();
    void Rotating(float horizontal, float vertical, bool auto);
}
