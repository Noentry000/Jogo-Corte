using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GerenciadorMenu3D : MonoBehaviour
{
    [Header("Configuração da Cena")]
    [Tooltip("Digite EXATAMENTE o nome da cena do seu jogo")]
    public string nomeDaCenaDoJogo;

    [Header("Botões do Menu")]
    [Tooltip("Arraste o objeto 'Inicio' (JOGAR)")]
    public GameObject botaoJogar;

    [Tooltip("Arraste o objeto 'sair' (SAIR)")]
    public GameObject botaoSair;

    void Start()
    {
        // Vincula as ações de clique diretamente aos botões
        AdicionarClique(botaoJogar, IniciarJogo);
        AdicionarClique(botaoSair, SairDoJogo);
    }

    public void IniciarJogo()
    {
        if (!string.IsNullOrEmpty(nomeDaCenaDoJogo))
        {
            SceneManager.LoadScene(nomeDaCenaDoJogo);
        }
        else
        {
            Debug.LogError("O nome da cena do jogo não foi preenchido no Inspector!");
        }
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");

#if UNITY_EDITOR
        // Para a execução do teste direto no Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Fecha a aplicação no jogo compilado (.exe / APK)
        Application.Quit();
#endif
    }

    private void AdicionarClique(GameObject obj, UnityEngine.Events.UnityAction acao)
    {
        if (obj == null) return;

        // Garante que o componente Button existe e limpa ouvintes antigos
        Button btn = obj.GetComponent<Button>();
        if (btn == null) btn = obj.AddComponent<Button>();

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(acao);
    }
}