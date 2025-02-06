using UnityEngine;


[RequireComponent(typeof(EdgeCollider2D))]
public class DB_ScreenEdgeCollider : MonoBehaviour
{
    /// <summary>
    /// places a edge collider to fit the screen size
    /// </summary>

    [SerializeField] private GameObject side_left;
    [SerializeField] private GameObject side_right;
    [SerializeField] private GameObject side_up;
    [SerializeField] private GameObject side_down;


    void Start()
    {
        SetEdgeColliderToScreenBounds();

    }

    private void SetEdgeColliderToScreenBounds()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        // Get the screen corners in world space
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        float left = mainCamera.transform.position.x - screenWidth / 2f;
        float right = mainCamera.transform.position.x + screenWidth / 2f;
        float top = mainCamera.transform.position.y + screenHeight / 2f;
        float bottom = mainCamera.transform.position.y - screenHeight / 2f;


        // Clamp the width so it is never more than 8
        left = Mathf.Max(left, - 4);
        right = Mathf.Min(right, 4);

        // Define the points for the EdgeCollider2D
        Vector2[] edgePoints = new Vector2[5];
        edgePoints[0] = new Vector2(left, bottom);  // Bottom-left
        edgePoints[1] = new Vector2(left, top);     // Top-left
        edgePoints[2] = new Vector2(right, top);    // Top-right
        edgePoints[3] = new Vector2(right, bottom); // Bottom-right
        edgePoints[4] = edgePoints[0];             // Close the loop

        // Set the points to the EdgeCollider2D
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = edgePoints;

        // below here is to place these objects to form a visible boundary
        side_left.transform.position = new Vector2(left, 0);
        side_right.transform.position = new Vector2(right, 0);
        side_up.transform.position = new Vector2(0,top);
        side_down.transform.position = new Vector2(0,bottom);

        side_up.transform.localScale = new Vector3(right - left, 0.05f, 1f);
        side_down.transform.localScale = new Vector3(right - left, 0.05f, 1f);
 

    }



}
