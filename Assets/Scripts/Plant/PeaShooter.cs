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
        if(!start)
            {return;}

        timer += Time.deltaTime;

        if (timer > interval) {
            timer = 0;
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
        }
    }

    
}
