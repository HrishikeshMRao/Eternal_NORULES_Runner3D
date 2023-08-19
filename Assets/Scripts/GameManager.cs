using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public SaveData saveData;
    public PlayerFollower playerFollower;
    public Animator animator;
    public PlayerControl playerControl;
    public GameObject gameOver;
    public GameObject scoreDisplayer;

    public void GameOver(string gameOverType)
    {
        animator.SetBool(gameOverType, true);

        if(gameOverType == "isCollided") {
            playerControl.isControlEnable = false;
            playerFollower.rotateAngle = 0.15f;
        }

        Invoke("GameOverTransition", 2f); 
    }

    void GameOverTransition()
    {
        gameOver.SetActive(true);
        scoreDisplayer.SetActive(false);
        playerFollower.enabled = false;
        playerControl.isControlEnable = false; //If player fell off of ground, to give 2 seconds delay for player inputs


        //To save the score
        saveData.Save(playerControl.gameObject.transform.position.z);
    }
}
