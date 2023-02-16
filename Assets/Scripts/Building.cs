using System;
using System.Linq;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    public int level = 0;
    
    [SerializeField]
    private bool isUpgradable; 
    private bool _canBeUpgraded;
    
    public Action<Building, GameObject> UpgradeEvent;
    
    private BuildingConfig _buildingConfig { get; set; }

    internal UpgradeBar _upgradeBar { get; set;} 

    public void SetBuildingConfig(BuildingConfig config)
    {
        _buildingConfig = config;
    }
    

    private void Awake()
    {
        _upgradeBar = this.GetComponent<UpgradeBar>();
    }

    private void OnMouseDown()
    {
        if (IsCanBeUpgraded(out var nextBuildingStruct))
        {
            UpgradeEvent.Invoke(this, nextBuildingStruct.Prefab);
        }
        else
        {
            Debug.LogError("We cant upgrade this building");
        } 
    }
    
    private bool IsCanBeUpgraded(out BuildingConfig.UpgradeStruct nextBuildingStruct)
    {
        if (!isUpgradable)
        {
            nextBuildingStruct = null;
            return false;
        }
        
        var newBuilding = _buildingConfig._upgradeStructs.FirstOrDefault(str => (str.level - level == 1));
        
        nextBuildingStruct = newBuilding;
        
        return newBuilding != null;
    }
}