using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    private GameObject _button;
    private GameObject[] _levelList;
    private Transform _buttonParent;
    private Transform _prefabParent;
    private Transform _savedPrefabs;

    public Material selectedMat;
    public Material defaultMat;
    public GameObject ball;

    private List<GameObject> _selectedObjects = new List<GameObject>();

    void Start() {
        _levelList = Resources.LoadAll("Prefabs").Cast<GameObject>().ToArray();
        _buttonParent = GameObject.Find("ButtonList").transform;
        _prefabParent = GameObject.Find("PrefabParent").transform;
        _savedPrefabs = GameObject.Find("SavedPrefabs").transform;
        foreach (GameObject go in _levelList)
        {
            if (go.name != "0")
            {
                _button = DefaultControls.CreateButton( new DefaultControls.Resources() );
                _button.gameObject.transform.SetParent(_buttonParent);
                _button.gameObject.transform.localScale = new Vector3(1.8f, 2, 1);
                _button.gameObject.GetComponentInChildren<Text>().text = go.name;
                Button buttonComponent = _button.GetComponentInChildren<Button>();
                buttonComponent.onClick.AddListener(delegate{TaskOnClick(go.name);});
            }
        }
    }

    private void TaskOnClick(string goName)
    {
        ClearSelection();
        int index = Int32.Parse(goName);
        if (_prefabParent.childCount > 0)
        {
            foreach(Transform child in _prefabParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
        GameObject go = Instantiate(_levelList[index], _prefabParent);
        go.name = go.name.Replace("(Clone)","").Trim();
        go.GetComponent<PlatformController>().enabled = false;
        go.GetComponent<Animation>().enabled = false;
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if ( Input.GetMouseButtonDown (0) || (Input.touchCount > 0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 1f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Ball"))
                {
                    _selectedObjects.Add((hit.collider.gameObject));
                    Debug.Log(_selectedObjects);
                    foreach (GameObject obj in _selectedObjects)
                    {
                        obj.GetComponent<Renderer>().sharedMaterial = selectedMat;
                    }
                }
            }
        }
    }

    public void GoUpwards()
    {
        if (_selectedObjects.Count > 0)
        {
            foreach (GameObject go in _selectedObjects)
            {
                if (go.transform.localPosition.z < 35f)
                {
                    go.transform.localPosition += new Vector3(0f, 0f,0.5f);
                }
            }
        }
    }

    public void GoDownwards()
    {
        if (_selectedObjects.Count > 0)
        {
            foreach (GameObject go in _selectedObjects)
            {
                if (go.transform.localPosition.z > 1f)
                {
                    go.transform.localPosition += new Vector3(0f, 0f,-0.5f);
                }
            }
        }
    }

    public void GoLeft()
    {
        if (_selectedObjects.Count > 0)
        {
            foreach (GameObject go in _selectedObjects)
            {
                if (go.transform.localPosition.x > -3.5f)
                {
                    go.transform.localPosition += new Vector3(-0.3f, 0f,0f);
                }
            }
        }
    }

    public void GoRight()
    {
        if (_selectedObjects.Count > 0)
        {
            foreach (GameObject go in _selectedObjects)
            {
                if (go.transform.localPosition.x < 3.5f)
                {
                    go.transform.localPosition += new Vector3(0.3f, 0f,0f);
                }
            }
        }
    }

    public void ClearSelection()
    {
        foreach (GameObject obj in _selectedObjects)
        {
            obj.GetComponent<Renderer>().sharedMaterial = defaultMat;
        }
        _selectedObjects.Clear();
    }

    public void DeleteSelection()
    {
        foreach (GameObject go in _selectedObjects)
        {
            Destroy(go);
        }
        ClearSelection();
    }

    public void AddBalls()
    {
        if (_prefabParent != null)
        {
            foreach (Transform selectedPrefab in _prefabParent)
            {
                GameObject bundle = Instantiate(ball, selectedPrefab.gameObject.transform.GetChild(2));
                // bundle.transform.localPosition = new Vector3(2, 0, -5f);
            }
        }
    }

    public void SaveLevels()
    {
        foreach (GameObject obj in _selectedObjects)
        {
            obj.GetComponent<Renderer>().sharedMaterial = defaultMat;
        }
        
        foreach (Transform go in _prefabParent)
        {
            if (_savedPrefabs.transform.Find(go.gameObject.name))
            {
                Destroy(go.gameObject);
            }
            go.transform.SetParent(_savedPrefabs);
            go.gameObject.SetActive(false);
            go.GetComponent<PlatformController>().enabled = true;
            go.GetComponent<Animation>().enabled = true;
        }
    }

    public void ChangeScene()
    {
        LevelManager.CurrentLevel = 0;
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

    public void DefaultToMat()
    {
        foreach (GameObject obj in _selectedObjects)
        {
            obj.GetComponent<Renderer>().sharedMaterial = defaultMat;
        }
    }
}
