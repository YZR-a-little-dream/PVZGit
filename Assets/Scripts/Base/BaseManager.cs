using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager 
{
    private static BaseManager _instance;
    public static BaseManager Instance
    {
        get{
            if(_instance == null)
            {
                _instance = new BaseManager();
            }
            return _instance;
        }
    }

    public string currentUserName = "";
    public BaseManager()
    {
        Debug.Log("----------- GameStart --------------");
        currentUserName = GetClientData().curUserName;
    }

    private ClientData GetClientData()
    {
        return LocalConfig.LoadClientData();
    }

    //设置用户名
    public void SetCurrntUserName(string name)
    {
        currentUserName = name;
        //save
        ClientData clientData = GetClientData();
        clientData.curUserName = name;
        LocalConfig.SaveClientData(clientData);

        EventCenter.Instance.EventTrigger<string>(EventType.EventCurrentUserChange,currentUserName);
    }
}
