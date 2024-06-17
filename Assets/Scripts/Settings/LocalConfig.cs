using System.IO;
using Newtonsoft.Json;
using UnityEngine;                  //Application.persistentDataPath配置
using System.Collections.Generic;   //使用字典命名空间
public class LocalConfig 
{
    //修改1:增加userData缓存数据
    public static Dictionary<string,UserData> usersDataDic = new Dictionary<string, UserData>();
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