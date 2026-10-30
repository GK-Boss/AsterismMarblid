using UnityEngine;

public class EnemyBase : MonoBehaviour, IChangeHP
{
    [SerializeField] protected float defaultMaxHp;
    [SerializeField] protected float moveSpeed = 3f;
    public int cost;

    protected float hp;
    protected Vector3Int posInt;
    protected Vector3 moveTarget;
    protected bool isDead;

    [HideInInspector] public bool stopMove;
    protected float deadlockTime;

    protected virtual void OnEnable()
    {
        hp = defaultMaxHp;
        posInt = Vector3Int.RoundToInt(transform.position);
        moveTarget = posInt;
        isDead = false;
    }

    private void FixedUpdate()
    {
        if (!stopMove)
            Move();
    }
    protected virtual void Update()
    {
        posInt = Vector3Int.RoundToInt(transform.position);
    }
    protected void Move()
    {
        float distance = Vector3.Distance(transform.position, moveTarget);

        if (distance < 0.1f || deadlockTime >= 2.0f / moveSpeed)
        {
            moveTarget = EnemyNavigation.instance.GetNext(posInt);
            deadlockTime = 0;
        }
        else
            deadlockTime += Time.fixedDeltaTime;

        Vector3 direction = (moveTarget - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.fixedDeltaTime;

        // 4. 進行方向を向く (スプライトの向きを変える場合など)
        if (direction.magnitude > 0.1f)
        {
            // 必要に応じて回転やフリップの処理
        }
    }

    public void ChangeHP(float value)
    {
        if (isDead) return;

        hp += value;

        if (hp <= 0)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {
        EnemySpawnerManager.instance.AddRiskLevel(cost);
        PoolManager.instance.Release(gameObject);
        isDead = true;
    }
}