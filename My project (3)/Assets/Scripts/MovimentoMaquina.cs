using UnityEngine;

public class MovimentoMaquina : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float rotationSpeed = 100f;

    void Update()
    {
        // Get W/S input for forward/backward movement
        float vertical = Input.GetAxis("Vertical");

        // Get A/D input for rotation
        float horizontal = Input.GetAxis("Horizontal");

        // Move forward/backward
        Vector3 movement = transform.forward * vertical * forwardSpeed;
        transform.position += movement * Time.deltaTime;

        // Rotate left/right
        float rotation = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
    }
}