using UnityEngine;

public class DB_PositionBlocks : MonoBehaviour
{
    void Start()
    {
        PositionObject();
    }

    private void Update()
    {
        PositionObject(); // later on only in start , call this function
    }

    void PositionObject()
    {
        // Get the Camera's viewport size (normalized screen space)
        Camera cam = Camera.main;

        // Get the screen dimensions in world space (camera's view)
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        // Calculate the world positions for the corners
        Vector3 upperLeft = new Vector3(-screenWidth / 2, screenHeight / 2, 0f); // Top-left corner
        Vector3 upperRight = new Vector3(screenWidth / 2, screenHeight / 2, 0f); // Top-right corner
        Vector3 bottomLeft = new Vector3(-screenWidth / 2, 0f, 0f); // Bottom-left corner
        Vector3 bottomRight = new Vector3(screenWidth / 2, 0f, 0f); // Bottom-right corner

        // Get the midpoint of these corners (center of the square)
        Vector3 center = new Vector3((upperLeft.x + upperRight.x) / 2, (upperLeft.y + bottomLeft.y) / 2, 0f);

        // Set the position of the GameObject to the calculated center
        transform.position = center;

        // Calculate the scale of the GameObject to make it fit between the corners
        float width = screenWidth; // Horizontal distance between left and right
        float height = screenHeight / 2; // Vertical distance between top and middle

        // Adjust the scale of the square to fit in the desired position
        transform.localScale = new Vector3(width, height, 1f);
    }

}
