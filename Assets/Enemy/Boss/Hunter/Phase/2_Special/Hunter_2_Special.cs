using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AttackPattern/Hunter/2_Special")]
public class Hunter_2_Special : Phase
{
    [SerializeField] private Hunter_2_Special_Bullet1 bullet1;
    [SerializeField] private float range;

    private Hunter_2_Special_Bullet1 generatedBullet1_1;
    private Hunter_2_Special_Bullet1 generatedBullet1_2;

    public override IEnumerator PhaseCoroutine(BossBase boss)
    {
        GameObject target = null;
        Vector3 targetPosition = Vector3.zero;
        float defaultAngle = 0;

        Hunter_2_Special_Bullet1 generatedBullet1_1;
        Hunter_2_Special_Bullet1 generatedBullet1_2;

        while (true)
        {
            boss.stopMove = false;
            yield return new WaitForSeconds(1f);
            target = (boss as Hunter).GetTarget(range);
            if (!target) continue;
            targetPosition = target.transform.position;
            Vector3 vec = (targetPosition - (boss as Hunter).firePoint.position).normalized;
            defaultAngle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg + 45;
            boss.stopMove = true;

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < 15; i++)
            {
                generatedBullet1_1 = PoolManager.instance.Get<Hunter_2_Special_Bullet1>(bullet1.gameObject);
                generatedBullet1_2 = PoolManager.instance.Get<Hunter_2_Special_Bullet1>(bullet1.gameObject);

                generatedBullet1_1.Setting(targetPosition, (boss as Hunter).firePoint.position, defaultAngle, +45, range);
                generatedBullet1_2.Setting(targetPosition, (boss as Hunter).firePoint.position, defaultAngle, -45, range);

                yield return new WaitForSeconds(0.1f);
            }
        }

    }

    public override void Stop()
    {

    }
}
