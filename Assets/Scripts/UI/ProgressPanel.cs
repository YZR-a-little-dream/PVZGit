using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanel : MonoBehaviour
{
    private GameObject Progress;
    private GameObject Head;
    private GameObject LevelText;
    private GameObject Bg;
    private GameObject Flag;
    private GameObject FlagPrefab;

    void Start()
    {
        Progress = transform.Find("Progress").gameObject;
        Head = transform.Find("Head").gameObject;
        LevelText = transform.Find("LevelText").gameObject;
        Bg = transform.Find("Bg").gameObject;
        Flag = transform.Find("Flag").gameObject;
        //从Resources中加载预制件
        FlagPrefab = Resources.Load("Prefab/Flag") as GameObject;
    }

    //设置进度条
    public void SetPercent(float per)
    {
        //图片进度条
        Progress.GetComponent<Image>().fillAmount = per;
        //进度条最右边位置（初始位置）
        float originPosX = Bg.GetComponent<RectTransform>().position.x 
                    + Bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        //进度条长度
        float width = Bg.GetComponent<RectTransform>().sizeDelta.x;
        //这个是自定义参数，用来做偏移，调整到自己认为合适的位置
        float offset = 0;
        //设置头的x轴位置：最右边的位置（数值为负数） - 进度条长度的百分比长度 + 自定义的偏移
        Head.GetComponent<RectTransform>().position = new Vector2
        (originPosX - per * width + offset,Head.GetComponent<RectTransform>().position.y);
    }

    //设置旗子百分比
    public void SetFlagPercent(float per)
    {
        Flag.SetActive(false);
        //进度条最右边位置（初始位置）
        float originPosX = Bg.GetComponent<RectTransform>().position.x 
                    + Bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        //进度条长度
        float width = Bg.GetComponent<RectTransform>().sizeDelta.x;
        //这个是自定义参数，用来做偏移，调整到自己认为合适的位置
        float offset = 5;
        //创建新的旗子
        GameObject newFlag = Instantiate(FlagPrefab);
        newFlag.transform.SetParent(gameObject.transform,false);
        newFlag.GetComponent<RectTransform>().position = Flag.GetComponent<RectTransform>().position;
        //设置位置
        //设置头的x轴位置：最右边的位置（数值为负数） - 进度条长度的百分比长度 + 自定义的偏移
        newFlag.GetComponent<RectTransform>().position = new Vector2
        (originPosX - per * width + offset,newFlag.GetComponent<RectTransform>().position.y);

        //设置僵尸头的层级
        Head.transform.SetAsLastSibling();
    }
}
