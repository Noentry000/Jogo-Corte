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

    private HashSet<LineSegment> touchingSegments = new HashSet<LineSegment>();

    void Update()
    {
        // Do NOTHING unless M1 is held
        if (!Input.GetMouseButton(0))
            return;

        // M1 is held and we're touching the line
        if (touchingSegments.Count > 0)
        {
            foreach (LineSegment segment in touchingSegments)
            {
                segment.Cut();
            }
        }
        else
        {
            // M1 is held but we're off the line
            score -= penaltyPerSecond * Time.deltaTime;
            score = Mathf.Max(score, 0f);
        }

        UpdateScore();
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
}