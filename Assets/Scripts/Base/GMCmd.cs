using UnityEditor;
using UnityEngine;

class GMCmd
{
    [MenuItem("GMCmd/SaveLocalConf")]
    public static void SaveLocalConfig()
    {
        for (int i = 0; i < 5; i++)
        {
            UserData userData = new UserData();
            userData.name = "xiaoqi" + i.ToString();
            userData.level = i;
            LocalConfig.SaveUserData(userData);
        }
        Debug.Log("Save End!!!!!!!!!!!!");
    }

    [MenuItem("GMCmd/GetLocalConfig")]
    public static void GetLocalConfig()
    {
        for (int i = 0; i < 5; i++)
        {
            string name = "xiaoqi" + i.ToString();
            UserData userData = LocalConfig.LoadUserData(name);
            Debug.Log(userData.name);
            Debug.Log(userData.level);
            //Debug.Log(userData.test);
        }
    }

}
