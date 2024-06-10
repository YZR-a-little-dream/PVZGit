using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchwood : MonoBehaviour
{
    //火焰子弹预制件
    private GameObject FireBulletPrefab;

    private void Start() {
        FireBulletPrefab = Resources.Load("Prefab/FireBullet") as GameObject;
    }

    //销毁普通子弹
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bullet")
        {
            //Debug.Log(other.tag + " "+  other.name);
            Bullet bullet = other.GetComponent<Bullet>();
            if(bullet.TorchwoodCreate)
            {
                return;
            }
            //销毁
            bullet.DestroyBullet();
            //计算触发点位置 生成火焰子弹
            CreateBullet(other.bounds.ClosestPoint(transform.position));
        }
        
    }

    //生成火焰子弹
    private void CreateBullet(Vector3 bornPos)
    {
        GameObject fireBullet = Instantiate(FireBulletPrefab, bornPos,Quaternion.identity);
        fireBullet.transform.parent = transform.parent;
        fireBullet.transform.position = bornPos;
        fireBullet.GetComponent<Bullet>().TorchwoodCreate = true;
    }
}
