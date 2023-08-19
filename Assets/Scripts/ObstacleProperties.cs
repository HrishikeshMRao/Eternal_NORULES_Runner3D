using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleProperties : MonoBehaviour
{
    void Update()
    {  
        if((gameObject.transform.position.y < -2f)) {
            Destroy(gameObject);
        }
    }   
}