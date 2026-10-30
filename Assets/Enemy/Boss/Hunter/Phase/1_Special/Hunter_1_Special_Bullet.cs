using TMPro;
using UnityEngine;

public class Hunter_1_Special_Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    public void Setting(Vector3 position, float angle, float range)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = speed * new Vector3(Mathf.Cos((angle - 45) * Mathf.Deg2Rad), Mathf.Sin((angle - 45) * Mathf.Deg2Rad));

        Invoke(nameof(Delete), range / speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeSelf) return;

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
