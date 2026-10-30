using UnityEngine;

public class Hunter_2_Special_Bullet1 : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;

    [SerializeField] private Hunter_2_Special_Bullet2 bullet2;

    private Rigidbody2D rb;
    private float defaultAngle;
    private float angleOffset;
    private float offsetSign;
    private float lifeTime;

    private Vector3 targetPosition;
    private float currentAngle;

    private float timer;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 0.1f)
        {
            PoolManager.instance.Get<Hunter_2_Special_Bullet2>(bullet2.gameObject).Setting(transform.position, targetPosition, lifeTime * speed / 4);
            timer -= 0.1f;
        }

        currentAngle += 2 * -angleOffset / lifeTime * Time.fixedDeltaTime;
        Move(currentAngle);
    }
    private void Move(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.linearVelocity = speed * new Vector3(Mathf.Cos((angle - 45) * Mathf.Deg2Rad), Mathf.Sin((angle - 45) * Mathf.Deg2Rad));
    }
    public void Setting(Vector3 _targetPosition, Vector3 position, float _defaultAngle, float _angleOffset, float range)
    {
        transform.position = position;
        defaultAngle = _defaultAngle;
        angleOffset = _angleOffset;
        targetPosition = _targetPosition;
        currentAngle = defaultAngle + angleOffset;
        //offsetSign = Mathf.Sign(angleOffset);
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0, 0, defaultAngle + angleOffset);

        lifeTime = 2 * (_targetPosition - position).magnitude / speed;
        Invoke(nameof(Delete), lifeTime);
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
