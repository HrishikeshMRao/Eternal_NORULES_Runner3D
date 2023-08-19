using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UIDocument mainMenu;
    public Button startButton;
    public SaveData saveData;

    private Button scoreButton;
    private Button homeButton;
    private GroupBox menu;
    private GroupBox hiScores;
    private List<Label> scoreCountText, scoreText;
    private List<int> scores;

    void Start() 
    {
        startButton = mainMenu.rootVisualElement.Q("StartButton") as Button;
        startButton.RegisterCallback<ClickEvent>(HandleStartButtonClick);

        homeButton = mainMenu.rootVisualElement.Q("HomeButton") as Button;
        homeButton.RegisterCallback<ClickEvent>(HandleHomeButtonClick);

        scoreButton = mainMenu.rootVisualElement.Q("ScoreButton") as Button;
        scoreButton.RegisterCallback<ClickEvent>(HandleScoreButtonClick);

        menu = mainMenu.rootVisualElement.Q("Menu") as GroupBox;
        hiScores = mainMenu.rootVisualElement.Q("HiScores") as GroupBox;
        scoreCountText = mainMenu.rootVisualElement.Query<Label>("ScoreCountText").ToList();
        scoreText = mainMenu.rootVisualElement.Query<Label>("ScoreText").ToList();

        hiScores.style.display = DisplayStyle.None;
    }

    void HandleStartButtonClick(ClickEvent evt)
    {
        SceneManager.LoadScene("Level01");
    }

    void HandleScoreButtonClick(ClickEvent evt)
    {
        menu.style.display = DisplayStyle.None;
        hiScores.style.display = DisplayStyle.Flex;

        scores = saveData.Load();

        for(int i=0; i<scores.Count; i++) {
            scoreCountText[i].text = "0" + (i+1) + ".";
            scoreText[i].text = scores[i].ToString();
        }

    }

    void HandleHomeButtonClick(ClickEvent evt) 
    {
        hiScores.style.display = DisplayStyle.None;
        menu.style.display = DisplayStyle.Flex;
    }
}
