using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float damge = 15.0f;

    private void Start() {
        GameObject.Destroy(gameObject,10);
    }

    private void Update() {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Zombie")
        {
            GameObject.Destroy(gameObject);
            //僵尸受击
            other.GetComponent<ZombieNormal>().ChangeHealth(-damge);
        }
    }
}
