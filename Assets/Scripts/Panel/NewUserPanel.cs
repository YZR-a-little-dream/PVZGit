using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewUserPanel : BasePanel
{
    public Button BtnOk;
    public Button BtnCancel;
    public InputField inputField;       //文本框
    private string inputString;          //输入的文本内容
    protected override void Awake()
    {
        base.Awake();
        BtnOk.onClick.AddListener(OnBtnOK);
        BtnCancel.onClick.AddListener(OnBtnCancel);
        inputField.onValueChanged.AddListener(OnInputChange);
    }

    private void OnInputChange(string value)
    {   
        inputString = value;
    }

    private void OnBtnOK()
    {
        // 修改用户名
        print("OnBtnOk");
        //去除前后空字符串
        if(inputString.Trim() == "")
        {
           print("输入的字符串为空");
            return;
        }
        else if(LocalConfig.LoadUserData(inputString) != null)
        {
            print("用户名已经存在");
        }

        //创建新用户
        UserData userData = new UserData();
        userData.name = inputString;
        userData.level = 1;
        LocalConfig.SaveUserData(userData);

        //更改当前用户选择
        BaseManager.Instance.SetCurrntUserName(userData.name);

        //广播新用户创建的消息
        EventCenter.Instance.EventTrigger<UserData>(EventType.EventNewUserCreate,userData);

        ClosePanel();
    }

    private void OnBtnCancel()
    {
        print("OnBtnCancel");
        if(BaseManager.Instance.currentUserName == "")
        {
            print(">>>>>>> BaseManager.Instance.currentUserName is empty!!!!");
            return;
        }
        ClosePanel();
    }
    
    
}
