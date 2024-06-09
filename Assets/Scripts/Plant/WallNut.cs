using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class WallNut : Plant
{
    
    protected  override void  Start()
    {
        base.Start();
        //TODO: 临时测试用，启动坚果  
    }

    private void Update() {
        if(!start)
            {return;}
    }

    public override float ChangeHealth(float num)
    {
        float currentHealth = base.ChangeHealth(num);
        //修改动画参数:BloodPercent，扣血时出发对应的动画
        animator.SetFloat("BloodPercent", currentHealth / health);
        return currentHealth;
    }
}
