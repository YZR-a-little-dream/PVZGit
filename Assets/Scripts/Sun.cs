using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float duration;
    private float timer;

    //从空中飘落的太阳
    public Vector3 targetPos;

    void Start()
    {
        timer=0;
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
            transform.position = Vector3.MoveTowards(transform.position,targetPos,10);
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
