using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private PoolConfig _poolConfig;

    private static Pool _instance;

    private static IEnumerable<GameObject> Items => _instance._poolConfig.Prefabs;

    private static readonly List<MonoBehaviour> PooledObjects = new();

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public static T Get<T>(Transform parent = null) where T : MonoBehaviour
    {
        var obj = PooledObjects.Find(o => o.GetType() == typeof(T)) as T;
        if (obj == null)
        {
            foreach (var item in Items)
            {
                if (item.TryGetComponent(out T value))
                {
                    obj = Instantiate(value, parent);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
        }
        else
        {
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            PooledObjects.Remove(obj);
        }

        if (obj == null)
            Debug.LogWarning($"Object of type {typeof(T)} doesn't exist in pool");
        
        return obj;
    }
    
    public static T GetPun<T>(Transform parent = null) where T : MonoBehaviourPun
    {
        var obj = PooledObjects.Find(o => o.GetType() == typeof(T)) as T;
        if (obj == null)
        {
            foreach (var item in Items)
            {
                if (item.TryGetComponent(out T value))
                {
                    obj = PhotonNetwork
                        .Instantiate(value.name, value.transform.position, value.transform.rotation)
                        .GetComponent<T>();
                    obj.transform.SetParent(parent);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
        }
        else
        {
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            PooledObjects.Remove(obj);
        }

        if (obj == null)
            Debug.LogWarning($"Object of type {typeof(T)} doesn't exist in pool");
        
        return obj;
    }

    public static void Release<T>(T obj) where T : MonoBehaviour
    {
        if (obj is IReleasable releasable)
            releasable.Dispose();
        
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(null);
        obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        PooledObjects.Add(obj);
    }
}