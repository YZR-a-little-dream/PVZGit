using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//关卡信息
[CreateAssetMenu(menuName = "Data/LevelInfo",fileName = "LevelInfo",order = 2)]
public class LevelInfo : ScriptableObject
{
    public List<LevelInfoItem> LevelInfoList = new List<LevelInfoItem>();
}
