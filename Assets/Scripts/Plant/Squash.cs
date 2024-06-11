using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum State{
    None,Down,Up,Over
}

public class Squash : Plant
{  
    //采用射线检测来检测僵尸
    //记录被攻击目标的位置(僵尸位置)
    private Vector3 attackPos;
    //当前状态参数
    private State squashState;
    //窝瓜看向的方位
    string LookName;
    //窝瓜造成的伤害
    public int damage;
    protected override void Start() {
        base.Start();

        attackPos = Vector3.zero;
        squashState = State.None;
        LookName=null;

        //boxCollider2D.enabled = true;
    }
    
    private void Update() {

        checkZombieInRange();
        switchState(squashState);
    }

    private void switchState(State state)
    {
        switch(state)
        {
            case State.Down:
                break;
            case State.Up:
                transform.position = Vector2.MoveTowards(transform.position
                    ,new Vector2(attackPos.x,attackPos.y + 130),Time.deltaTime * 200);
                break;
            case State.Over:
                transform.position = Vector2.MoveTowards(transform.position,attackPos,Time.deltaTime * 200);
                break;
            default:
                break;
        }
    }

    public void checkZombieInRange()
    {
        //当检测到攻击位置时退出检测
        if(attackPos != Vector3.zero)
            return;

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position,Vector2.right,110f,
                                1 << LayerMask.NameToLayer("Zombie"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position,Vector2.left,110f,
                                1 << LayerMask.NameToLayer("Zombie"));                    
              
        if(hitRight.collider !=null )
        {
            attackPos = hitRight.collider.gameObject.transform.position;
            LookName = "Right";
        }
        if(hitLeft.collider != null)
        {
            attackPos = hitLeft.collider.gameObject.transform.position;
            LookName="Left";
        }
        if(LookName != null)
        {
            animator.SetTrigger(LookName);
            animator.SetTrigger("Attack");
        }
            
    }

    public void AttackUp()
    {
        squashState = State.Up;
        animator.SetTrigger("AttackUp");
    }

    public void AttackOver()
    {
        squashState = State.Over;
        animator.SetTrigger("AttackOver");
    }

    public void DoReallyAttack()
    {
        Destroy(gameObject,0.5f);
    }

    public override float ChangeHealth(float num)
    {
        return health;
    }

    //对僵尸照成伤害
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Zombie"))
      {
        other.GetComponent<ZombieNormal>().ChangeHealth(-damage);
      } 
    }
    
}
