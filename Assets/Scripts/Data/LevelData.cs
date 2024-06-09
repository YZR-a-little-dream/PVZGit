using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "configureData",fileName = "Data",order = 1)]
public class LevelData : ScriptableObject
{
    public List<LevelItem> LevelDataList = new List<LevelItem>();
}

[System.Serializable]
public class LevelItem
{
    public int id;
    public int levelId;
    public int progressId;
    public int createTime;
    public int zombieType;
    public int bornPos;
}


//关卡信息
[CreateAssetMenu(menuName = "Data",fileName = "LevelInfo",order = 1)]
public class LevelInfo : ScriptableObject
{
    public List<LevelInfoItem> LevelInfoList = new List<LevelInfoItem>();
}

[System.Serializable]
public class LevelInfoItem
{
    public int levelId;
    public string LevelName;
    public float[] progressPercent;

    public override string ToString()
    {
        return "[id]:" + levelId.ToString();
    }
}