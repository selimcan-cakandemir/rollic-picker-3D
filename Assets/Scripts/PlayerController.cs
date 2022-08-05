using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float forwardSpeed;
    public float lateralSpeed;
    public float offsetWall = 1.5f;
    public LayerMask layerMask;
    private Vector3 _movement;
    private Transform _transform;
    private readonly int[] _direction = new int[] { 1, -1 };

    private Ray _ray;
    private RaycastHit _hitData;
    private Touch _touch;
    private float _touchSpeed;

    void Start() {
        _transform = this.transform;
        _touchSpeed = 0.01f;
    }
    
    void Update() {
        _movement = new Vector3(Input.GetAxisRaw("Horizontal") * lateralSpeed, 0, forwardSpeed);
        transform.Translate(_movement * Time.deltaTime);

        for (int i = 0; i < 2; i++)
        {
            _ray = new Ray(_transform.position, new Vector3(_direction[i],0,0 ));
            if(Physics.Raycast(_ray, out _hitData,10f, layerMask));
            {
                if (_hitData.distance < offsetWall)
                {
                    _transform.position = _hitData.point - new Vector3(_direction[i] * offsetWall, 0f, 0f);
                }
            }
        }

        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(transform.position.x + _touch.deltaPosition.x * _touchSpeed,
                    transform.position.y, transform.position.z);
            }
        }
        
    }

}
