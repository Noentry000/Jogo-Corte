using UnityEngine;
using TMPro;
using System.Collections;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public GameObject mascot;
    public GameObject panel;
    public int[] stopAfterLines; // line indexes after which the dialogue closes, e.g. 2, 5, 8
    private int index;
    private bool dialogueActive;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (!dialogueActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        dialogueActive = true;
        StartCoroutine(TypeLine());
    }

    void StartDialogue2()
    {
        index = 6;
        StartCoroutine(TypeLine());
    }

    // Call this from your interaction (e.g. when the player presses an object)
    public void ResumeDialogue()
    {
        if (dialogueActive || index >= lines.Length - 1) return;

        mascot.SetActive(true);
        panel.SetActive(true);
        dialogueActive = true;
        index++;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    bool IsStopPoint(int lineIndex)
    {
        foreach (int stop in stopAfterLines)
        {
            if (stop == lineIndex) return true;
        }
        return false;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            if (IsStopPoint(index))
            {
                // Pause: hide the dialogue but keep this script active so it can resume later
                dialogueActive = false;
                mascot.SetActive(false);
                panel.SetActive(false);
                return;
            }

            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            mascot.SetActive(false);
            panel.SetActive(false);
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}