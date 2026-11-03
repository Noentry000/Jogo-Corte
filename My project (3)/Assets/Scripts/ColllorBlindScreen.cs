using System.Collections;
using UnityEngine;

public class ColorblindScreen : MonoBehaviour
{
    public enum Mode { Normal = 0, Protanopia = 1, Deuteranopia = 2, Tritanopia = 3 }

    public static ColorblindScreen Instance { get; private set; }

    [SerializeField] Mode mode = Mode.Normal;
    [SerializeField] bool correct = false;
    [SerializeField] bool flipY = false;

    public Mode CurrentMode => mode;
    public bool Correct => correct;

    Material material;
    RenderTexture rt;

    const string ModeKey = "colorblind_mode";
    const string CorrectKey = "colorblind_correct";

    static readonly int ModeId = Shader.PropertyToID("_Mode");
    static readonly int CorrectId = Shader.PropertyToID("_Correct");
    static readonly int FlipId = Shader.PropertyToID("_Flip");

    void Awake()
    {
        // Só pode existir um, e ele persiste entre cenas
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Carrega a escolha salva
        mode = (Mode)PlayerPrefs.GetInt(ModeKey, (int)mode);
        correct = PlayerPrefs.GetInt(CorrectKey, correct ? 1 : 0) == 1;
    }

    public void SetMode(int m)
    {
        mode = (Mode)m;
        PlayerPrefs.SetInt(ModeKey, m);
        PlayerPrefs.Save();
    }

    public void SetCorrect(bool c)
    {
        correct = c;
        PlayerPrefs.SetInt(CorrectKey, c ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnEnable()
    {
        var shader = Resources.Load<Shader>("ColorblindScreen");
        if (shader == null)
        {
            Debug.LogError("Shader ColorblindScreen não encontrado em Assets/Resources.");
            enabled = false;
            return;
        }

        material = new Material(shader);
        StartCoroutine(Loop());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        if (rt != null) { rt.Release(); Destroy(rt); rt = null; }
        if (material != null) { Destroy(material); material = null; }
    }

    IEnumerator Loop()
    {
        var wait = new WaitForEndOfFrame();
        while (true)
        {
            yield return wait;
            if (mode == Mode.Normal) continue;   // Normal = não faz nada

            if (rt == null || rt.width != Screen.width || rt.height != Screen.height)
            {
                if (rt != null) { rt.Release(); Destroy(rt); }
                rt = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            }

            material.SetFloat(ModeId, (float)mode);
            material.SetFloat(CorrectId, correct ? 1f : 0f);
            material.SetFloat(FlipId, flipY ? 1f : 0f);

            ScreenCapture.CaptureScreenshotIntoRenderTexture(rt);
            Graphics.Blit(rt, (RenderTexture)null, material);
        }
    }
}