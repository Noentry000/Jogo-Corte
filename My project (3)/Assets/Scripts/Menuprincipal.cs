using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GerenciadorMenu3D : MonoBehaviour
{
    

    void Start()
    {
     
    }

    public void ComecarJogo()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Abrindo");
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Menu");
    }

    public void ProximaFase()
    {
        int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;

        if (proximaCena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(proximaCena);
            Debug.Log("Proxima fase");
        }
        else
        {
            SceneManager.LoadScene(0);
            Debug.Log("Ultima fase, voltando ao menu");
        }
    }
    
    public void FecharJogo()
    {
        Application.Quit();
        Debug.Log("Fechando");
    }

    


}