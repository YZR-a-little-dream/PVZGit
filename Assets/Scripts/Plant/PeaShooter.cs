using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PeaShooter : Plant
{
    public float interval;

    private float timer;

    public GameObject bullet;

    public Transform bulletPos;

    // [Header("基础属性")]
    // public float health = 100;
    // private float currentHealth;

    protected override void Start() {
        base.Start();
    }

    private void Update() {
        checkZombieInRange();
    }

    //通过射线检测来检测僵尸
    public void checkZombieInRange()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,Vector2.right,1000f,
                                1 << LayerMask.NameToLayer("Zombie"));  
        //Debug.Log(hitInfo.collider.gameObject); 
        if(hitInfo)
        {
            if(hitInfo.collider.gameObject.CompareTag("Zombie"))
                {
                    fireBullet();
                }
        }              
                  
    }

    public void fireBullet()
    {
       if(!start)
            {return;}
            timer += Time.deltaTime;

            if (timer > interval) {
            timer = 0;
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
            }     
    }

}
