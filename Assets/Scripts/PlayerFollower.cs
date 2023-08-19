
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float rotateAngle = 0;

    private Vector3 rawPosition;

    // Update is called once per frame
    void LateUpdate()
    {
        rawPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        transform.position = rawPosition + offset;
        transform.Rotate(Vector3.right, rotateAngle);
    }
}
