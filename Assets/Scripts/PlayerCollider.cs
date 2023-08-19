using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public TrackBuilder trackBuilder;
    public PlayerControl playerControl;
    public GameManager gameManager;
    private Vector3 hitDirection;
    private int prevPlatformHit = 0;
    
    private bool isGameOver = false;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if(body == null) return;

        if(prevPlatformHit == hit.collider.GetInstanceID()) return;


        if(hit.collider.tag == "Obstacle") {
            hitDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = hitDirection * playerControl.controller.velocity.z / 10;

            if(!isGameOver) {
                gameManager.GameOver("isCollided");
                isGameOver = true;
            }
        }

        if(hit.collider.tag == "Platform") {
            trackBuilder.TrackBuild();
            prevPlatformHit = hit.collider.GetInstanceID();
        }
    }
}
