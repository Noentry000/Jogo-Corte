using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class ColorblindScreen : MonoBehaviour
{
    public enum Mode { Normal = 0, Protanopia = 1, Deuteranopia = 2, Tritanopia = 3 }

    [SerializeField] Mode mode = Mode.Normal;
    [Tooltip("Desligado: simula o daltonismo. Ligado: corrige as cores para ajudar quem tem a condição.")]
    [SerializeField] bool correct = false;
    [Tooltip("Marque se a imagem aparecer de cabeça para baixo")]
    [SerializeField] bool flipY = false;

    Material material;
    RenderTexture rt;

    static readonly int ModeId = Shader.PropertyToID("_Mode");
    static readonly int CorrectId = Shader.PropertyToID("_Correct");
    static readonly int FlipId = Shader.PropertyToID("_Flip");

    // Use em Dropdown e Toggle da UI
    public void SetMode(int m) { mode = (Mode)m; }
    public void SetCorrect(bool c) { correct = c; }

    void OnEnable()
    {
        var shader = Resources.Load<Shader>("ColorblindScreen");
        if (shader == null)
        {
            Debug.LogError("Shader ColorblindScreen não encontrado em Assets/Resources. " +
                           "Selecione o script no Editor para ele ser gerado.");
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

#if UNITY_EDITOR
    // Gera o arquivo do shader automaticamente
    void Reset() => ColorblindShaderGenerator.Ensure();
    void OnValidate() => ColorblindShaderGenerator.Ensure();
#endif
}

#if UNITY_EDITOR
[InitializeOnLoad]
static class ColorblindShaderGenerator
{
    const string Folder = "Assets/Resources";
    const string Path_ = "Assets/Resources/ColorblindScreen.shader";

    static ColorblindShaderGenerator() => EditorApplication.delayCall += Ensure;

    public static void Ensure()
    {
        if (File.Exists(Path_)) return;

        Directory.CreateDirectory(Folder);
        File.WriteAllText(Path_, ShaderSource);
        AssetDatabase.ImportAsset(Path_);
        Debug.Log("ColorblindScreen: shader gerado em " + Path_);
    }

    const string ShaderSource = @"
Shader ""Custom/ColorblindScreen""
{
    Properties
    {
        _MainTex (""Texture"", 2D) = ""white"" {}
        _Mode (""Mode"", Float) = 0
        _Correct (""Correct"", Float) = 0
        _Flip (""Flip Y"", Float) = 0
    }
    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include ""UnityCG.cginc""

            sampler2D _MainTex;
            float _Mode, _Correct, _Flip;

            static const float3x3 Protan = float3x3(
                 0.152286, 1.052583, -0.204868,
                 0.114503, 0.786281,  0.099216,
                -0.003882, -0.048116, 1.051998);
            static const float3x3 Deutan = float3x3(
                 0.367322, 0.860646, -0.227968,
                 0.280085, 0.672501,  0.047413,
                -0.011820, 0.042940,  0.968881);
            static const float3x3 Tritan = float3x3(
                 1.255528, -0.076749, -0.178779,
                -0.078411,  0.930809,  0.147602,
                 0.004733,  0.691367,  0.303900);

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                if (_Flip > 0.5) uv.y = 1 - uv.y;

                float3 c = tex2D(_MainTex, uv).rgb;
                int m = (int)round(_Mode);
                if (m == 0) return fixed4(c, 1);

                float3x3 M = (m == 1) ? Protan : (m == 2) ? Deutan : Tritan;
                float3 sim = saturate(mul(M, c));
                if (_Correct < 0.5) return fixed4(sim, 1);

                float3 e = c - sim;
                float3 shift = (m == 3)
                    ? float3(0.7 * e.b + e.r, 0.7 * e.b + e.g, 0)
                    : float3(0, 0.7 * e.r + e.g, 0.7 * e.r + e.b);
                return fixed4(saturate(c + shift), 1);
            }
            ENDCG
        }
    }
}";
}
#endif