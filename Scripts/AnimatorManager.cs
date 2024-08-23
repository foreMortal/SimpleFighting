using System;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{//класс для управления анимациями, некоторые анмации должны ограничивать игрока в возможностях во время их действия
 //другие - нет, легче контролировать всё  из одного места
    private Animator animator;
    private int blockedState;
    private float timer;

    public event Action<int> AnimationBlockedEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (blockedState == 1)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                BlockedType(0);
            }
        }
    }

    public void SetAnim(string name, bool setBlockState=false, bool canBeBlocked=false, float time=0.05f, float blockedTime = 0.5f)
    {
        if(!canBeBlocked || canBeBlocked && blockedState == 0)
        {
            animator.CrossFade(name, time);

            if (setBlockState)
            {
                BlockedType(1);
                timer = blockedTime;
            }
        }
    }

    private void BlockedType(int type)
    {
        blockedState = type;
        AnimationBlockedEvent?.Invoke(type);
    }
}
