using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantInfoItem
{
    public int plantId;
    public string plantName;
    public string description;
    public GameObject cardPrefab;
    //这些信息已经存储在卡片中
    // public int useNum;
    // public int cdTime;
    // public GameObject prefab;
}