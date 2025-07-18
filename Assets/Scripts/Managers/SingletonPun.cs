using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingletonPun<T> : MonoBehaviourPun where T : MonoBehaviourPun
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
