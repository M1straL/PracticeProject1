using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Loader : MonoBehaviour
{
    [SerializeField] 
    private GameObject terrainPrefab;

    private List<GameObject> _terrainParts;
    private List<Building> _buildings;

    [SerializeField] 
    private BuildingConfig buildingConfig;

    [SerializeField] 
    private Camera _mainCamera;

    [SerializeField] 
    private Camera _flyingCamera;

    private float _timeToGenerate = 5f;
    private float _timer;
    
    private void Start()
    {
        _mainCamera.farClipPlane = 10000F;
        _mainCamera.transform.position = new Vector3(795, 1005, 608);
        _mainCamera.transform.Rotate(new Vector3(76, -90, 0));
        
        _terrainParts = new List<GameObject>(100);
        GenerateTerrain();
        _timer = _timeToGenerate;

        _buildings = new List<Building>(100);
        GenerateBuildings();

    }

    // Update is called once per frame
    private void Update()
    {
        if (_timer <= 0)
        {
           _timer = _timeToGenerate;
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }


    private void GenerateTerrain()
    {
        var terrainRoot = new GameObject();
        
        for (int i = 0; i < 1000; i += 100)
        {
            for (int j = 0; j < 1000; j+= 100)
            {
                var gameObj = Instantiate(terrainPrefab, new Vector3(i,0,j), Quaternion.identity, terrainRoot.transform);
                _terrainParts.Add(gameObj);
            }
        }   
    }

    private void GenerateBuildings()
    {
        foreach (var part in _terrainParts)
        {
            var partPos = part.transform.position;
            
            var allStructs = buildingConfig._upgradeStructs;
            var random = Random.Range(0, allStructs.Count);
            var prefab = allStructs[random].Prefab;
            
            var buildPosition = new Vector3(partPos.x + 50F, partPos.y + prefab.transform.position.y, partPos.z + 50F);
            
            CreateBuilding(prefab, buildPosition);
        }
    }

    private Building CreateBuilding(GameObject prefab, Vector3 buildPosition)
    {
        var buildingGameObj = Instantiate(prefab, buildPosition, Quaternion.Euler(0,90,0));
        
        var building = buildingGameObj.GetComponent<Building>();

        var progressBar = buildingGameObj.GetComponentInChildren<UpgradeBar>();
        if (progressBar != null)
        {
            progressBar.camera = _mainCamera;
        }

        building.SetBuildingConfig(buildingConfig); //TODO переписать
        building.UpgradeEvent += OnUpgradeEvent;

        _buildings.Add(building);

        return building;
    }

    private void OnUpgradeEvent(Building building, GameObject upgraded)
    {
        var oldGo = building.gameObject;
        oldGo.SetActive(false);

        var position = new Vector3(oldGo.transform.position.x, upgraded.transform.position.y,
            oldGo.transform.position.z);
        
        var newBuilding = CreateBuilding(upgraded, position);
        var animator = newBuilding.gameObject.GetComponent<Animator>();
        
        //animator.SetTrigger("trigger");

        Coroutine coroutine = StartCoroutine(UpdateProgressBar(newBuilding._upgradeBar));

        if (!_buildings.Remove(building))
        {
            Debug.Log($"There's no such gameobject {building.gameObject.name} {building.level} in buildings array");
        }
        building.UpgradeEvent -= OnUpgradeEvent;
        Destroy(oldGo);
    }

    private IEnumerator UpdateProgressBar(UpgradeBar bar)
    {
        for (int i = 0; i < 100; i++)
        {
            Debug.Log($"Coroutine: {i}");
            
            if (i == 55)
            {
                yield break;        
            }
        }
    }
    
    public void ChangeView()
    {
        _mainCamera.enabled = !_mainCamera.enabled;
        _flyingCamera.enabled = !_flyingCamera.enabled;
    }
}
