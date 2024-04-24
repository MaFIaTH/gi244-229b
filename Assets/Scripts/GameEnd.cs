using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private TextMeshProUGUI yourAssetText;
    [SerializeField] private TextMeshProUGUI enemyAssetText;
    // Start is called before the first frame update
    void Start()
    {
        switch (Settings.gameResult)
        {
            case GameResult.Win:
                endText.text = "You Win!";
                break;
            case GameResult.Lose:
                endText.text = "You Lose...";
                break;
            case GameResult.Draw:
                endText.text = "It's a draw...";
                break;
        }
        yourAssetText.text = $"Your Assets:\n{Settings.myAssets}";
        enemyAssetText.text = $"Enemy Assets:\n{Settings.enemyAssets}";
    }

    public void GoToMainMenu()
    {
        // Go to main menu
        SceneManager.LoadScene("MainMenu");
    }
}
