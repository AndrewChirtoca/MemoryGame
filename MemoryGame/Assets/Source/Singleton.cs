//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80
//
//  Creator: Chirtoca Andrei <andrewchirtoca@gmail.com>
//
//===----------------------------------------------------------------------===//



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;



public class Singleton<T> : MonoBehaviour where T: class
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Singleton error! make sure you have a " + typeof(T).FullName + " in your scene");
            }
            return _instance;
        }
    }

    public void RegisterSingleton(T instance)
    {
        Assert.IsNull(_instance);
        Assert.IsNotNull(instance);
        _instance = instance;
    }

    private static T _instance;
}
