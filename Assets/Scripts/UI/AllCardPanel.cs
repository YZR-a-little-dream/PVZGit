using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCardPanel : MonoBehaviour
{
   public GameObject Bg;
   //卡片预制件
   public GameObject beforeCardPrefab;

   private void Start() {
        //生成选卡栏的40个格子
        for(int i = 0; i < 40 ; i++)
        {
            GameObject beforeCard = Instantiate(beforeCardPrefab);
            beforeCard.transform.SetParent(Bg.transform,false);
            beforeCard.name = "Card" + i.ToString();
        }
        //需要得到数据 才能初始化选卡栏卡片 所以InitCards()要放在UIMananger
   }

   public void InitCards()
   {
    foreach(PlantInfoItem plantInfo in GameManager.Instance.plantInfo.plantInfoList)
    {
        Transform cardParent = Bg.transform.Find("Card" + plantInfo.plantId);
        GameObject reallyCard = Instantiate(plantInfo.cardPrefab) as GameObject;
        reallyCard.transform.SetParent(cardParent,false);
        reallyCard.transform.localPosition = Vector2.zero;
        reallyCard.name = "BeforeCard";
    }
   }
}
