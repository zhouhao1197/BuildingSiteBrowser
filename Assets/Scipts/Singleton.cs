﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>:MonoBehaviour where T:Singleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }
}
