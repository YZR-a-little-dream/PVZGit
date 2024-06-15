using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public Slider slider;
    public GameObject btnStart;
    public float curProgress;           //当前进度条参数
    public float loadingTime;           //虚假的加载时间
    public bool really;                 //是否真实加载
    private AsyncOperation operation;   //获取进度条的operation
    void Start()
    {
        btnStart.SetActive(false);
        btnStart.GetComponent<Button>().onClick.AddListener(OnBtnStart);
        curProgress = 0;
        slider.value = curProgress;

        loadingTime = 2.0f;
        //really = true;
        if(really)
        {
            operation = SceneManager.LoadSceneAsync("Menu");
            operation.allowSceneActivation = false;
        }
    }

    void OnBtnStart()
    {
        if(!really)
        {
            SceneManager.LoadScene("Menu");
        }else
        {
            operation.allowSceneActivation = true;
        }
        DOTween.Clear();
    }

    void OnSlierValueChange(float value)
    {
        slider.value = value;
        if(value >= 1.0)
        {
            btnStart.SetActive(true);
        }
    }

    void Update()
    {
        if(!really)
        {
            curProgress += Time.deltaTime/loadingTime;
            if(curProgress > 1.0)
            {
                curProgress = 1;
            }
            OnSlierValueChange(curProgress);
        }
        else
        {
            curProgress = Mathf.Clamp01(operation.progress / 0.9f);
            OnSlierValueChange(curProgress);
        }
    }
}
