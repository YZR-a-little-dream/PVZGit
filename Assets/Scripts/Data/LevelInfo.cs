using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//关卡信息
[CreateAssetMenu(menuName = "Data",fileName = "LevelInfo",order = 1)]
public class LevelInfo : ScriptableObject
{
    public List<LevelInfoItem> LevelInfoList = new List<LevelInfoItem>();
}
