using TMPro;
using UnityEngine;

public class DB_gameOver : MonoBehaviour
{
    /// <summary>
    /// it simply calls to update life value
    /// when ball collides with screen bottom
    /// 
    /// </summary>
    

    [SerializeField]
    private DB_GameManager gameManager;

    [SerializeField]
    private DB_audioPlayer audioPlayer;

    [SerializeField]
    private AudioClip ballDestroyClip;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CB"))
        {
            audioPlayer.playAudio(ballDestroyClip);

            collision.gameObject.SetActive(false);

            gameManager.updateLifesValue(-1);

        }
    }
}
