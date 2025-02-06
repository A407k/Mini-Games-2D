using TMPro;
using UnityEngine;

public class DB_BlockData : MonoBehaviour
{
    /// <summary>
    /// Set block inactive on collision and
    /// asssig block a random sprite
    /// </summary>

    private int lifeNumber; // its used for displaying life and it decreases on collision

    private int startingLife; // its static and is used to update score on inactivation

    [SerializeField]
    private TextMeshProUGUI display_Life; // Text component to Display the life number on Block

    [SerializeField]
    private DB_ScoreHandler scoreHandler_Reference;

    // Reference to the SpriteRenderer component
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite[] sprites; // list of sprites block can chnage in to



    private void Start()
    {
        // Find the TMP component
        //display_Life = GetComponentInChildren<TextMeshProUGUI>();
        if (display_Life == null)
        {
            Debug.LogError("TextMeshProUGUI component is missing!");
           // display_Life = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// this method is public and used by
    /// other scripts to set life of block
    /// </summary>
    /// <param name="lifeNum"></param>
    public void set_blockLife(int lifeNum)
    {
        ChangeSpriteRandomly();

        lifeNumber = lifeNum;
        startingLife = lifeNum;

        UpdateDisplaynumber();

    }

    /// <summary>
    /// Changes the sprite from a random sprite in the list.
    /// </summary>
    public void ChangeSpriteRandomly()
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("No sprites defined in the list.");
            return;
        }

        // Pick a random index from the sprites array
        int randomIndex = Random.Range(0, sprites.Length);
        Sprite randomSprite = sprites[randomIndex];

        // Set the SpriteRenderer's sprite to the randomly chosen sprite
        spriteRenderer.sprite = randomSprite;
    }

    /// <summary>
    /// what happens when block collides with ball
    /// reduce life , update life display
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CB")) // Compare the tag
        {
            // Decrease the Life of BB
            lifeNumber--;

            // Update the diplay number
            UpdateDisplaynumber();

            if (lifeNumber <= 0)
            {
                scoreHandler_Reference.BlockLife_Reference(startingLife);

                gameObject.SetActive(false);
            }

        }
    }

    /// <summary>
    /// simply update the display of life
    /// </summary>
    private void UpdateDisplaynumber()
    {
        if (display_Life != null)
        {
            display_Life.text = lifeNumber.ToString();
        }
    }

}
