using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Side
{
    public string Name;
    public Transform SpawnPointOrganizer;
    public SpawnPoint[] SpawnPoints;

    public void OnStart(Vector3 center)
    {
        foreach (SpawnPoint s in SpawnPoints)
        {
            s.transform.rotation = Quaternion.FromToRotation(s.transform.forward, center - s.transform.position);
        }
    }
}

public class SpawnPointManager : MonoBehaviour
{
    public List<Side> m_Sides;
    public Dictionary<string, Side> SideDictionary = new Dictionary<string, Side>();


    #region SINGLETON
    private static SpawnPointManager m_Instance;
    public static SpawnPointManager Instance { get { return m_Instance; } }


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


    void Start()
    {
        foreach (Side s in m_Sides)
        {
            s.OnStart(transform.position);
            SideDictionary.Add(s.Name, s);
        }
    }


    public Transform GetRandomSpawnPointTransform(string side)
    {
        Side s = SideDictionary[side];
        int size = s.SpawnPoints.Length;
        int i = Random.Range(0, size);
        Transform t = s.SpawnPoints[i].transform;
        return t;
    }


    private void OnValidate()
    {
        foreach (Side side in m_Sides)
        {
            if (side.SpawnPointOrganizer)
            {
                side.Name = side.SpawnPointOrganizer.name;
                side.SpawnPoints = side.SpawnPointOrganizer.GetComponentsInChildren<SpawnPoint>();
            }
        }
    }
}
