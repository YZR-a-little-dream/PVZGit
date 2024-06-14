using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int sunNum;
    
    public GameObject bornParent;       //僵尸生成位置的父物体
    public GameObject zombiePrefab;     //僵尸生成预制体
    public float createzombieTime;      //僵尸生成间隔时间
    private int zOrderIndex;            //僵尸生成层级 用来解决过多僵尸生成后僵尸闪烁的问题

    //读取表格数据(关卡波次，关卡波次)
    [HideInInspector] public LevelData levelData;
    [HideInInspector] public LevelInfo levelInfo;
    [HideInInspector] public PlantInfo plantInfo;
    public bool gameStart;
    //关卡
    public int curLevelId = 1;
    //僵尸波次
    public int curProgressId = 1;

    //容纳当前波次容器  用于后续做僵尸进度条
    public List<GameObject> curProgressZombie;

    private void Start() {
        curProgressZombie = new List<GameObject>();
        ReadTable();
    }

    //读取表格信息
    void ReadTable()
    {
        //StartCoroutine(LoadTable());
        LoadTableNew();
    }

    public void LoadTableNew()
    {
        levelData =  Resources.Load("TableData/Level") as LevelData;
        levelInfo = Resources.Load("TableData/LevelInfo") as LevelInfo;
        plantInfo = Resources.Load("TableData/plantInfo") as PlantInfo;
        GameStart();
    }
    // IEnumerator LoadTable()
    // {
    //     ResourceRequest request = Resources.LoadAsync("Level");
    //     ResourceRequest request2 = Resources.LoadAsync("LevelInfo");
    //     yield return request;
    //     levelData = request.asset as LevelData;
    //     yield return request2;
    //     levelInfo = request2.asset as LevelInfo;
        
    // }

    private void GameStart()
    {
        //初始化UI控件
        UIManager.Instance.InitUI();

    }

    public void GameReallyStart()
    {
        GameManager.Instance.gameStart = true;
        CreateZombie();
        InvokeRepeating("CreateSunDown",10,10);
        //播放背景音乐
        SoundManager.Instance.PlayBGM(Globals.BGM1);
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
        //StartCoroutine(DalayCreateZombie());
        curProgressZombie = new List<GameObject>();
        TableCreateZombie();
        //调用初始化面板的函数
        UIManager.Instance.InitProgressPanel();
    }

    //根据表格数据创建僵尸
    private void TableCreateZombie()
    {
        //判断是否是最后一个波次，如果表格中当前波次没有可以创建的僵尸，则游戏胜利
        bool canCreate = false;

        for(int i = 0; i< levelData.LevelDataList.Count;i++)
        {
            LevelItem levelItem = levelData.LevelDataList[i];
            if(levelItem.levelId == curLevelId && levelItem.progressId == curProgressId)
            {
                // 延迟一段时间后创建僵尸
                StartCoroutine(ITableCreateZombie(levelItem));
                //代表当前波次有僵尸
                canCreate = true;
            }
        }

        if(!canCreate)
        {
            StopAllCoroutines();
            curProgressZombie = new List<GameObject>();
            //TODO: 胜利之后的一些表现
            gameStart = false;
        } 
    }

    IEnumerator ITableCreateZombie(LevelItem levelItem)
    {
        yield return new WaitForSeconds(levelItem.createTime);
        //加载预制件：从Resources文件夹中加载
        GameObject zombiePrefab = Resources.Load("Prefab/Zombie" + 
        levelItem.zombieType.ToString()) as GameObject;
        GameObject zombie = Instantiate(zombiePrefab);

        Transform zombieLine = bornParent.transform
            .Find("born" + levelItem.bornPos.ToString());
        zombie.transform.SetParent(zombieLine);
        zombie.transform.localPosition = Vector3.zero;
        //解决僵尸过多导致的层架闪烁
        zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        zOrderIndex += 1;
        curProgressZombie.Add(zombie);
    }

    //僵尸死亡后从容器中移除
    public void ZombieDied(GameObject gameObject)
    {
        if(curProgressZombie.Contains(gameObject))
        {
            curProgressZombie.Remove(gameObject);
            //僵尸死亡后更新进度条
            UIManager.Instance.UpdateProgressPanel();
        }
        //当前波次的僵尸全部被消灭 开启下一个波次
        if(curProgressZombie.Count == 0)
        {
            curProgressId += 1;
            TableCreateZombie();
        }
    }

    //等待一定时间后，在随机行生成一只僵尸【废弃】
    // IEnumerator DalayCreateZombie()
    // {
    //     //等待
    //     yield return new WaitForSeconds(createzombieTime);

    //     //生成
    //     GameObject zombie = Instantiate(zombiePrefab);
    //     int index = Random.Range(0,5);
    //     Transform zombieLine = bornParent.transform.Find("born" + index.ToString());
    //     zombie.transform.SetParent(zombieLine);
    //     zombie.transform.localPosition = Vector3.zero;
    //     //后生成的僵尸层级靠后 用于修复僵尸闪烁Bug
    //     zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
    //     zOrderIndex +=1;

    //     //再次启动定时器
    //     StartCoroutine(DalayCreateZombie());
    // }

    public void CreateSunDown()
    {
        //获取左下角，右上角的世界坐标
        Vector3 leftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector3 rightTop = Camera.main.ViewportToWorldPoint(Vector2.one);
        //加载Sun预制体
        GameObject sunPrefab = Resources.Load("Prefab/Sun") as GameObject;
        //初始化太阳位置
        float x = Random.Range(leftBottom.x + 30,rightTop.x -30);
        Vector3 bornPos = new Vector3(x,rightTop.y,0);
        GameObject sun = Instantiate(sunPrefab,bornPos,Quaternion.identity);
        //设置目标位置
        float y = Random.Range(rightTop.y - 80,rightTop.y - 30);
        sun.GetComponent<Sun>().SetTargetPos(new Vector3(bornPos.x,y,0));

    }
    
}
