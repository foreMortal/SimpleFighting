using UnityEngine;

public interface IInputManager
{
    public void GaverInput();
    public Vector3 GetMoveInput();
    public Vector3 GetAttackInput();
}
