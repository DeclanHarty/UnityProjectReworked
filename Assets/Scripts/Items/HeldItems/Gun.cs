using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : HeldItem
{
    public int damage = 8;

    public override void Fire1Action()
    {
        RaycastHit2D hit = Physics2D.Raycast(currentPosition, mouseDirection, 10, LayerMask.GetMask(new string[] {"Ground","Enemy"}));
        if(hit){
            Debug.Log(hit.collider.gameObject.layer);
            if(hit.collider.gameObject.layer == 8){
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            Debug.DrawLine(currentPosition, hit.distance * mouseDirection + currentPosition, Color.red, .2f);
        }
        

    }

    public override void Fire2Action()
    {
        throw new System.NotImplementedException();
    }
}
