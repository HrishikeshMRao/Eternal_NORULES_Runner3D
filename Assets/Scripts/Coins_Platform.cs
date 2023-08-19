using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins_Platform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float xRotationSpeed; //inpput from ui
    [SerializeField] private float yRotationSpeed;
    [SerializeField] private float zRotationSpeed;
    private MeshCollider detect; // collider of the coin
    void Start()
    {
        detect = GetComponent<MeshCollider>();
        transform.rotation = Quaternion.identity;//Get the initial pose of the coin
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xRotationSpeed, yRotationSpeed, zRotationSpeed); //rotate the coin by the given amount in degres each frame
    }
    private void OnTriggerEnter(Collider other) //interrupt when collider is triggerd
    {
        if (other.tag == "player")
        {
            Destroy(gameObject);
        }
    }
}
