using UnityEngine;

public interface ActorInterface
{
    //controlMode getMode();
    ActorMoveInterface getMove();
    ActorBattleInterface getBattle();
    void CameraRotate(float _mouseX, float _mouseY);
    void CameraScale(float scale);
}
