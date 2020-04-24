using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public string Name;
    public GameObject Prefab;
    public int Amount;
    public float ActiveTime;
    public Transform Parent;
}