using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Dialogue dialogue; // drag the object with your Dialogue script here
    public int correctForStop; // 0 = HEADSET, 1 = GLASSES, 2 = MASK, 3 = GLOVES
    public AudioClip correctSound;
    public AudioClip wrongSound;
    [Range(0f, 1f)] public float soundVolume = 1f;

    [Header("Arrow")]
    public GameObject arrow; // child object with a SpriteRenderer
    public float bobHeight = 0.1f;
    public float bobSpeed = 4f;
    public Vector3 arrowWorldRotation = Vector3.zero; // fixed rotation in world space (euler angles)
    public bool faceCameraHorizontally = false; // turns the arrow around Y to face the camera, keeps it upright

    private Vector3 arrowWorldOffset;

    void Start()
    {
        if (arrow != null)
        {
            // remember where the arrow sits relative to the object, in world space
            arrowWorldOffset = arrow.transform.position - transform.position;
            arrow.SetActive(false);
        }
    }

    void Update()
    {
        if (arrow == null) return;

        bool show = dialogue.WaitingForChoice && dialogue.CurrentStop == correctForStop;
        arrow.SetActive(show);

        if (!show) return;

        // bounce straight up and down in world space
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        arrow.transform.position = transform.position + arrowWorldOffset + Vector3.up * offset;

        // rotation in world space, ignoring the parent's rotation
        if (faceCameraHorizontally && Camera.main != null)
        {
            float camY = Camera.main.transform.eulerAngles.y;
            arrow.transform.rotation = Quaternion.Euler(arrowWorldRotation.x, camY + arrowWorldRotation.y, arrowWorldRotation.z);
        }
        else
        {
            arrow.transform.rotation = Quaternion.Euler(arrowWorldRotation);
        }
    }

    void OnMouseDown()
    {
        if (!dialogue.WaitingForChoice) return;

        if (dialogue.CurrentStop == correctForStop)
        {
            Debug.Log("Correct!");
            AudioSource.PlayClipAtPoint(correctSound, Camera.main.transform.position, soundVolume);
            gameObject.SetActive(false); // becomes invisible
            dialogue.ResumeDialogue();
        }
        else
        {
            Debug.Log("Wrong object");
            AudioSource.PlayClipAtPoint(wrongSound, Camera.main.transform.position, soundVolume);
            // add feedback here later (sound, shake, etc.)
        }
    }
}