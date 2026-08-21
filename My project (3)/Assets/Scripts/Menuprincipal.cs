using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerenciar a troca de cenas

public class MenuController : MonoBehaviour
{
    // Coloque exatamente o mesmo nome da cena do seu jogo 3D
    [SerializeField] private string nomeDaCenaDoJogo = "Fase1";

    public void IniciarJogo()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }
}