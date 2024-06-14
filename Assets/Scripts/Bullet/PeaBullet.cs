using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;


public class PeaBullet : Bullet
{
    //豌豆射手的打击感
    public override void DestroyBullet()
    {
        Camera.main.transform.DOShakePosition(2,3);
        base.DestroyBullet();
        SoundManager.Instance.PlaySound(Globals.S_PeaHit);
    }
}
