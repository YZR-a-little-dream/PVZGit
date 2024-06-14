using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEditor;
using DG.Tweening;
using JetBrains.Annotations;

public class Card : MonoBehaviour,IPointerClickHandler
{
    
    public GameObject objectPrefab;         //卡片对应的物体预制件
    private GameObject curGameObject;        //记录当前创建出来的物体
    [HideInInspector] public GameObject darkBg;
    [HideInInspector] public GameObject progressBar;
    public float waitTime;
    private float timer;
    public int useSun;

    //容纳卡片的plantInfo
    public PlantInfoItem plantInfoItem;
    //有没有从下面的选卡栏中选中变成我们的卡片   开始游戏后锁定选卡栏避免选卡栏能重复使用       是否移动
    public bool hasUse = false,hasLock = false,isMoving = false;

    //卡片是否启动
    public bool hasStart = false;

    void Start()
    {
        darkBg = transform.Find("dark").gameObject;
        progressBar = transform.Find("progress").gameObject;

        //还没开始游戏先去除压黑操作
        darkBg.SetActive(false);
        progressBar.SetActive(false);
    }

    void Update()
    {
        if(!GameManager.Instance.gameStart)
            return;
        if(!hasStart)
        {
            hasStart = true;
            darkBg.SetActive(true);
            progressBar.SetActive(true);
        }

        timer += Time.deltaTime;
        UpdateProgress();
        UpdateDaekBg();
    }

    void UpdateProgress()
    {
        float per = Mathf.Clamp(timer/waitTime,0,1);
        progressBar.GetComponent<Image>().fillAmount = 1-per;
    }

    void UpdateDaekBg()
    {
        if(progressBar.GetComponent<Image>().fillAmount == 0 
            && GameManager.Instance.sunNum >= useSun)
        {
            darkBg.SetActive(false);
        }else
        {
            darkBg.SetActive(true);
        }
    }

    //点击卡片开始拖拽
    public void OnBeginDrag(BaseEventData data)
    {
        if(!hasStart)
        {
            return;
        }
        //判断是否可以种植，压黑存在则无法种植
        if(darkBg.activeSelf)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject = Instantiate(objectPrefab);
        
        curGameObject.transform.position = TranlateScreenToWorld(pointerEventData.position);
        //播放点击卡片的声音
        SoundManager.Instance.PlaySound(Globals.S_Seedlift);
    }

    public void OnDrag(BaseEventData data)
    {
        if(curGameObject == null)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        //根据鼠标移动的位置对应移动
        curGameObject.transform.position = TranlateScreenToWorld(pointerEventData.position);
    }

    public void OnEndDrag(BaseEventData data)
    {
        if(curGameObject == null)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        //拿到鼠标所在的碰撞体
        Collider2D[] col = Physics2D.OverlapPointAll(TranlateScreenToWorld(pointerEventData.position));
        foreach(Collider2D col2 in col)
        {
            //判断为“土地”，并且土地上没有其他植物
            if(col2.tag == "Land" && col2.transform.childCount == 0)
            {
                //把当前物体添加为土地的子物体
                curGameObject.transform.parent = col2.transform;
                curGameObject.transform.localPosition = Vector3.zero;
                //启用植物碰撞器和动画
                curGameObject.GetComponent<Plant>().SetPlantStart();
                //播放种植到土地上的声音
                SoundManager.Instance.PlaySound(Globals.S_Plant);
                //重置默认值，生成结束
                curGameObject = null;
                //扣除太阳花费
                GameManager.Instance.ChangeSunNum(-useSun);
                //重置计时器
                timer = 0;
                
                break;
            }
        }

        //如果没有符合条件的土地，则curGameObject还存在着，那么销毁他
        if(curGameObject != null)
        {
            Destroy(curGameObject);
            curGameObject=null;
        }
    }

    //鼠标坐标转换为世界坐标
    public static Vector3 TranlateScreenToWorld(Vector3 position)
    {
        Vector3 cameraTranslatePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslatePos.x,cameraTranslatePos.y,0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isMoving || hasLock)
            return;
        if(hasUse)
        {
            ReMoveCard(gameObject);    
        }else{
            AddCard(gameObject);
        }
    }

    //从选卡栏移除卡片
    public void ReMoveCard(GameObject removeCard)
    {
        ChoseCardPannel choseCardPannel = UIManager.Instance.choseCardPannel;
        if(choseCardPannel.chooseCard.Contains(removeCard))
        {
            //移除操作
            removeCard.GetComponent<Card>().isMoving = true;
            choseCardPannel.chooseCard.Remove(removeCard);
            //选卡栏归位操作
            choseCardPannel.UpdateCardPosition();
            //移动回原来的位置
            Transform cardParent = UIManager.Instance.allCardPanel.Bg.transform
                .Find("Card" + removeCard.GetComponent<Card>().plantInfoItem.plantId);
            Vector3 curPosition = removeCard.transform.position;
            removeCard.transform.SetParent(UIManager.Instance.transform,false);
            removeCard.transform.position = curPosition;
            //DOMove
            removeCard.transform.DOMove(cardParent.position,0.3f).OnComplete(
                () =>{
                    // hasLock = false;
                    // darkBg.SetActive(false);
                    cardParent.Find("BeforeCard").GetComponent<Card>().darkBg.SetActive(false);
                    cardParent.Find("BeforeCard").GetComponent<Card>().hasLock = false;
                    removeCard.GetComponent<Card>().isMoving = false;
                    Destroy(removeCard);
                }
            );
        }
    }

    //添加卡片到选卡栏
    public void AddCard(GameObject gameObject)
    {
        
        ChoseCardPannel choseCardPannel = UIManager.Instance.choseCardPannel;
        //记录当前卡片的张数
        int curIndex = choseCardPannel.chooseCard.Count;
        if(curIndex > 8)
        {
            print("已经选中的卡片超过最大数量");
            return;
        }

        GameObject useCard = Instantiate(plantInfoItem.cardPrefab);
        useCard.transform.SetParent(UIManager.Instance.transform);
        useCard.transform.position = transform.position;
        useCard.name = "Card";
        useCard.GetComponent<Card>().plantInfoItem = plantInfoItem;
        hasLock = true;
        darkBg.SetActive(true);
        //移动到目标位置
        Transform targetObject = choseCardPannel.cards.transform.Find("Card" + curIndex);
        useCard.GetComponent<Card>().isMoving = true;
        useCard.GetComponent<Card>().hasUse = true;
        choseCardPannel.chooseCard.Add(useCard);
        //DoMove进行移动
        useCard.transform.DOMove(targetObject.position,0.3f).OnComplete(
            () =>{
                useCard.transform.SetParent(targetObject,false);
                useCard.transform.localPosition = Vector3.zero;
                useCard.GetComponent<Card>().isMoving = false;
            }
        );
    }
}
