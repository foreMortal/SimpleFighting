using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchPadInput : MonoBehaviour, IInputManager
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform moveCenter;
    private Vector3 moveVector, attackPosition, startPosition;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        startPosition = moveCenter.localPosition;
    }

    public void GaverInput()
    {
        moveVector = Vector3.zero;
        attackPosition = Vector3.zero;
        //CheckPosition(Input.mousePosition);

        foreach (var touch in Touch.activeTouches)
        {
            CheckPosition(touch.screenPosition);
        }
    }

    public Vector3 GetMoveInput()
    {
        return moveVector;
    }
    public Vector3 GetAttackInput()
    {
        return attackPosition;
    }

    private void CheckPosition(Vector3 pos)
    {
        Ray ray = cam.ScreenPointToRay(pos);
        if(Physics.Raycast(ray, out RaycastHit info, 1f, 1 << 5))
        {
            if (info.collider.CompareTag("Move"))
            {
                pos = info.collider.transform.position - info.point;
                moveCenter.localPosition = startPosition + new Vector3(pos.x, -pos.y, 0f).normalized * 160f;
                Vector3 movePos = -cam.transform.forward * pos.y + cam.transform.right * pos.x;
                moveVector = movePos.normalized;
                moveVector.y = 0;
            }
            if (info.collider.CompareTag("Attack"))
            {
                attackPosition = pos;
            }
        }
    }
}
