using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewUserPanel : BasePanel
{
    public Button BtnOk;
    public Button BtnCancel;

    protected override void Awake()
    {
        base.Awake();
        BtnOk.onClick.AddListener(OnBtnOK);
        BtnCancel.onClick.AddListener(OnBtnCancel);
    }

    private void OnBtnOK()
    {
        //TODO: 修改用户名
        print("OnBtnOk");
        ClosePanel();
    }

    private void OnBtnCancel()
    {
        print("OnBtnCancel");
        ClosePanel();
    }
    
    
}
