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

    private HashSet<LineSegment> touchingSegments = new HashSet<LineSegment>();
    private LineSegment[] allSegments;

    void Start()
    {
        allSegments = FindObjectsOfType<LineSegment>();

        UpdateScore();
        UpdateProgress();
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            UpdateProgress();
            return;
        }

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
}