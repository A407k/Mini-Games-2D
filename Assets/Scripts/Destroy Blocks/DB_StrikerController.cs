using Unity.Mathematics;
using UnityEngine;

public class DB_StrikerController : MonoBehaviour
{
    /// <summary>
    /// it handles the striker/ paddle movements and
    /// also decides the reflection of the ball
    /// </summary>

    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed at which the striker moves toward the target position

    public float maxbounceAngle = 75f;

    [Header("Vertical Bounds (Viewport)")]
    [Tooltip("The lower margin in viewport space (0 = bottom, 1 = top). E.g., 0.1 puts the lower bound at 10% up the screen.")]
    [SerializeField] private float bottomViewportMargin = 0.1f;

    [Tooltip("The upper margin in viewport space. E.g., 0.45 puts the upper bound slightly below the center (0.5).")]
    [SerializeField] private float topViewportMargin = 0.45f;

    [Header("Horizontal Clamp")]
    [Tooltip("Maximum allowed horizontal distance in world units (both left and right).")]
    [SerializeField] private float maxHorizontalClamp = 3.4f;

    void FixedUpdate()
    {
        CheckForInput();
    }

    private void CheckForInput()
    {
        // Check for touch input (mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            MoveStriker(touch.position);
        }
        // Check for mouse input (WebGL/desktop)
        else if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            MoveStriker(mousePosition);
        }
    }

    private void MoveStriker(Vector2 inputPosition)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Calculate the distance from the camera to the striker (assuming striker is at z = 0)
        float distance = -cam.transform.position.z;

        // Convert the input (screen) position to world space using the determined distance.
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, distance));

        // Get the horizontal bounds from the viewport.
        // Left and right bounds in world space at the vertical center (y = 0.5) of the viewport.
        Vector3 leftPoint = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, distance));
        Vector3 rightPoint = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, distance));

        // Clamp horizontal movement: Use screen bounds but further limit them to ±maxHorizontalClamp.
        float xMin = Mathf.Max(leftPoint.x, -maxHorizontalClamp);
        float xMax = Mathf.Min(rightPoint.x, maxHorizontalClamp);

        // Get vertical bounds using viewport margins.
        Vector3 lowerPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, bottomViewportMargin, distance));
        Vector3 upperPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, topViewportMargin, distance));
        float yMin = lowerPoint.y;
        float yMax = upperPoint.y;

        // Clamp the desired world position to the computed boundaries.
        float clampedX = Mathf.Clamp(worldPos.x, xMin, xMax);
        float clampedY = Mathf.Clamp(worldPos.y, yMin, yMax);

        Vector3 targetPosition = new Vector3(clampedX, clampedY, transform.position.z);

        // Smoothly move the striker toward the target position.
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }


    /// <summary>
    /// this method decide how the ball will
    /// reflect depending on wheere it collides 
    /// on striker
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DB_BallMovement ball = collision.gameObject.GetComponent<DB_BallMovement>();

        if (ball != null)
        {
            Vector3 strikerPosition = this.transform.position;
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = strikerPosition.x - contactPoint.x;
            float width = collision.otherCollider.bounds.size.x / 2;

            float curentAngle = Vector2.SignedAngle(Vector2.up, ball.rb.velocity);
            float bounceAngle = (offset / width) * this.maxbounceAngle;
            float newAngle = Mathf.Clamp(curentAngle + bounceAngle, -this.maxbounceAngle, this.maxbounceAngle);

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            ball.rb.velocity = rotation * Vector2.up * ball.rb.velocity.magnitude;
        }

    }

}

