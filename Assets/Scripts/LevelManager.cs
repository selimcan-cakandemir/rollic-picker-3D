using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;
    public static int CurrentLevel = 0;
    private static TextMeshProUGUI _levelText;
    private static Transform _savedPrefabs;
    
    private static GameObject[] _levelList;
    
    void Awake(){ 
        _instance = this; 
    }
    
    private GameObject _titleCard;
    private static Transform _platformsParent;
    private static float _savedLateralSpeed;

    void Start() {
        PauseGame();
        _titleCard = GameObject.Find("Canvas_UI").transform.Find("TitleCard").gameObject;
        _platformsParent = GameObject.Find("Platforms").transform;
        _savedLateralSpeed = GameObject.Find("Player").gameObject.GetComponent<PlayerController>().lateralSpeed;
        _levelText = GameObject.Find("Canvas_UI").transform.Find("Level").gameObject.GetComponent<TextMeshProUGUI>();
        _savedPrefabs = GameObject.Find("SavedPrefabs").transform;
    }

   
    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            ResumeGame();
            CloseTitle();
        }
    }

    void PauseGame() {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    void CloseTitle() {
        _titleCard.SetActive(false);
    }

    public static void Freeze() {
        GameObject.Find("Player").gameObject.GetComponent<PlayerController>().lateralSpeed = 0;
        foreach (Transform platform in _platformsParent) {
            platform.gameObject.GetComponent<PlatformController>().forwardSpeed = 0;
        }
    }

    public static void Thaw() {
        GameObject.Find("Player").gameObject.GetComponent<PlayerController>().lateralSpeed = _savedLateralSpeed;
        foreach (Transform platform in _platformsParent) {
            platform.gameObject.GetComponent<PlatformController>().forwardSpeed = -5;
        }
    }

    public static void ResetLevel() {
        CurrentLevel = 0;
        BasketScript.SetLevel(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void CheckScore() {
        if (BasketScript.CurrentScore >= BasketScript.ThresholdScore)
        {
            //Bir sonraki level geliyor
            _instance.StartCoroutine(SetNextLevel(1));
            
            // Platform kalkıyor
            _instance.StartCoroutine(TriggerAnimation(2));

            // Ardından oyun devam ediyor
            _instance.StartCoroutine(ThawGame(3));
        }
        else
        {
            LevelManager.ResetLevel();
        }
    }
    
    private static IEnumerator ThawGame(int waitTime) {
        yield return new WaitForSeconds(waitTime);
        LevelManager.Thaw();
    }

    static IEnumerator TriggerAnimation(int waitTime) {
        yield return new WaitForSeconds(waitTime);
    }

    static IEnumerator SetNextLevel(int waitTime) {
        yield return new WaitForSeconds(waitTime);
        _levelList = Resources.LoadAll("Prefabs").Cast<GameObject>().ToArray();
        if (CurrentLevel >= 9)
        {
            
            int randomLevel = Random.Range(1, _levelList.Length + 1);
            BasketScript.SetLevel(randomLevel);  
            string levelName = randomLevel.ToString();
            _levelText.text = levelName;
            _levelText.color = Color.magenta;
            foreach (GameObject go in _levelList)
            {
                if (go.gameObject.name == levelName)
                {
                    Instantiate(go, _platformsParent.transform, true);
                    BasketScript.SetLevel(randomLevel);
                }
            }
        } else
        {
            CurrentLevel++;
            _levelList = Resources.LoadAll("Prefabs").Cast<GameObject>().ToArray();
            if (_savedPrefabs != null)
            {
                foreach (Transform savedPrefab in _savedPrefabs)
                {
                    savedPrefab.gameObject.SetActive(true);
                    int levelNo = Convert.ToInt32(savedPrefab.name);
                    foreach (GameObject setLevel in _levelList)
                    {
                        _levelList[levelNo] = savedPrefab.gameObject;
                    }
                }
            }
            string levelName = CurrentLevel.ToString();
            _levelText.text = levelName;
            foreach (GameObject go in _levelList)
            {
                if (go.gameObject.name == levelName)
                {
                    Instantiate(go, _platformsParent.transform, true);
                    BasketScript.SetLevel(CurrentLevel);
                }
            }
        }
    }

    public void ChangeScene() {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 0)
        {
            SceneManager.LoadScene(currentScene + 1, LoadSceneMode.Single);
        }
        if (currentScene == 1)
        {
            SceneManager.LoadScene(currentScene - 1, LoadSceneMode.Single);
        }
    }
}
