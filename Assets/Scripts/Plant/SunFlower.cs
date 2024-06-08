using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SunFlower : Plant
{
    public GameObject sunPrefab;
    
    public float readyTime;
    private float timer;

    //在太阳花两侧生成的随机数
    private int sunNum;

    protected override void Start() {
        base.Start();
        timer = 0;
        sunNum = 0;
    }

    private void Update() {
        if(!start)
            return;
        timer += Time.deltaTime;
        if(timer > readyTime) {
            animator.SetBool("Ready", true);
        }
    }

    public void BornSunOver()
    {
        BronSun();
        animator.SetBool("Ready",false);
        timer = 0;
    }

    //生成太阳
    private void BronSun()
    {
        GameObject sunNew = Instantiate(sunPrefab);
        sunNum += 1;
        float randomX;
        if(sunNum % 2 ==1)
        {
            randomX = Random.Range(transform.position.x - 30,transform.position.x - 20);
        }else
        {
            randomX = Random.Range(transform.position.x + 30,transform.position.x + 20);
        }
        float randomY = Random.Range(transform.position.y - 20,transform.position.y + 20);
        sunNew.transform.position = new Vector2(randomX,randomY);
    }
}
