using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform to follow
    public float smoothSpeed = 0.125f; // The speed with which the camera will follow the player
    public float xOffset = 0f; // The horizontal offset between the camera and player

    void LateUpdate()
    {
        // Calculate the desired horizontal position of the camera
        float targetX = target.position.x + xOffset;
        // Smoothly interpolate between the current camera position and the desired horizontal position
        float smoothedX = Mathf.Lerp(transform.position.x, targetX, smoothSpeed * Time.deltaTime);
        // Set the camera's horizontal position to the smoothed horizontal position, maintaining its original y and z coordinates
        transform.position = new Vector3(smoothedX, transform.position.y, transform.position.z);
    }
}
