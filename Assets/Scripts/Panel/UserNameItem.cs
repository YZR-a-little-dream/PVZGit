using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserNameItem : MonoBehaviour
{
    private GameObject select;          //选中状态
    private Text txt;                   //设置用户名
    public UserData userData;           //用户信息
    public Button btn;                  //UserNameItem的按钮
    private UserPanel parent;           //父节点UserPanel
    public string ItemType = "name";    //此处用"name"来表示用户名称,用"new"表示创建新用户

    private void Awake() {
        txt = transform.Find("Name").GetComponent<Text>();
        select = transform.Find("Select").gameObject;
        select.SetActive(false);
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnBtnNameItem);
    }

    //初始化用户名称项
    public void InitItem(UserData userData,UserPanel userPanel)
    {
        this.userData = userData;
        txt.text = userData.name;
        parent = userPanel;
    }

    //创建新用户
    public void InitNewUserItem()
    {
        ItemType = "new";
        txt.text = "创建新用户";
    }

    private void OnBtnNameItem()
    {
        if(ItemType == "name")
        {
            //修改列表的选中状态
            //修改UserPanel中的当前名字
            parent.CurName = userData.name;
        }else if(ItemType == "new")
        {
            //TODO: 打开创建新用户的界面
            BaseUIManager.Instance.OpenPanel(UIConst.NewUserPanel);
        }
    }

    //子项刷新
    public void RefreSelect()
    {
        select.SetActive(userData.name == parent.CurName);
    }

}
