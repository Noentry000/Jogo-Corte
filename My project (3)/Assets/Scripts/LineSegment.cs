using UnityEngine;

public class LineSegment : MonoBehaviour
{
    public Material blackMaterial;
    public Material greenMaterial;

    private Renderer lineRenderer;

    public bool isCut = false;

    void Start()
    {
        lineRenderer = GetComponent<Renderer>();

        isCut = false;
        lineRenderer.material = blackMaterial;
    }

    public void Cut()
    {
        if (isCut)
            return;

        isCut = true;
        lineRenderer.material = greenMaterial;
    }
}