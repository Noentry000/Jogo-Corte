using UnityEngine;

public class LineSegment : MonoBehaviour
{
    public Material greenMaterial;

    private Renderer lineRenderer;
    private Material originalMaterial;

    public bool isCut = false;

    void Start()
    {
        lineRenderer = GetComponent<Renderer>();

        originalMaterial = lineRenderer.material;
    }

    public void Cut()
    {
        if (isCut)
            return;

        isCut = true;

        lineRenderer.material = greenMaterial;
    }
}