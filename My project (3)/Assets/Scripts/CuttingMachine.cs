using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CuttingMachine : MonoBehaviour
{
    [Header("Score")]
    public float score = 1000f;
    public float penaltyPerSecond = 5f;

    [Header("UI")]
    public Image efficiencyBar;
    public Image progressBar;
    public GameObject victoryPanel;
    public TMP_Text efficiencyText;
    public GameObject defeatPanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cutSound;
    [Range(0f, 1f)] public float cutVolume = 1f;
    public AudioClip victorySound;
    [Range(0f, 1f)] public float victoryVolume = 1f;
    public AudioClip defeatSound;
    [Range(0f, 1f)] public float defeatVolume = 1f;

    private HashSet<LineSegment> touchingSegments = new HashSet<LineSegment>();
    private LineSegment[] allSegments;
    private bool victory = false;
    private bool defeat = false;
    private float maxScore;

    void Start()
    {
        maxScore = score;

        allSegments = FindObjectsOfType<LineSegment>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }

        UpdateScore();
        UpdateProgress();
    }

    void Update()
    {
        if (victory || defeat)
            return;

        if (!Input.GetMouseButton(0))
        {
            StopCutSound();
            UpdateProgress();
            CheckVictory();
            return;
        }

        StartCutSound();

        if (touchingSegments.Count > 0)
        {
            foreach (LineSegment segment in touchingSegments)
            {
                segment.Cut();
            }
        }
        else
        {
            score -= penaltyPerSecond * Time.deltaTime;
            score = Mathf.Max(score, 0f);
        }

        UpdateScore();
        UpdateProgress();

        if (score <= 0f)
        {
            Defeat();
            return;
        }

        CheckVictory();
    }

    void StartCutSound()
    {
        if (audioSource == null || cutSound == null)
            return;

        if (audioSource.isPlaying && audioSource.clip == cutSound)
            return;

        audioSource.clip = cutSound;
        audioSource.loop = true;
        audioSource.volume = cutVolume;
        audioSource.Play();
    }

    void StopCutSound()
    {
        if (audioSource == null)
            return;

        if (audioSource.isPlaying && audioSource.clip == cutSound)
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Segment"))
        {
            LineSegment segment = other.GetComponent<LineSegment>();

            if (segment != null)
            {
                touchingSegments.Add(segment);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Segment"))
        {
            LineSegment segment = other.GetComponent<LineSegment>();

            if (segment != null)
            {
                touchingSegments.Remove(segment);
            }
        }
    }

    void UpdateScore()
    {
        if (efficiencyBar != null)
        {
            efficiencyBar.fillAmount = score / maxScore;
        }
    }

    void UpdateProgress()
    {
        if (progressBar == null || allSegments.Length == 0)
            return;

        int cutSegments = 0;

        foreach (LineSegment segment in allSegments)
        {
            if (segment.isCut)
            {
                cutSegments++;
            }
        }

        progressBar.fillAmount = (float)cutSegments / allSegments.Length;
    }

    void CheckVictory()
    {
        if (allSegments.Length == 0)
            return;

        int cutSegments = 0;

        foreach (LineSegment segment in allSegments)
        {
            if (segment.isCut)
            {
                cutSegments++;
            }
        }

        float progress = (float)cutSegments / allSegments.Length * 100f;

        if (progress >= 100f)
        {
            Victory();
        }
    }

    void Victory()
    {
        victory = true;

        StopCutSound();

        if (audioSource != null && victorySound != null)
        {
            audioSource.PlayOneShot(victorySound, victoryVolume);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (efficiencyText != null)
        {
            efficiencyText.text = "EFICIENCIA: " + Mathf.RoundToInt(score) + "%";
        }
    }

    void Defeat()
    {
        defeat = true;

        StopCutSound();

        if (audioSource != null && defeatSound != null)
        {
            audioSource.PlayOneShot(defeatSound, defeatVolume);
        }

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
    }
}