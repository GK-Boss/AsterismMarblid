using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private List<AudioData> se;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            EnemyBase[] allEnemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);

            foreach (var enemy in allEnemies)
            {
                enemy.ChangeHP(-999999999);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.instance.PlaySE(se);
        }
    }
}
