using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHp;
    private float currentHP;
    private AnimatorManager animator;

    public event Action unitDied;
    public event Action<float> unitHited;

    public float MaxHealth { get { return maxHp; } }

    public void Refresh()
    {
        currentHP = maxHp;
    }

    private void Awake()
    {
        animator = GetComponent<AnimatorManager>();
        currentHP = maxHp;
    }

    public void GetHited(float damage)
    {
        animator.SetAnim("GetHited", true, false, blockedTime: 0.5f);
        currentHP -= damage;
        unitHited?.Invoke(currentHP);
        if(currentHP <= 0f)
        {
            unitDied?.Invoke();
        }
    }
}
