﻿using UnityEngine;

public abstract class Singleton <T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance != null) {
            Destroy(this);
            return;
        }

        Instance = this as T;
        transform.SetParent(null);
    }
}
