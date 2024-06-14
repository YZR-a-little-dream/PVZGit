using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;

public class Sun : MonoBehaviour
{
    public float duration;
    private float timer;

    //从空中飘落的太阳
    public Vector3 targetPos;

    //太阳渐变
    private SpriteRenderer sr;

    void Start()
    {
        timer=0;
        sr = GetComponent<SpriteRenderer>();
        CreateSun();
    }

    //缩放太阳
    private void CreateSun()
    {
        transform.DOScale(0,0.5f).From();
        sr.DOFade(0,0.5f).From();
    }
    public void SetTargetPos(Vector3 pos)
    {
        targetPos = pos;
    }

    void Update()
    {
        //先移动到落点
        if(targetPos != Vector3.zero && Vector3.Distance(targetPos,transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPos,0.1f);
            return;
        }
        //再延时销毁
        timer += Time.deltaTime;
        if(timer > duration)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        //TODO: 飞到UI太阳所在位置，然后销毁
        GameObject.Destroy(gameObject);
        //点击后：增加阳光数量
        GameManager.Instance.ChangeSunNum(25);
    }
}
