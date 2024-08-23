using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Vector3 checkRadius, offset, attackRadius;
    [SerializeField] private LayerMask mask;
    [SerializeField] private bool active;
    [SerializeField] private float damage;
    private IInputManager inputManager;
    private int blockedType;
    private float rotDelta;
    private bool rotate;
    private Quaternion targetRotation;
    private AnimatorManager animator;
    private Collider[] colliders = new Collider[10];
    private Collider closestEnemy;

    private void Awake()
    {
        animator = GetComponent<AnimatorManager>();
        TryGetComponent<IInputManager>(out inputManager);
        animator.AnimationBlockedEvent += BlockedType;
    }

    public void SelfUpdate()// вызывается из playerManager
    {
        if (inputManager.GetAttackInput() != Vector3.zero && blockedType == 0)
        {
            ScanForAttack();
        }
        if (rotate)
        {
            rotDelta += Time.deltaTime * 3f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotDelta);
            if (rotDelta >= 1f)
                rotate = false;
        }
    }

    private void ScanForAttack()
    {
        int t = Physics.OverlapCapsuleNonAlloc(transform.position, transform.position + new Vector3(0f, 1.8f, 0f), 0.68f, colliders,  mask);
        if (t > 0)
        {
            closestEnemy = colliders[0];
            Vector3 first = transform.forward;
            Vector3 second = closestEnemy.transform.forward;
            first.y = 0f;
            second.y = 0f;
            float closestDot = Vector3.Dot(first, second);

            foreach (var col in colliders)
            {
                if (col != null)
                {
                    Vector3 second1 = col.transform.forward;
                    second.y = 0f;
                    float enemyDot = Vector3.Dot(first, second1);
                    if (enemyDot < closestDot)
                    {
                        closestEnemy = col;
                        closestDot = enemyDot;
                    }
                }
                else
                    break;
            }
            Vector3 enemyRot = new Vector3(closestEnemy.transform.position.x, transform.position.y, closestEnemy.transform.position.z) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Rotate(Quaternion.LookRotation(enemyRot, Vector3.up));
        }

        MakeAttack();
    }

    private void Rotate(Quaternion rotation)
    {
        targetRotation = rotation;
        rotate = true;
        rotDelta = 0f;
    }

    public void MakeAttack()
    {
        if(blockedType == 0)
        {
            animator.SetAnim(PickAnim(), true, true);

            if (Physics.OverlapBoxNonAlloc(transform.position + (transform.forward * 0.44f) + transform.up, attackRadius, colliders, transform.rotation, mask) > 0)
            {
                foreach(var enemy in colliders)
                {
                    if (enemy != null)
                        enemy.GetComponent<Health>().GetHited(damage);
                    else
                        break;
                }
            }
        }
    }

    public void BlockedType(int type)
    {
        blockedType = type;
    }

    private string PickAnim()
    {
        int t = Random.Range(0, 5);
        return t switch
        {
            0 => "bk_rh_right_B",
            1 => "bk_knee_left_A",
            2 => "bk_push_right_A",
            3 => "bk_rh_left_A",
            4 => "bk_rh_left_B",
            _ => "bk_rh_left_B",
        };
    }

    private void OnDrawGizmos()
    {
        
    }
}
