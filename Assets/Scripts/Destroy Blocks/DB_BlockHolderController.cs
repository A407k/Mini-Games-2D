using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DB_BlockHolderController : MonoBehaviour
{
    /// <summary>
    /// all main functions happen here
    /// activating blocks
    /// assinging blocks params
    /// </summary>

    [Header("Activation Settings")]
    [Tooltip("Delay between activating each block (in seconds)")]
    public float activationDelay = 0.5f;

    [Tooltip("The default life number to assign to each block when activated")]
    public int defaultLifeNumber = 5;

    List<GameObject> blockList;

    private int minLimit = 1;
    private int maxLimit = 1;

    [SerializeField]
    private Transform ballSpawnPosition;

    [SerializeField]
    private GameObject ballSpawn;

    [SerializeField]
    private DB_GameManager gameManager;

    [Header("Audio Settings")]
    [SerializeField]
    // Reference to the AudioSource component.
    private AudioSource audioSource;
   // private DB_audioPlayer audioPlayer;

    [SerializeField]
    private AudioClip blockSpawnClip;

    [SerializeField]
    private AudioClip levelCompleteClip;


    private void Awake()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        blockList = addBlocksToList();

        ActivateBlocksSequentially();
    }


    private void Update()
    {
       if(AreAllBlocksInactive())
        {
            playAudio(levelCompleteClip);

            ballSpawn.SetActive(false);

            ActivateBlocksSequentially();

        }
    }

    /// <summary>
    /// Call this function to begin activating blocks one by one.
    /// It will only start if not all blocks are active.
    /// </summary>
    public void ActivateBlocksSequentially()
    {
        if (!AreAllBlocksActive())
        {
            StartCoroutine(ActivateBlocksCoroutine());
        }
    }

    /// <summary>
    /// Coroutine that finds all inactive blocks (tag "Block") that are children (or grandchildren) 
    /// of this Holder, then activates them one by one with a delay.
    /// </summary>
    private IEnumerator ActivateBlocksCoroutine()
    {
        // Create a copy of the blockList and shuffle it to get a random order.
        List<GameObject> shuffledBlocks = new List<GameObject>(blockList);
        for (int i = 0; i < shuffledBlocks.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledBlocks.Count);
            // Swap positions
            GameObject temp = shuffledBlocks[i];
            shuffledBlocks[i] = shuffledBlocks[randomIndex];
            shuffledBlocks[randomIndex] = temp;
        }

        // Determine how many blocks to leave inactive.
        // This picks a random number between 5 and 10.
        int leaveInactive = Random.Range(8, 15);  // 5 inclusive, 11 exclusive, so between 5 and 10.

        // Calculate how many blocks should be activated.
        int totalToActivate = Mathf.Max(0, shuffledBlocks.Count - leaveInactive);
        int activatedCount = 0;

        // Loop through the shuffled list and activate blocks until we reach totalToActivate.
        foreach (GameObject block in shuffledBlocks)
        {
            // Only activate if we haven't activated the required number yet.
            if (activatedCount < totalToActivate)
            {
                if (!block.activeSelf)
                {
                    block.SetActive(true);

                    playAudio(blockSpawnClip);

                    // Try to get the DB_BlockData component; add it if it doesn't exist.
                    DB_BlockData data = block.GetComponent<DB_BlockData>();
                    if (data == null)
                    {
                        data = block.AddComponent<DB_BlockData>();
                    }

                    // Set its life number and add that value to the score.
                    data.set_blockLife(GetRandomInt(minLimit, maxLimit));
                   // score += data.get_blockLife(); // Assuming you have a method or property to get the life.

                    // Wait for the specified delay before activating the next block.
                    activatedCount++;
                    yield return new WaitForSeconds(activationDelay);
                }
            }
            else
            {
                // Leave the remaining blocks inactive.
            }
        }

        // Optional: Increase maxLimit, reposition and toggle ballSpawn.
        maxLimit += 2;
      
        
        DB_BallMovement ball = ballSpawn.GetComponent<DB_BallMovement>();
        ball.StartMoving();

        gameManager.updateLifesValue(1); // increase game life by 1

    }


    public void playAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    private List<GameObject> addBlocksToList()
    {
        // Collect all blocks under this Holder that have the tag "Block"
        List<GameObject> blockList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            // If blocks might be nested deeper, you can use a recursive search here.
            foreach (Transform grandChild in child)
            {
                if (grandChild.CompareTag("Block"))
                {
                    blockList.Add(grandChild.gameObject);
                }
            }
        }

        return blockList;
    }

    /// <summary>
    /// Checks all children (and grandchildren) for blocks and returns true if all are active.
    /// </summary>
    private bool AreAllBlocksActive()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild.CompareTag("Block") && !grandChild.gameObject.activeSelf)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool AreAllBlocksInactive()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                // If a block is active, return false.
                if (grandChild.CompareTag("Block") && grandChild.gameObject.activeSelf)
                {
                    return false;
                }
            }
        }

        

        // If we haven't found any active blocks, then all blocks are inactive.
        return true;

       
    }

    // Generates a random integer between min (inclusive) and max (exclusive)
    public int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }


}
