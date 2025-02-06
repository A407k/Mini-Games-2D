using TMPro;
using UnityEngine;

public class DB_GameManager : MonoBehaviour
{
    /// <summary>
    /// main game over things happen here
    /// </summary>
    

    [SerializeField]
    private TextMeshProUGUI text_Lifes;

    [SerializeField]
    private GameObject text_gameOver;

    [SerializeField]
    private GameObject ballSpawn;

    private int Lifes = 2; // starting lifes


    [Header("Audio Settings")]
    [SerializeField]
    private DB_audioPlayer audioPlayer;

    [SerializeField]
    private AudioClip gameOverClip;

    private void Start()
    {
        updateLifesDisplay();
    }

    private void updateLifesDisplay()
    {
        if (text_Lifes != null)
        {
            text_Lifes.text = Lifes.ToString();
        }
    }

    /// <summary>
    /// public cause other scripts call it update life values
    /// </summary>
    /// <param name="i"></param>
    public void updateLifesValue(int i)
    {
        Lifes += i;

        updateLifesDisplay();
        callToBall();
    }

    private void callToBall()
    {
        DB_BallMovement ball = ballSpawn.GetComponent<DB_BallMovement>();
        ball.StartMoving();
    }

    /// <summary>
    /// main game over function
    /// </summary>
    private void Update()
    {
        if(Lifes <= 0)
        {
            audioPlayer.playAudio(gameOverClip);

            text_gameOver.SetActive(true);

            ballSpawn.SetActive(false);

        }
    }


    /// <summary>
    /// public cause a button in game calls it
    /// it call a method to reset the ball
    /// </summary>
    public void getNewBall()
    {
        if (Lifes > 0)
        {
            ballSpawn.SetActive(false) ;

            callToBall();
        }
    }

}
