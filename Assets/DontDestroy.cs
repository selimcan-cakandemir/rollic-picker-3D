using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy _dontDestroy;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (_dontDestroy == null)
        {
            _dontDestroy = this;
        }
        else
        {
            DestroyObject(gameObject);
        }

    }
}
