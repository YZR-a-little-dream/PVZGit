using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text sunNumText;

    public ProgressPanel progressPanel;

    public void InitUI()
    {
        sunNumText.text = GameManager.Instance.sunNum.ToString();
        progressPanel.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        sunNumText.text = GameManager.Instance.sunNum.ToString();
    }

    //初始化旗子位置
    public void InitProgressPanel()
    {
        LevelInfoItem levelInfo = GameManager.Instance.levelInfo.LevelInfoList
                                    [GameManager.Instance.curLevelId];
        for (int i = 0; i < levelInfo.progressPercent.Length;i++)
        {
            //拿到配置的数据，并且在指定位置生成旗子
            float percent = levelInfo.progressPercent[i];
            if(percent == 1)
            {
                continue;
            }
            progressPanel.SetFlagPercent(percent);
        }
        //初始化进度为0
        progressPanel.SetPercent(0);
        progressPanel.gameObject.SetActive(true);
    }

    public void UpdateProgressPanel()
    {
        //TODO: 拿到当前波次的僵尸总数
        int progressNum = 0;
        for(int i = 0; i < GameManager.Instance.levelData.LevelDataList.Count ; i++)
        {
            LevelItem levelItem = GameManager.Instance.levelData.LevelDataList[i];
            if(levelItem.levelId == GameManager.Instance.curLevelId && 
                levelItem.progressId == GameManager.Instance.curProgressId)
            {
                progressNum++;
            }
        }
        
        Debug.Log(progressNum);
        //当前波次剩余僵尸数量
        int remainNum = GameManager.Instance.curProgressZombie.Count;
        //当前波次进行到多少百分比
        float percent = (float)(progressNum - remainNum) / progressNum;
        //当前波次比例，前一次波次比例
        LevelInfoItem levelInfoItem = GameManager.Instance.levelInfo.LevelInfoList
                [GameManager.Instance.curLevelId];
        float progressPercent = levelInfoItem.progressPercent[GameManager.Instance.curProgressId - 1];
        float lastProgressPercent = 0;
        if(GameManager.Instance.curProgressId > 1)
        {
            lastProgressPercent = levelInfoItem.progressPercent[GameManager.Instance.curProgressId - 2];
        }
        //最终比例 = 当前比例 + 前一次百分比
        float finalPercent = percent *(progressPercent - lastProgressPercent) + lastProgressPercent;
        progressPanel.SetPercent(finalPercent);
    }
}
