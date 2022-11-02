using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR.Management;
using System;

public class GameMng : MonoBehaviour
{
    static private GameMng _instance;
    public static GameMng I
    {
        get
        {
            if (_instance.Equals(null))
            {
                // 여기 들어오면 안됨
                Debug.LogError("Instance is null");
            }
            return _instance;
        }
    }

    public Vr_Input input;
    public bool _isVR = false;
    public bool SingleGame = false;
    public Define.VRType VR_Type = Define.VRType.Vive;

    void Awake()
    {
        _instance = this;
        Managers.Init();
        DontDestroyOnLoad(this);
    }
}
