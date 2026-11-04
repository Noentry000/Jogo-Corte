using UnityEngine;

public class MovimentoMaquina : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float rotationSpeed = 100f;

    [Header("Walkable Surface")]
    public string walkableTag = "Ground";
    public float rayStartHeight = 1f;
    public float rayDistance = 5f;

    void Update()
    {
        // Get W/S input for forward/backward movement
        float vertical = Input.GetAxis("Vertical");

        // Get A/D input for rotation
        float horizontal = Input.GetAxis("Horizontal");

        // Move forward/backward (only if there is walkable ground at the new position)
        Vector3 movement = transform.forward * vertical * forwardSpeed;
        Vector3 newPosition = transform.position + movement * Time.deltaTime;

        if (IsOverWalkableSurface(newPosition))
        {
            transform.position = newPosition;
        }

        // Rotate left/right
        float rotation = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
    }

    bool IsOverWalkableSurface(Vector3 position)
    {
        Vector3 origin = position + Vector3.up * rayStartHeight;

        // RaycastAll so the player's own collider (if it has one) doesn't block the check
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, rayDistance, ~0, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag(walkableTag))
            {
                return true;
            }
        }

        return false;
    }
}