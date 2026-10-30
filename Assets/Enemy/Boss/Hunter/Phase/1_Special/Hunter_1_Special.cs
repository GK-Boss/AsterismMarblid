using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AttackPattern/Hunter/1_Special")]
public class Hunter_1_Special : Phase
{
    [SerializeField] private Hunter_1_Special_Bullet bullet;
    [SerializeField] private float range;

    public override IEnumerator PhaseCoroutine(BossBase boss)
    {
        Hunter_1_Special_Bullet generatedBullet = null;
        GameObject target = null;
        Transform targetTransform = null;
        float defaultAngle = 0;

        while (true)
        {
            boss.stopMove = false;
            yield return new WaitForSeconds(1f);
            target = (boss as Hunter).GetTarget(range);
            if (!target) continue;
            targetTransform = target.transform;
            Vector3 vec = (targetTransform.position - boss.transform.position).normalized;
            defaultAngle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg + 45;
            boss.stopMove = true;


            yield return new WaitForSeconds(1f);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    if (i != 0)
                    {
                        generatedBullet = PoolManager.instance.Get<Hunter_1_Special_Bullet>(bullet.gameObject);
                        generatedBullet.Setting((boss as Hunter).firePoint.position, defaultAngle + 5 * j, range);

                        generatedBullet = PoolManager.instance.Get<Hunter_1_Special_Bullet>(bullet.gameObject);
                        generatedBullet.Setting((boss as Hunter).firePoint.position, defaultAngle - 5 * j, range);
                    }
                    else
                    {
                        generatedBullet = PoolManager.instance.Get<Hunter_1_Special_Bullet>(bullet.gameObject);
                        generatedBullet.Setting((boss as Hunter).firePoint.position, defaultAngle, range);
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    public override void Stop()
    {

    }
}
