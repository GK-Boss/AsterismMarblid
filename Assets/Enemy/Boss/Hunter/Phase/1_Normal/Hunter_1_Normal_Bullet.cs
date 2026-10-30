using UnityEngine;

public class Hunter_1_Normal_Bullet : MonoBehaviour
{
    [SerializeField] private float damage;

    private Rigidbody2D rb;
    public void Setting(Vector3 targetPosition, float speed, float lifeTime)
    {
        Vector3 vec = targetPosition - transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = speed * vec.normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg + 45);
        Invoke(nameof(Delete), lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            collision.GetComponent<IChangeHP>().ChangeHP(-damage);
            Delete();
        }
    }

    private void Delete()
    {
        if (!gameObject.activeSelf) return;

        CancelInvoke();

        PoolManager.instance.Release(this);
    }
}
