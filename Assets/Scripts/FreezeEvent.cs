using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEvent : MonoBehaviour {
    
    private Animation _anim;

    private void Start() {
        _anim = transform.parent.Find("RaisingPlatform").GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            LevelManager.Freeze();
            StartCoroutine(TriggerAnimation(3));
            // Topların düşmesini bekliyoruz
            StartCoroutine(WaitBeforeCheck(3));
            
        }
    }

    private IEnumerator WaitBeforeCheck(int waitTime) {
        yield return new WaitForSeconds(waitTime);
        LevelManager.CheckScore();
    }
    
    private IEnumerator TriggerAnimation(int waitTime) {
        yield return new WaitForSeconds(waitTime);
        _anim.Play();
    }
}
