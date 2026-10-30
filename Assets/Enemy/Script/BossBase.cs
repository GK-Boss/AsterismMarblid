using UnityEngine;

public class BossBase : EnemyBase
{
    protected int maxPhase;
    protected int currentPhase;
    [SerializeField] protected Phase[] phases;
    [SerializeField] protected RewardBox rewardBox;

    public float hpRatio = 1.0f;
    public float strengthRatio = 1.0f;

    private Coroutine currentCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();
        int currentLiskLevel = EnemySpawnerManager.instance.GetRiskLevel();
        hp = Mathf.CeilToInt(currentLiskLevel * hpRatio);
        currentCoroutine = StartCoroutine(phases[currentPhase].PhaseCoroutine(this));
    }
    protected override void Dead()
    {
        if (isDead) return;

        if (currentPhase >= maxPhase)
        {
            EnemySpawnerManager.instance.AddRiskLevel(EnemySpawnerManager.instance.GetRiskLevel() / 2);
            RewardManager.instance.AddReward(rewardBox);
            base.Dead();
        }
        else
        {
            NextPhase();
        }
    }

    private void NextPhase()
    {
        phases[currentPhase].Stop();
        StopCoroutine(currentCoroutine);

        currentPhase++;
        currentCoroutine = StartCoroutine(phases[currentPhase].PhaseCoroutine(this));
    }
}
