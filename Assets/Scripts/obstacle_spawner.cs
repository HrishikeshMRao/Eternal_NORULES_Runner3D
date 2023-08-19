using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class obstacle_spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> whiteSpace;
    [SerializeField] private GameObject block;
    [SerializeField] private int GoThroughcount = 2;
    [SerializeField] private TrackBuilder pos;
    [SerializeField] private float buffer;
    public Queue<float> gap = new Queue<float>();
    // Update is called once per frame
    public void spawner()
    {
        Vector3 blockScale = block.transform.localScale;
        int count = Random.Range(1, GoThroughcount+1);
        //float chick = Random.Range(1f, 3.5f);
        List<float> width = new();
        List<Vector3> location = new();
        Queue<GameObject> passWays = new Queue<GameObject>();
        float Pass = -10f;
        gap.Enqueue(count);
        for(int i =0; i < count; i++)
        {
            int rand = Random.Range(0, whiteSpace.Count);
            width.Add(whiteSpace[rand].transform.localScale.x);
            float place = Random.Range((Pass + (width[i] / 2) + 2*buffer), (10f - (width[i] / 2) - 2*buffer));
            gap.Enqueue(place);
            location.Add(new Vector3(place, whiteSpace[rand].transform.position.y, 0));
            Pass = location[i].x + (width[i] / 2);
            passWays.Enqueue(whiteSpace[rand]);
        }
        float zPos = pos.newObstaclePos;
        for (int i = 0; i < count; i++)
        {       
            //Debug.Log(location[i]); Debug.Log(i);
            Instantiate(passWays.Dequeue(), (location[i]+new Vector3(0,0,zPos)), Quaternion.identity);                      
        }
        blockScale.Set(((location[0].x - width[0] / 2) + 10f) - buffer, block.transform.localScale.y, block.transform.localScale.z);
        block.transform.localScale = blockScale;
        Vector3 FirstBlock = new Vector3(-10f+buffer, 0.55f, zPos);
        Instantiate(block, (FirstBlock + (block.transform.localScale / 2)), Quaternion.identity);
        if (count > 1)
        {
            for (int i = 0; i < count - 1; i++)
            {
                if (location[i+1].x - location[i].x>buffer)
                {
                    blockScale = block.transform.localScale;
                    blockScale.Set(-(location[i].x + width[i] / 2) + (location[i + 1].x - width[i + 1] / 2) -  buffer, block.transform.localScale.y, block.transform.localScale.z);
                    block.transform.localScale = blockScale;
                    Instantiate(block, (location[i] + (block.transform.localScale / 2) + new Vector3(buffer, 0.55f, zPos)), Quaternion.identity);
                }
                
            }
        }
        if (10f - location[count - 1].x > buffer)
        {
            blockScale = block.transform.localScale;
            blockScale.Set((10f - (location[count - 1].x + width[count - 1] / 2)) - buffer, block.transform.localScale.y, block.transform.localScale.z);
            block.transform.localScale = blockScale;
            Vector3 LastBlock = new Vector3(10f - (block.transform.localScale.x / 2) - buffer, (block.transform.localScale.y / 2) + 0.55f, zPos);
            Instantiate(block, (LastBlock), Quaternion.identity);
        }
    }

    


}
