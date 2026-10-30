using System.Collections;
using UnityEngine;

public abstract class Phase : ScriptableObject
{
    public abstract IEnumerator PhaseCoroutine(BossBase boss);
    public abstract void Stop();
}
