using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressPanel : MonoBehaviour
{
    private GameObject Progress;
    private GameObject Head;
    private GameObject LevelText;
    private GameObject Bg;

    void Start()
    {
        Progress = transform.Find("Progress").gameObject;
        Head = transform.Find("Head").gameObject;
        LevelText = transform.Find("LevelText").gameObject;
        Bg = transform.Find("Bg").gameObject;
    }

    
    void Update()
    {
        
    }
}
