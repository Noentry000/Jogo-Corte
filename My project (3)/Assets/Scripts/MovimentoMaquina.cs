using UnityEngine;

public class MovimentoMaquina : MonoBehaviour
{
    public float forwardSpeed = 50f;
    public float rotationSpeed = 100f;

    void Update()
    {
        // Get mouse scroll input for forward/backward movement
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Get A/D input for rotation
        float horizontal = Input.GetAxis("Horizontal");

        // Move forward/backward
        Vector3 movement = transform.forward * scroll * forwardSpeed;
        transform.position += movement * Time.deltaTime;

        // Rotate left/right
        float rotation = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
    }
}