using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;
using System;

public class TrackBuilder : MonoBehaviour
{
    public GameObject platform, platformTrack;
    //public ObstacleProperties[] obstacleproperties;
    public Queue<GameObject> platformArray = new();
    //[SerializeField] private GameObject coin;
    [SerializeField] private obstacle_spawner spawn;
    [SerializeField] PlayerControl speed;
    [SerializeField] private float scaleFactor = 1f;
    [SerializeField] private GameObject coin;
    [SerializeField] private float interval = 1f;
    public float newObstaclePos = 100f, newPlatformPos = 100f;
    private Vector3 newPlatformPosVector,coinVector;
    private GameObject newPlatform;
    public List<float> previousGap = new();
    public List<float> newGap = new();
    public GameObject[] initialPlatform = new GameObject[2];
    private float obstaclegap;

    public void TrackBuild()
    {
        Debug.Log("new platform");
        // Building new Platform
        newPlatformPosVector = new Vector3(0, 0, newPlatformPos);
        newPlatform = Instantiate(platform, newPlatformPosVector, platform.transform.rotation);
        newPlatform.transform.SetParent(platformTrack.transform);
        platformArray.Enqueue(newPlatform);

        if (platformArray.Count > 4)
        {
            Destroy(platformArray.Dequeue());
        }

        newPlatformPos += 50f;

        // Building Obstacles on the platform
        do
        {
            //obstacletId = Random.Range(0, obstacles.Length); // To choosing Random obstacle
            //newObstaclePosVector = new Vector3(0, 0, newObstaclePos);
            //Instantiate(obstacles, newObstaclePosVector, obstacles.transform.rotation);
            spawn.spawner();

            //change
            newObstaclePos += obstacleOffset() - 1;
            //Debug.Log(newObstaclePos);
        }
        while (newObstaclePos < newPlatformPos - 25);
    }

    private float obstacleOffset()
    {
        float largest = 0;
        float count;
        Debug.Log(spawn.gap.Count);
        if (previousGap.Count < 1)
        {
            if (spawn.gap.Count != 0)
            {
                count = spawn.gap.Dequeue();
                for (int i = 0; i < count; i++)
                {
                    previousGap.Add(spawn.gap.Dequeue());
                }
            }
            return (30f);
        }
        if (spawn.gap.Count != 0)
        {
            count = spawn.gap.Dequeue();
            for (int i = 0; i < count; i++)
            {
                newGap.Add(spawn.gap.Dequeue());
                for (int j = 0; j < previousGap.Count; j++)
                {
                    coinSpawner(i,j);
                    if ((newGap[i] - previousGap[j]) > largest)
                    {
                        largest = newGap[i] - previousGap[j];
                    }
                }
            }
            previousGap = newGap;
        }
        newGap = new();
        obstaclegap = (30f+largest * speed.forwardAcceleration * scaleFactor);
        return (obstaclegap);
    }

    private void coinSpawner(int i,int j)
    {
        float gap = obstaclegap;
        float CoinPosZ = newObstaclePos-gap;
        
        float CoinPosX = previousGap[j];
        float nextX = newGap[i];
        float lastX = previousGap[j];
        if (nextX >= CoinPosX)
        {
            while (((newObstaclePos - CoinPosZ) >= 0) || ((nextX - CoinPosX) >= 0))
            {
                coinVector = new Vector3(CoinPosX, 1.55f, CoinPosZ);
                Instantiate(coin, coinVector, Quaternion.identity);
                CoinPosZ += interval*4;
                //if(nextX!=lastX)
                //CoinPosZ += gap / (interval * (nextX - lastX));
                if (CoinPosX <= nextX)
                CoinPosX += interval;
            }
        }
        else
        {
            while (((newObstaclePos - CoinPosZ) >= 0) || ((CoinPosX - nextX) >= 0))
            {
                coinVector = new Vector3(CoinPosX, 1.55f, CoinPosZ);
                Instantiate(coin, coinVector, Quaternion.identity);
                CoinPosZ += interval*4;
                //if(nextX!=lastX)
                //CoinPosZ += gap / (interval * (nextX - lastX));
                if (CoinPosX >= nextX)
                CoinPosX -= interval;
            }
        }
    }

    void Start()
    {
        // TrackBuild();
        platformArray.Enqueue(initialPlatform[0]);
        platformArray.Enqueue(initialPlatform[1]);
    }

}
