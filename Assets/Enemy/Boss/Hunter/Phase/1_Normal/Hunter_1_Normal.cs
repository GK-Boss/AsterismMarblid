using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AttackPattern/Hunter/1_Normal")]
public class Hunter_1_Normal : Phase
{
    [SerializeField] private Hunter_1_Normal_Bullet bullet;
    [SerializeField] private float range;
    [SerializeField] private float maxSpeed;
    public override IEnumerator PhaseCoroutine(BossBase boss)
    {
        Hunter_1_Normal_Bullet generatedBullet = null;
        GameObject target = null;
        while (true)
        {
            boss.stopMove = false;
            yield return new WaitForSeconds(1.1f);

            target = (boss as Hunter).GetTarget(range);
            boss.stopMove = true;
            if (!target) continue;
            generatedBullet = PoolManager.instance.Get<Hunter_1_Normal_Bullet>(bullet.gameObject);
            generatedBullet.transform.position = (boss as Hunter).firePoint.position;
            generatedBullet.Setting(target.transform.position, maxSpeed / 1.0f, range / maxSpeed);
            yield return new WaitForSeconds(0.3f);
            if (!target) continue;
            generatedBullet = PoolManager.instance.Get<Hunter_1_Normal_Bullet>(bullet.gameObject);
            generatedBullet.transform.position = (boss as Hunter).firePoint.position;
            generatedBullet.Setting(target.transform.position, maxSpeed / 0.9f, range / maxSpeed);
            yield return new WaitForSeconds(0.3f);
            if (!target) continue;
            generatedBullet = PoolManager.instance.Get<Hunter_1_Normal_Bullet>(bullet.gameObject);
            generatedBullet.transform.position = (boss as Hunter).firePoint.position;
            generatedBullet.Setting(target.transform.position, maxSpeed / 0.8f, range / maxSpeed);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public override void Stop()
    {

    }
}