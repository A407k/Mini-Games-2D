using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DB_BallMovement : MonoBehaviour
{

    /// <summary>
    /// Script simply moves the ball and
    /// plays a sound on collision
    /// </summary>

    [Header("Movement Settings")]
    [Tooltip("The speed at which the ball moves.")]
    [SerializeField] private float speed = 250f;

    [SerializeField]
    private Transform ballSpawnPosition; // where thw ball will be at starting of game

    public Rigidbody2D rb; // its public cause other scripts access it to change values


    [Header("Audio Settings")]
    [SerializeField]
    // Reference to the AudioSource component.
    private AudioSource audioSource;
    //private DB_audioPlayer audioPlayer;

    [SerializeField]
    private AudioClip ballCollidingClip;

    [SerializeField]
    private AudioClip ballSpawnClip;

    private void Awake()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();

        // Cache the Rigidbody2D component.
        rb = GetComponent<Rigidbody2D>();

        // Optional: ensure no drag slows the ball down.
        rb.drag = 0f;
        rb.angularDrag = 0f;
    }

    public void StartMoving()
    {
        // position the ball and call the function to move the ball
        transform.position = ballSpawnPosition.position;
        gameObject.SetActive(true);       

        Invoke(nameof(move), 2);

    }   

    private void move()
    {
        playAudio(ballSpawnClip); // play a clip on ball Spawn

        // Generate a random horizontal component between -1 and 1, and a fixed downward component.
        Vector2 forceDirection = new Vector2(Random.Range(-1f, 1f), -1f).normalized;

        // Apply the force using Impulse so the ball starts moving immediately.
        rb.AddForce(forceDirection * speed, ForceMode2D.Impulse);
    }

    public void playAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void FixedUpdate()
    {
        // If the ball is moving, normalize the velocity so it always has the exact speed.
        if (rb.velocity != Vector2.zero)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playAudio (ballCollidingClip);
    }


}
