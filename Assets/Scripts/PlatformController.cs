using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    
    [HideInInspector]
    public float forwardSpeed = -5;
    
    private Vector3 _movement;
    
    void Start() {
        Destroy(this.gameObject, 20);
    }
    
    void Update() {
        _movement = new Vector3(0, 0, forwardSpeed);
        transform.Translate(_movement * Time.deltaTime);
    }
}
