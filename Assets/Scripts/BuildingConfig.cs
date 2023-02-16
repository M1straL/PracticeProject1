using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingConfig", menuName = "ScriptableObjects/BuildingConfig", order = 1)]
public class BuildingConfig : ScriptableObject
{
    [SerializeField] 
    public List<UpgradeStruct> _upgradeStructs;
    
    [Serializable]
    public class UpgradeStruct
    {
        public int level;
        public GameObject Prefab;
    }
}