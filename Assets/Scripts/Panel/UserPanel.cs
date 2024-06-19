using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UserPanel : BasePanel
{
    public Button btnOk;
    public Button btnCancel;
    public Button btnDelete;
    public ScrollRect scroll;           //滚动容器
    public GameObject UserNamePrefab;     //用户名预制件
    private List<UserData> testData;    //测试数据
    private Dictionary<string,UserNameItem> menuNameItems;  //字典容器容纳所有子节点
    //将UserNameItem的具体数据信息存放到UserPanel上
    private string curUserName;
    public string CurName
    {
        get => curUserName;
        set 
        {
            curUserName = value; 
            RefreshSelectState();
        } 
    }

    protected override void Awake()
    {
        base.Awake();
        btnOk.onClick.AddListener(OnBtnOK);
        btnCancel.onClick.AddListener(OnBtnCancel);
        btnDelete.onClick.AddListener(OnBtnDelete);

        // testData = new List<UserData>();
        // testData.Add(new UserData("yzr1",1));
        // testData.Add(new UserData("yzr2",2));
        EventCenter.Instance.AddEventListener<UserData>(EventType.EventNewUserCreate,OnEventNewUserCreate);
        EventCenter.Instance.AddEventListener<UserData>(EventType.EventUserDelete,OnEventUserDelete);
    }

    private void OnEventUserDelete(UserData userData)
    {
        RefreshMainPanel();
    }

    //监听响应函数
    private void OnEventNewUserCreate(UserData userData)
    {
        RefreshMainPanel();
    }

    private void Start() {
        RefreshMainPanel();
        //select item
        CurName = BaseManager.Instance.currentUserName;
    }

    private void RefreshMainPanel()
    {
        //remove all children
        foreach(Transform child in scroll.content)
        {
            //print(child);
            Destroy(child.gameObject);
        }
        //init all children
        menuNameItems = new Dictionary<string, UserNameItem>();
        foreach(UserData userData in LocalConfig.LoadAllUserData())
        {
            Transform prefab = Instantiate(UserNamePrefab).transform; 
            prefab.SetParent(scroll.content,false);
            prefab.localPosition = Vector3.zero;
            prefab.localScale = Vector3.one;
            prefab.localRotation = Quaternion.identity;
            prefab.GetComponent<UserNameItem>().InitItem(userData,this);
            menuNameItems.Add(userData.name,prefab.GetComponent<UserNameItem>());
        }
        //add new user item
        Transform newPrefab = Instantiate(UserNamePrefab).transform;
        newPrefab.SetParent(scroll.content,false);
        newPrefab.localPosition = Vector3.zero;
        newPrefab.localScale = Vector3.one;
        newPrefab.localRotation = Quaternion.identity;
        newPrefab.GetComponent<UserNameItem>().InitNewUserItem();
    }

    //刷新子节点的所有选中状态
    public void RefreshSelectState()
    {
        foreach(UserNameItem item in menuNameItems.Values)
        {
            item.RefreSelect();
        }
        
    }

        private void OnBtnOK()
    {
        if(CurName != "")
        {
            BaseManager.Instance.SetCurrntUserName(CurName);
            ClosePanel();
        }
        else
        {
            print(">>>>>>>>>>>>>>>Cur name is empty");
        }
        
    }

    private void OnBtnCancel()
    {
        ClosePanel();
    }

    private void OnBtnDelete()
    {
        //如果没有选中K
        if(CurName == "")
            return;
        bool isSuccess =  LocalConfig.ClearUserData(CurName);
        //如果删除成功并且当前名称等于当前用户名
        if(isSuccess && CurName == BaseManager.Instance.currentUserName)
        {
            List<UserData> users = LocalConfig.LoadAllUserData();
            if(users.Count > 0)
            {
                BaseManager.Instance.SetCurrntUserName(users[0].name);
            }
            else
            {
                BaseManager.Instance.SetCurrntUserName("");
                BaseUIManager.Instance.OpenPanel(UIConst.NewUserPanel);
            }
        }
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        EventCenter.Instance.RemoveEventListener<UserData>(EventType.EventNewUserCreate,OnEventNewUserCreate);
        EventCenter.Instance.RemoveEventListener<UserData>(EventType.EventUserDelete,OnEventUserDelete);
    }

}
