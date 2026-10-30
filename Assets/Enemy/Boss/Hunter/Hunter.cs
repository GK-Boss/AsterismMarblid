using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Hunter : BossBase
{
    public Transform firePoint;

    [SerializeField] private GameObject bow;
    private GameObject target;


    protected override void Update()
    {
        base.Update();
        if (target)
        {
            Vector3 vec = target.transform.position - transform.position;
            bow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg + 45);
        }
        else
        {
            bow.transform.rotation = Quaternion.identity;
        }
    }

    public GameObject GetTarget(float range)
    {
        float minDistance = Mathf.Infinity;
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Building"));

        for (int i = 0; i < results.Length; i++)
        {
            float distance = (results[i].transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                target = results[i].gameObject;
                minDistance = distance;
            }
        }
        return target;
    }
}
