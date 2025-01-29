using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Gun : HeldItem
{
    public int damage = 4;

    public int number_pellets = 6;
    public float max_angle_of_spread = 10;

    public override void Fire1Action()
    {
        for(int i = 0; i < number_pellets; i++ ){
            float bullet_angle;

            if(i == 0){
                bullet_angle = 0;
            }else{
                bullet_angle = Random.Range(1, max_angle_of_spread/2);
                int flip_angle = Random.Range(0,2);
                if(flip_angle == 1){
                    bullet_angle *= -1;
                }
                
            }

            RaycastHit2D hit = Physics2D.Raycast(currentPosition, Quaternion.AngleAxis(bullet_angle, Vector3.forward) * mouseDirection, 10, LayerMask.GetMask(new string[] {"Ground","Enemy"}));
            if(hit){
                if(hit.collider.gameObject.layer == 8){
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                }
                Debug.DrawLine(currentPosition, (Vector2)(hit.distance * (Quaternion.AngleAxis(bullet_angle, Vector3.forward) * mouseDirection))  + currentPosition, Color.red, .2f);

            }
        }
    }

    public override void Fire2Action()
    {
        throw new System.NotImplementedException();
    }
}
