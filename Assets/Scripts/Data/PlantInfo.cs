using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//关卡信息
[CreateAssetMenu(menuName = "Data/plantInfo",fileName = "PlantInfo",order = 1)]
public class PlantInfo : ScriptableObject
{
    public List<PlantInfoItem> plantInfoList = new List<PlantInfoItem>();
}

