using UnityEngine;

public class DB_BallMovement : MonoBehaviour
{
    public float speed = 5f; // Constant speed of the ball
    private Rigidbody2D rb;  // Reference to the Rigidbody2D component
    private Vector2 velocity; // Ball's velocity vector
    private bool isBallMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = new Vector2(1, 1).normalized * speed; // Initialize with a default direction and speed

        Invoke("startMoving", 2);
    }

    void Update()
    {
        if (isBallMoving)
        {
            // Update the velocity every frame to keep it moving at constant speed
            rb.velocity = velocity;
        }
        
    }

    private void startMoving()
    {
        isBallMoving = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reflect the velocity based on the collision normal
       // if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Paddle"))
       
        
            // Get the normal of the surface that the ball collided with
            Vector2 normal = collision.contacts[0].normal;

            // Reflect the velocity based on the normal
            velocity = Vector2.Reflect(velocity, normal).normalized * speed; // Normalize to keep constant speed
        
    }

}
