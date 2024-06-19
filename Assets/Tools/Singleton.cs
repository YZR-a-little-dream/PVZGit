using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Singleton<T>,new()
{
    
    private static T instance;
    public static T Instance
    {
        get{
            if(instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    protected virtual void Awake() 
    {
        // if(instance != null)
        // { Destroy(gameObject); }
        // else
        // { instance = (T)this; }
        instance = (T)this;
    }
}
