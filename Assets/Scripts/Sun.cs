using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float duration;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer=0;
    }

    // Update is called once per frame
    void Update()
    {
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
