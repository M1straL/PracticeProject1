using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBar : MonoBehaviour
{
    public Camera camera;
    public GameObject _hpBar;
    private Gradient _gradient;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.LookAt(camera.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
