using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class ScreenEdgeCollider : MonoBehaviour
{
    [Tooltip("Offset from the bottom edge in world units")]
    [SerializeField] private float bottomOffset = 0.5f;

    [Tooltip("Vertical offset for the middle point of the bottom edge")]
    [SerializeField] private float middlePointRaise = 0.3f;

    private void Start()
    {
        SetEdgeCollider();
    }

    private void SetEdgeCollider()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        // Get screen bounds in world space
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        float left = mainCamera.transform.position.x - screenWidth / 2f;
        float right = mainCamera.transform.position.x + screenWidth / 2f;
        float top = mainCamera.transform.position.y + screenHeight / 2f;
        float bottom = mainCamera.transform.position.y - screenHeight / 2f;

        // Define points for the EdgeCollider2D
        Vector2[] edgePoints = new Vector2[6];
        edgePoints[0] = new Vector2(left, bottom + bottomOffset);              // Bottom-left
        edgePoints[1] = new Vector2(left, top);                               // Top-left
        edgePoints[2] = new Vector2(right, top);                              // Top-right
        edgePoints[3] = new Vector2(right, bottom + bottomOffset);            // Bottom-right
        edgePoints[4] = new Vector2(0f, bottom + bottomOffset + middlePointRaise); // Middle raised point
        edgePoints[5] = edgePoints[0];                                        // Close the loop

        // Assign points to the EdgeCollider2D
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = edgePoints;
    }

}
