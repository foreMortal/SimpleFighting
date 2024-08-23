using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Transform player;
    private Attack attack;
    private Health health;
    private AnimatorManager animator;
    private EnemySpawner spawner;
    public float t;
    public int blockedState, movingType = -1, enemyType;
    private float dieIn;

    public void RefreshEnemy(Vector3 position)
    {
        gameObject.SetActive(true);
        GetComponent<Collider>().enabled = true;
        transform.position = position;
        animator.SetAnim("Run", false, true);
        movingType = -1;
        blockedState = 0;
        health.Refresh();
    }

    public void Setup(Transform player, int enemyType, EnemySpawner spawner)
    {
        this.player = player;
        this.enemyType = enemyType;
        this.spawner = spawner;
        health = GetComponent<Health>();
        attack = GetComponent<Attack>();
        animator = GetComponent<AnimatorManager>();

        health.unitDied += EnemyDied;
        animator.AnimationBlockedEvent += BlockState;
    }

    private void Update()
    {
        if(dieIn > 0f)// скрыть противника после анимации смерти
        {
            dieIn -= Time.deltaTime;
            if (dieIn <= 0f)
                gameObject.SetActive(false);
        }

        transform.LookAt(player.position);
        t = (player.position - transform.position).sqrMagnitude;
        if(t <= 0.7 * 0.7f && blockedState == 0)
        {
            attack.MakeAttack();
            if (movingType != 0)
                movingType = 0;
        }
        else if(blockedState == 0)
        {
            if(movingType != 1)
            {
                movingType = 1;
                animator.SetAnim("Run", false, true);
            }
            transform.position = Vector3.MoveTowards(transform.position, player.position, 3f * Time.deltaTime);
        }
    }

    private void EnemyDied()
    {
        spawner.EnemyDied(this, enemyType);
        animator.SetAnim("Death", true, false, blockedTime: 2f);
        GetComponent<Collider>().enabled = false;
        dieIn = 1f;
    }

    private void BlockState(int type)
    {
        blockedState = type;
    }
}
