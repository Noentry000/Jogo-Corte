using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CuttingMachine : MonoBehaviour
{
    [Header("Score")]
    public float score = 1000f;
    public float penaltyPerSecond = 5f;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text progressText;
    public GameObject victoryPanel;
    public TMP_Text efficiencyText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cutSound;
    [Range(0f, 1f)] public float cutVolume = 1f;
    public AudioClip victorySound;
    [Range(0f, 1f)] public float victoryVolume = 1f;

    private HashSet<LineSegment> touchingSegments = new HashSet<LineSegment>();
    private LineSegment[] allSegments;
    private bool victory = false;

    void Start()
    {
        allSegments = FindObjectsOfType<LineSegment>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        UpdateScore();
        UpdateProgress();
    }

    void Update()
    {
        if (victory)
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
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.RoundToInt(score);
        }
    }

    void UpdateProgress()
    {
        if (progressText == null || allSegments.Length == 0)
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

        progressText.text = "Progress: " + Mathf.RoundToInt(progress) + "%";
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
}