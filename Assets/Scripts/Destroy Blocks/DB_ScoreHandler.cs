using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DB_ScoreHandler : MonoBehaviour
{
    /// <summary>
    /// it update the score isply and also
    /// svaes the highscore locally
    /// </summary>

    [SerializeField]
    private TextMeshProUGUI Text_Score;

    [SerializeField]
    private TextMeshProUGUI Text_HighScore;

    private int Score = 0; // to store the score

    private int highScore; // to store the HighScore

    private const string HighScoreKey = "DB_HighScore";


    private void Start()
    {
        // Load HighScore when the scene starts
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        // initial score display when scene starts
        updateScore();

    }

    public void BlockLife_Reference(int life)
    {
        Score += life;

        updateScore();
    }

    private void updateScore()
    {
        // display score
        if (Text_Score != null)
        {
            Text_Score.text = Score.ToString();
        }

        // update highscore in script
        updateHighScore();

        // display highscore
        if (Text_HighScore != null)
        {
            Text_HighScore.text = highScore.ToString();
        }

        

    }

    private void updateHighScore()
    {
        if (Score > highScore)
        {
            highScore = Score;

            // save highscore in persistant storage
            SaveHighScore();
        }
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
    }



}
