using System;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float speed;

    private IInputManager inputManager;
    private CharacterController controller;
    private AnimatorManager animator;
    private int blockedType;
    private float rotDelta, movingValue, animBuffer;
    private bool rotate;
    private Vector3 move;
    private Quaternion targetRotation;
   
    private void Awake()
    {
        animator = GetComponent<AnimatorManager>();
        inputManager = GetComponent<IInputManager>();
        controller = GetComponent<CharacterController>();
        animator.AnimationBlockedEvent += BlockedType;
    }

    public void SelfUpdate()// этот метод вызывается из playerManager
    {//эта абстракция не совсем здесь нужна но во первых так нужно по правилам solid
     //что не так важно. а во вторых это позволяет сделать управление с клавиатуры что облегчает тесты
        Vector3 newDir = inputManager.GetMoveInput();
        if (newDir != Vector3.zero && blockedType == 0)
        {
            if(movingValue != 1)
            {
                movingValue = 1;
                animator.SetAnim("Run", false, true, 0.2f);
            }
            if (rotate)
            {
                rotDelta += Time.deltaTime * 5f;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotDelta);
                if (rotDelta >= 1f)
                    rotate = false;
            }
            if (newDir != move)
            {
                Rotate(Quaternion.LookRotation(newDir, Vector3.up));
                move = newDir;
            }
            controller.Move((speed * Time.deltaTime * move) + new Vector3(0f, -2f, 0f));
        }
        else if(movingValue != -1 && blockedType == 0)
        {
            movingValue = -1;
            animator.SetAnim("Idle", false, true, 0.2f);
        }
        if (animBuffer > 0f)
        {
            animBuffer -= Time.deltaTime;
            if (animBuffer <= 0f)
                movingValue = 0;
        }
    }

    public void BlockedType(int type)
    {
        movingValue = -1;
        blockedType = type;
        if (type == 0)
            animBuffer = 1f;
    }

    private void Rotate(Quaternion rotation)
    {
        targetRotation = rotation;
        rotate = true;
        rotDelta = 0f;
    }
}
