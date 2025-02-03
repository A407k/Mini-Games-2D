using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_BlockHolderController : MonoBehaviour
{
    [Header("Activation Settings")]
    [Tooltip("Delay between activating each block (in seconds)")]
    public float activationDelay = 0.5f;

    [Tooltip("The default life number to assign to each block when activated")]
    public int defaultLifeNumber = 5;

    [Header("Score Settings")]
    [Tooltip("Current game score, increased by the block's life value when activated")]
    public int score = 0;



    private void Start()
    {
        ActivateBlocksSequentially();
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

        // Activate each inactive block one by one.
        foreach (GameObject block in blockList)
        {
            if (!block.activeSelf)
            {
                block.SetActive(true);

                // Try to get the BlockData component; add it if it doesn't exist.
                DB_BlockData data = block.GetComponent<DB_BlockData>();
                if (data == null)
                {
                    data = block.AddComponent<DB_BlockData>();
                }

                // Set its life number and add that value to the score.
                data.lifeNumber = defaultLifeNumber;
                score += data.lifeNumber;

                // Optionally, you can also update a UI element here with the new score.

                // Wait for the specified delay before activating the next block.
                yield return new WaitForSeconds(activationDelay);
            }
        }
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
}
