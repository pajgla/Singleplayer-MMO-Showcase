using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T ms_Instance = null;
    [SerializeField] bool m_DestroyOnLoad = false;

    protected virtual void Awake()
    {
        if (ms_Instance != null && ms_Instance != this)
        {
            Debug.LogError("Trying to instantiate a singleton while one instance is already active. This instance will be destroyed.");
            Destroy(this.gameObject);
            return;
        }
        
        ms_Instance = this as T;

        if (m_DestroyOnLoad == false)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static T Get()
    {
        if (ms_Instance == null)
        {
            Debug.LogError("No instance of this singleton is active. Class: " + typeof(T).Name);
        }

        return ms_Instance;
    }
}
