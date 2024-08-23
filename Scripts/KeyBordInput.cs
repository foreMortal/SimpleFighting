using UnityEngine;

public class KeyBordInput : MonoBehaviour, IInputManager
{
    [SerializeField] private Camera _camera;
    private Vector3 move;
    public void GaverInput()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            move += _camera.transform.forward;
        if(Input.GetKey(KeyCode.S))
            move += -_camera.transform.forward;
        if (Input.GetKey(KeyCode.A))
            move += -_camera.transform.right;
        if (Input.GetKey(KeyCode.D))
            move += _camera.transform.right;

        move.y = 0;
        this.move = move.normalized;
    }

    public Vector3 GetMoveInput()
    {
        return move;
    }
    public Vector3 GetAttackInput()
    {
        return Vector3.zero;
    }
}
