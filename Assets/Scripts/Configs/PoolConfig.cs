using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "Configs/PoolConfig")]
public class PoolConfig : ScriptableObject
{
    public List<GameObject> Prefabs = new List<GameObject>();
}