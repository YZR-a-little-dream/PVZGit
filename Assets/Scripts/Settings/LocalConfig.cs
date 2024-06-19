using System.IO;
using Newtonsoft.Json;
using UnityEngine;                  //Application.persistentDataPath配置
using System.Collections.Generic;
using System;   //使用字典命名空间
public class LocalConfig 
{
    //修改1:增加userData缓存数据
    public static Dictionary<string,UserData> usersDataDic = new Dictionary<string, UserData>();
    //全局用户缓存数据 缓存在内存中
    public static ClientData clientData;
    //保存用户数据文本
    public static void SaveUserData(UserData userData)
    {
         // 在persistentDataPath下再创建一个/users文件夹，方便管理
        if (!File.Exists(Application.persistentDataPath + "/users"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/users");
        }
        //修改2：保存缓存数据
        usersDataDic[userData.name] = userData;

        // 转换用户数据为JSON字符串
        string jsonData = JsonConvert.SerializeObject(userData);
        // 将JSON字符串写入文件中(文件名为userData.name)
        File.WriteAllText(Application.persistentDataPath + string.Format("/users/{0}.json", userData.name), jsonData);                            
    }

    //读取用户数据到内存
    public static UserData LoadUserData(string userName)
    {
        //修改3：率先从缓存中读取数据，而不是从文本文件中读取
        if(usersDataDic.ContainsKey(userName))
        {
            return usersDataDic[userName];
        }
        string path = Application.persistentDataPath + string.Format("/users/{0}.json", userName);
        // string temp = ($"/users/{userName}.json");
        // string path = Application.persistentDataPath + temp; 

        // 检查用户配置文件是否存在
        if (File.Exists(path))
        {
            // 从文本文件中加载JSON字符串
            string jsonData = File.ReadAllText(path);
            // 将JSON字符串转换为用户内存数据
            UserData userData = JsonConvert.DeserializeObject<UserData>(jsonData);
            return userData;
        }
        else
        {
            return null;
        }
    }

    // 加载所有用户储存的信息
    public static List<UserData> LoadAllUserData()
    {
        string folderPath = Application.persistentDataPath + "/users";
        DirectoryInfo folder = new DirectoryInfo(folderPath);
        List<UserData> users = new List<UserData>();
        FileInfo[] allFiles = folder.GetFiles("*.json");
        //先检查内存
        if(allFiles.Length == usersDataDic.Count)
        {
            foreach(UserData userData in usersDataDic.Values)
            {
                users.Add(userData);
            }
            return users;
        }
        //从硬盘加载
        foreach(FileInfo file in allFiles)
        {
            UserData userData = LoadUserData(file.Name.Split('.')[0]);
            if(userData != null)
            {
                users.Add(userData);
            } 
        }

        return users;
    }

    public static bool ClearUserData(string userName)
    {
        string path = Application.persistentDataPath + string.Format("/users/{0}.json", userName);
        if(File.Exists(path))
        {
            UserData oldUserData = LoadUserData(userName);
            File.Delete(path);
            if(usersDataDic.ContainsKey(userName))
            {
                usersDataDic.Remove(userName);
            }
            //删除成功后进行广播
            EventCenter.Instance.EventTrigger<UserData>(EventType.EventUserDelete,oldUserData);
            return true;
        }
        else
        {
            Debug.Log("-------删除失败-------");
            return false;
        }
    }

    //保存用户数据文本
    public static void SaveClientData(ClientData data)
    {
        clientData = data;
        //转换数据为Json字符串
        string jsonData = JsonConvert.SerializeObject(clientData);
        //将Json字符串写入文件中（文件名为userData.name）
        File.WriteAllText(Application.persistentDataPath + "/client_data.json",jsonData);
    }

    //读取用户数据到内存
    public static ClientData LoadClientData()
    {
        //优先从缓存中读取数据，而不是从文本文件中读取
        if(clientData != null)
            return clientData;
        string path = Application.persistentDataPath + "/client_data.json";
        //检查用户配置文件是否存在
        if(File.Exists(path))
        {
            //从文本中加载Json字符串
            string jsonData = File.ReadAllText(path);
            //将字符串转换为用户内存数据
            ClientData clientData = JsonConvert.DeserializeObject<ClientData>(jsonData);
            return clientData;
        }else
        {
            clientData = new ClientData();
            //转换数据为Json字符串
            string jsonData = JsonConvert.SerializeObject(clientData);
            //将Json字符串写入文件中（文件名为userData.name）
            File.WriteAllText(Application.persistentDataPath + "/client_data.json",jsonData); 
            return clientData;
        }
    }

}

public class UserData
{
    public string name;
    public int level;
    public UserData()
    {

    }
    public UserData(string name,int level)
    {
        this.name = name;
        this.level = level;
    }
}

//存储游戏全局数据
public class ClientData
{
    public string curUserName = "";
    public override string ToString()
    {
        return "curUserName:" + curUserName;
    }
}