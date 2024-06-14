using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChoseCardPannel : MonoBehaviour
{
    //容纳所有卡片的父物体
    public GameObject cards;
    //容纳卡片的背景栏
    public GameObject beforeCardPrefab;
    //选中卡片的容器
    public List<GameObject> chooseCard;

    private void Start() {
        chooseCard = new List<GameObject>();
        for (int i = 0; i < 8; i++)
        {
            GameObject beforeCard = Instantiate(beforeCardPrefab); 
            beforeCard.transform.SetParent(cards.transform,false);
            beforeCard.name = "Card" + i.ToString();
            beforeCard.transform.Find("Bg").gameObject.SetActive(false);
        }
    }

    //实现选卡栏的回位操作
    public void UpdateCardPosition()
    {
        for(int i =0;i<chooseCard.Count;i++)
        {
            GameObject useCard = chooseCard[i];
            Transform targetObject = cards.transform.Find("Card" + i.ToString());
            useCard.GetComponent<Card>().isMoving = true;
            //DOMove 进行移动
            useCard.transform.DOMove(targetObject.position,0.3f).OnComplete(
                () =>{
                    useCard.transform.SetParent(targetObject,false);
                    useCard.transform.localPosition = Vector3.zero;
                    useCard.GetComponent<Card>().isMoving = false;
                }
            );
        }
    } 
}
