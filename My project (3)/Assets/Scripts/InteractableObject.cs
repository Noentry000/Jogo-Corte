using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Dialogue dialogue; // drag the object with your Dialogue script here
    public int correctForStop; // 0 = HEADSET, 1 = GLASSES, 2 = MASK, 3 = GLOVES

    void OnMouseDown()
    {
        if (!dialogue.WaitingForChoice) return;

        if (dialogue.CurrentStop == correctForStop)
        {
            Debug.Log("Correct!");
            gameObject.SetActive(false); // becomes invisible
            dialogue.ResumeDialogue();
        }
        else
        {
            Debug.Log("Wrong object");
            // add feedback here later (sound, shake, etc.)
        }
    }
}