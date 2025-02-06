using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class DB_audioPlayer : MonoBehaviour
{
    /// <summary>
    /// Problem with playing multiple clips at once
    /// </summary>

    // Reference to the AudioSource component.
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();

    }

    public void playAudio(AudioClip clip)
    {
        // simply assign and play the clip 
        audioSource.clip = clip;
        audioSource.Play();
    }

}
