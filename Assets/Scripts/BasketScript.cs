using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TextMeshProUGUI;

public class BasketScript : MonoBehaviour {
    
    public TextMeshProUGUI scoreText;
    public static int ThresholdScore = 5;
    public static int CurrentScore = 0;

    private static Animation _anim;
    
    public static int[] LevelNo = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private static readonly int[] Threshold = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 };
    
    void Start() {
        scoreText.text = "0/" + ThresholdScore;
    }
    
    public static void SetLevel(int level) {
        ThresholdScore = Threshold[LevelNo[level]];
        CurrentScore = 0;
    }
    
    void Update() {
        scoreText.text = CurrentScore + "/" + ThresholdScore;
    }
    
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Ball"))
        {
            CurrentScore++;
            Destroy(collision.gameObject, 3f);
        }
    }
}
