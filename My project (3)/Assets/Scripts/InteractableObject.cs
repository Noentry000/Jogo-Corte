using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Dialogue dialogue; // drag the object with your Dialogue script here
    public int correctForStop; // 0 = HEADSET, 1 = GLASSES, 2 = MASK, 3 = GLOVES
    public AudioClip correctSound; // NEW
    public AudioClip wrongSound; // NEW
    [Range(0f, 1f)] public float soundVolume = 1f; // NEW

    void OnMouseDown()
    {
        if (!dialogue.WaitingForChoice) return;

        if (dialogue.CurrentStop == correctForStop)
        {
            Debug.Log("Correct!");
            AudioSource.PlayClipAtPoint(correctSound, Camera.main.transform.position, soundVolume); // NEW
            gameObject.SetActive(false); // becomes invisible
            dialogue.ResumeDialogue();
        }
        else
        {
            Debug.Log("Wrong object");
            AudioSource.PlayClipAtPoint(wrongSound, Camera.main.transform.position, soundVolume); // NEW
            // add feedback here later (sound, shake, etc.)
        }
    }
}