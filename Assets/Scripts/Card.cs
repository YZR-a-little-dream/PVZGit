using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    
    public GameObject objectPrefab;         //卡片对应的物体预制件
    private GameObject curGameObject;        //记录当前创建出来的物体
    [HideInInspector] public GameObject darkBg;
    [HideInInspector] public GameObject progressBar;
    public float waitTime;
    private float timer;
    public int useSun;

    void Start()
    {
        darkBg = transform.Find("dark").gameObject;
        progressBar = transform.Find("progress").gameObject;
    }

        void Update()
    {
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

    public void OnBeginDrag(BaseEventData data)
    {
        //判断是否可以种植，压黑存在则无法种植
        if(darkBg.activeSelf)
        {
            return;
        }
        PointerEventData pointerEventData = data as PointerEventData;
        curGameObject = Instantiate(objectPrefab);
        
        curGameObject.transform.position = TranlateScreenToWorld(pointerEventData.position);
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
                curGameObject = null;
                //扣除太阳花费
                GameManager.Instance.ChangeSunNum(-useSun);
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
    
}
