using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance { get; private set; }

    // プレハブのInstanceID（int）をキーにして、オブジェクトプールを管理
    private Dictionary<int, ObjectPool<GameObject>> _poolDictionary = new Dictionary<int, ObjectPool<GameObject>>();

    // 取り出されたアクティブなオブジェクトが「どのプール出身か」を記録する辞書（返却時に使用）
    private Dictionary<int, int> _instanceToPrefabMap = new Dictionary<int, int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 【型指定版】プレハブを指定して、プールから特定のコンポーネントを取得します。
    /// </summary>
    public T Get<T>(GameObject prefab, Transform parent = null) where T : Component
    {
        // 下記の通常のGetメソッドを呼び出してGameObjectを取得
        GameObject obj = Get(prefab, parent);

        if (obj == null) return null;

        // 指定されたコンポーネントを取得して返す
        if (obj.TryGetComponent<T>(out var component))
        {
            return component;
        }

        // 万が一プレハブに指定したコンポーネントがついていなかった場合のセーフティ
        Debug.LogError($"SmartPoolManager: プレハブ {prefab.name} に要求されたコンポーネント {typeof(T).Name} が見つかりません。");
        return null;
    }

    /// <summary>
    /// 【通常版】プレハブを指定して、プールからGameObjectを取得します（従来通り）
    /// </summary>
    public GameObject Get(GameObject prefab, Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogError("SmartPoolManager: 渡されたプレハブがヌルです。");
            return null;
        }

        int prefabId = prefab.GetInstanceID();

        if (!_poolDictionary.ContainsKey(prefabId))
        {
            CreateNewPool(prefab, prefabId);
        }

        GameObject obj = _poolDictionary[prefabId].Get();

        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }

        return obj;
    }

    /// <summary>
    /// オブジェクト（GameObjectまたはComponent）をプールに戻します
    /// </summary>
    public void Release(GameObject instance)
    {
        if (instance == null) return;

        int instanceId = instance.GetInstanceID();

        if (_instanceToPrefabMap.TryGetValue(instanceId, out int prefabId))
        {
            if (_poolDictionary.TryGetValue(prefabId, out var pool))
            {
                pool.Release(instance);
                return;
            }
        }

        Debug.LogWarning($"SmartPoolManager: プール外で生成されたオブジェクト {instance.name} のため通常のDestroyを実行します。");
        Destroy(instance);
    }

    /// <summary>
    /// オーバーロード：コンポーネントをそのまま渡してプールに戻せるようにする
    /// </summary>
    public void Release(Component component)
    {
        if (component != null)
        {
            Release(component.gameObject);
        }
    }

    private void CreateNewPool(GameObject prefab, int prefabId)
    {
        Transform poolRoot = new GameObject($"Pool_{prefab.name}").transform;
        poolRoot.SetParent(this.transform);

        var newPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab, poolRoot);
                _instanceToPrefabMap[obj.GetInstanceID()] = prefabId;
                return obj;
            },
            actionOnGet: target => target.SetActive(true),
            actionOnRelease: target =>
            {
                target.SetActive(false);
                target.transform.SetParent(poolRoot);
            },
            actionOnDestroy: target =>
            {
                if (target != null)
                {
                    _instanceToPrefabMap.Remove(target.GetInstanceID());
                    Destroy(target);
                }
            },
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100
        );

        _poolDictionary.Add(prefabId, newPool);
    }
}