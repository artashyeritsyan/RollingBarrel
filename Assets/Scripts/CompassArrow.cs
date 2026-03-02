using UnityEngine;

public class CompassArrow : MonoBehaviour
{
    public Transform enemy;
    public Camera cam;


    public float edgeOffset = 100f;

    void Update()
    {
        if (enemy == null) return;

        // Convert enemy world position to viewport space
        Vector3 viewportPos = cam.WorldToViewportPoint(enemy.position);

        // Check if enemy is behind camera
        if (viewportPos.z < 0)
        {
            viewportPos *= -1;
        }

        // Convert viewport (0-1) to screen center (-1 to 1)
        Vector2 screenDir = new Vector2(
            viewportPos.x - 0.5f,
            viewportPos.y - 0.5f
        );

        screenDir.Normalize();

        // Rotate arrow
        float angle = Mathf.Atan2(screenDir.y, screenDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Place arrow on screen edge
        float halfWidth = Screen.width / 2f;
        float halfHeight = Screen.height / 2f;

        float x = screenDir.x * (halfWidth - edgeOffset);
        float y = screenDir.y * (halfHeight - edgeOffset);

        transform.position = new Vector3(
            halfWidth + x,
            halfHeight + y,
            0f
        );
    }

    void Start()
    {
        
    }
}
