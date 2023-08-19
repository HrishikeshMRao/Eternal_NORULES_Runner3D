using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    public UIDocument gameOverScreen;
    public Label finalScoreText;
    public PlayerControl playerControl;
    public Button retry;
    public Button home;
    void Start()
    {
        retry = gameOverScreen.rootVisualElement.Q<Button>("RetryButton");
        retry.RegisterCallback<ClickEvent>(HandleRetryButtonClick);

        home = gameOverScreen.rootVisualElement.Q<Button>("HomeButton");
        home.RegisterCallback<ClickEvent>(HandleHomeButtonClick);

        finalScoreText = gameOverScreen.rootVisualElement.Q<Label>("FinalScoreText");
        finalScoreText.text = "Final Score : " + (int)playerControl.gameObject.transform.position.z;
    }

    void HandleRetryButtonClick(ClickEvent evt)
    {
        SceneManager.LoadScene("Level01");
    }

    void HandleHomeButtonClick(ClickEvent evt) 
    {
        SceneManager.LoadScene("Home");
    }
}
