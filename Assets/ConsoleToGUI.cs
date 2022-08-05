using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleToGUI : MonoBehaviour
{
    //#if !UNITY_EDITOR
    static string _myLog = "";
    private string _output;
    private string _stack;
     
    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }
     
    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }
     
    public void Log(string logString, string stackTrace, LogType type)
    {
        _output = logString;
        _stack = stackTrace;
        _myLog = _output + "\n" + _myLog;
        if (_myLog.Length > 5000)
        {
            _myLog = _myLog.Substring(0, 4000);
        }
    }
     
    void OnGUI()
    {
        //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
        {
            _myLog = GUI.TextArea(new Rect(5, 5, Screen.width - 10, Screen.height - 10), _myLog);
        }
    }
    //#endif
}

