using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] protected string targetTag;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float power = 1;

    protected float lifeTime;
    protected GameObject target;
    private BuildingData data;

    public void Init(GameObject _target, BuildingData _data)
    {
        target = _target;
        data = _data;

        Invoke(nameof(Delete), data.currentRange / speed);

        if (target)
        {
            Vector3 vec = target.transform.position - transform.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
        }
    }

    private void FixedUpdate()
    {
        if (target && target.activeInHierarchy)
        {
            // ˆÚ“®
            Vector3 nextPos = speed * (target.transform.position - transform.position).normalized * Time.fixedDeltaTime;
            transform.position += nextPos;

            //‰ñ“]
            Vector3 vec = target.transform.position - transform.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
        }
        else
        {
            transform.position += speed * transform.right * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            collision.GetComponent<IChangeHP>().ChangeHP(-power);
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
