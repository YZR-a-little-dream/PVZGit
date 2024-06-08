using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int sunNum;
    
    public GameObject bornParent;       //僵尸生成位置的父物体
    public GameObject zombiePrefab;     //僵尸生成预制体
    public float createzombieTime;      //僵尸生成间隔时间

    private void Start() {
        //初始化UI控件
        UIManager.Instance.InitUI();

        CreateZombie();
    }

    public void ChangeSunNum(int changeNum)
    {
        sunNum += changeNum;
        if(sunNum <=0)
            sunNum = 0;
        
        //TODO: 阳光数量发生改变，通知卡片压黑等...
        UIManager.Instance.UpdateUI();
    }

    public void CreateZombie()
    {
        StartCoroutine(DalayCreateZombie());
    }

    IEnumerator DalayCreateZombie()
    {
        //等待
        yield return new WaitForSeconds(createzombieTime);

        //生成
        GameObject zombie = Instantiate(zombiePrefab);
        int index = Random.Range(0,5);
        Transform zombieLine = bornParent.transform.Find("born" + index.ToString());
        zombie.transform.SetParent(zombieLine);
        zombie.transform.localPosition = Vector3.zero;

        //再次启动定时器
        StartCoroutine(DalayCreateZombie());
    }
}
