using TMPro;
using UnityEngine;

public class Move_Canon : MonoBehaviour
{

    public float moveSpeed = 5f; // Speed at which the object moves toward the target position

    
    public bool isPlaying;

    [SerializeField]
    private GameObject GO_text;


    private void Start()
    {
        isPlaying = true;
        GO_text.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlaying)
        {
            CheckForInput();
        }
        
    }

    private void CheckForInput()
    {
        // Check for touch input (for mobile)
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Process the touch input
            MoveCanon(touch.position);
        }
        // Check for mouse input (for WebGL and desktop platforms)
        else if (Input.GetMouseButton(0)) // Detect left mouse button click
        {
            // Use mouse position as touch position
            Vector2 mousePosition = Input.mousePosition;

            // Process the mouse input
            MoveCanon(mousePosition);
        }
    }

    private void MoveCanon(Vector2 inputPosition)
    {
        // Convert the input position to world space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 0));

        // Update the x position of the game object
        Vector3 newPosition = transform.position;
        newPosition.x = worldPosition.x;

        // Smoothly move to the target position at the specified speed
        transform.position = Vector3.Lerp(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BB"))
        {
            isPlaying = false;

            GO_text.SetActive(true);
        }
    }
}
