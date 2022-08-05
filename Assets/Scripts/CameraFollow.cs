using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private GameObject _player;
    public float followOffset;
    
    void Start() {
        _player = GameObject.Find("Player");
    }
    
    void Update() {
        //Transform.position'u daha iyi performans için önce değişkene koyuyoruz, native kod'a çevirilmesi için gereken maliyeti azaltmak için 
        var cameraTransform = transform;
        var position = cameraTransform.position;
        position = new Vector3(position.x, position.y, _player.transform.position.z + followOffset);
        cameraTransform.position = position;
    }
}
