using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    public List<ObjectPool> m_ObjectPools = new List<ObjectPool>();
    private float activeTime;


    #region SINGLETON
    private static ObjectPooler m_Instance;
    public static ObjectPooler Instance { get { return m_Instance; } }


    private void Awake()
    {
        if (m_Instance && m_Instance != this)
        {
            Destroy(gameObject);
        }

        m_Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion


    private void Start()
    {
        PopulateObjectPools();
    }


    private void PopulateObjectPools()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (ObjectPool pool in m_ObjectPools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Amount; i++)
            {
                GameObject go = Instantiate(pool.Prefab, pool.Parent);
                go.SetActive(false);
                objectPool.Enqueue(go);
            }

            PoolDictionary.Add(pool.Name, objectPool);
        }
    }


    public GameObject GetGameObject(string poolName)
    {
        return PoolDictionary[poolName].Dequeue();
    }


    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        GameObject go = PoolDictionary[poolName].Dequeue();

        if (!go)
        {
            Debug.LogWarning("Pool with name \"" + poolName + "\" does not exist.");
            return null;
        }

        go.SetActive(true);
        go.transform.position = position;
        go.transform.rotation = rotation;

        PoolDictionary[poolName].Enqueue(go);

        activeTime = GetPool(poolName).ActiveTime;
        if (activeTime != 0)
            StartCoroutine(SetInactive(go, activeTime));

        return go;
    }


    private ObjectPool GetPool(string poolName)
    {
        foreach (ObjectPool pool in m_ObjectPools)
        {
            if (poolName == pool.Name)
            {
                return pool;
            }
        }

        return null;
    }


    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation, float activeTime)
    {
        GameObject go = PoolDictionary[poolName].Dequeue();

        if (!go)
        {
            Debug.LogWarning("Pool with name \"" + poolName + "\" does not exist.");
            return null;
        }

        go.SetActive(true);
        go.transform.position = position;
        go.transform.rotation = rotation;

        PoolDictionary[poolName].Enqueue(go);

        StartCoroutine(SetInactive(go, activeTime));

        return go;
    }


    private IEnumerator SetInactive(GameObject spawnedObject, float timer)
    {
        yield return new WaitForSeconds(timer);
        spawnedObject.SetActive(false);

        if (spawnedObject.transform.parent)
        {
            spawnedObject.transform.position = spawnedObject.transform.parent.position;
            spawnedObject.transform.rotation = spawnedObject.transform.parent.rotation;
        }
    }


    private void UpdatePooledObjectInEditor()
    {
        for (int i = 0; i < m_ObjectPools.Count; i++)
        {
            if (m_ObjectPools[i].Prefab)
                m_ObjectPools[i].Name = m_ObjectPools[i].Prefab.name;
            else
                m_ObjectPools[i].Name = "";

            if (m_ObjectPools[i].Amount < 0)
                m_ObjectPools[i].Amount = 0;
        }
    }


    private void OnValidate()
    {
        UpdatePooledObjectInEditor();
    }
}