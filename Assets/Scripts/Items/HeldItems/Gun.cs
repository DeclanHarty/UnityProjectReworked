using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Gun : HeldItem
{
    public int damage = 4;
    public int number_pellets = 6;
    public float max_angle_of_spread = 30;
    public float max_distance = 5;

    public float firingSpeed = 1;
    public float timeSinceLastFired = 1;

    public override void Fire1Action()
    {
        if(timeSinceLastFired < firingSpeed){
            return;
        }
        timeSinceLastFired = 0;

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

            RaycastHit2D hit = Physics2D.Raycast(currentPosition, Quaternion.AngleAxis(bullet_angle, Vector3.forward) * mouseDirection, max_distance, LayerMask.GetMask(new string[] {"Ground","Enemy"}));
            if(hit){
                if(hit.collider.gameObject.layer == 8){
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                }
                Debug.DrawLine(currentPosition, (Vector2)(hit.distance * (Quaternion.AngleAxis(bullet_angle, Vector3.forward) * mouseDirection))  + currentPosition, Color.red, .2f);
            }else{
                Debug.DrawLine(currentPosition, (Vector2)(max_distance * (Quaternion.AngleAxis(bullet_angle, Vector3.forward) * mouseDirection))  + currentPosition, Color.red, .2f);
            }
            
        }
    }

    public override void Fire2Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        timeSinceLastFired += Time.deltaTime;
        heldItemController.ammoIndicator.SetAmmoFillIndicator(Mathf.Min(timeSinceLastFired/firingSpeed, 1f));
    }
}
