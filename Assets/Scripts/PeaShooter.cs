using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PeaShooter : MonoBehaviour
{
    public float interval;

    private float timer;

    public GameObject bullet;

    public Transform bulletPos;

    [Header("基础属性")]
    public float health = 100;
    private float currentHealth;

    private void Start() {
        currentHealth = health;
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer > interval) {
            timer = 0;
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
        }
    }

    public float ChangeHealth(float num)
    {
        currentHealth  = Mathf.Clamp(currentHealth + num , 0 ,health );
        if(currentHealth <= 0)
        {
            GameObject.Destroy(gameObject);
        }
        return currentHealth;
    }
}
