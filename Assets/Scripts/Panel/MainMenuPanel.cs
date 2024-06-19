using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public Button btnChangeUser;
    //左上角用户名
    public Text userNameText;
    //右上角关卡小关
    public Text smallLevelText;

    protected override void Awake()
    {
        base.Awake();
        btnChangeUser.onClick.AddListener(OnBtnChangeUser); 
        EventCenter.Instance.AddEventListener<UserData>(EventType.EventNewUserCreate,OnEventNewUserCreate);
        EventCenter.Instance.AddEventListener<string>(EventType.EventCurrentUserChange,OnEventCurrentUserChange);  
    }

    private void OnEventCurrentUserChange(string curName)
    {
        userNameText.text  = curName;
        if(LocalConfig.LoadUserData(curName) == null)
        {
            smallLevelText.text = "1";
            return;
        }
        smallLevelText.text = LocalConfig.LoadUserData(curName).level.ToString();
    }

    private void Start() {
        if(BaseManager.Instance.currentUserName == "")
        {
            //第一次进来没有创建过用户
            BaseUIManager.Instance.OpenPanel(UIConst.NewUserPanel);
            return;
        }
        //初始化用户名和关卡信息
        userNameText.text = BaseManager.Instance.currentUserName;
        UserData userData = LocalConfig.LoadUserData(BaseManager.Instance.currentUserName);
        if(userData != null)
        {
            smallLevelText.text = userData.level.ToString();
        }
    }

    private void OnEventNewUserCreate(UserData userData)
    {
        userNameText.text = userData.name;
        smallLevelText.text = userData.level.ToString();
    }

    private void OnBtnChangeUser()
    {
        //TODO: 打开用户列表界面
        BaseUIManager.Instance.OpenPanel(UIConst.UserPanel);
    }
}
