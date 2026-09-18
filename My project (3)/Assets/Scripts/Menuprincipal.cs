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
        SceneManager.LoadScene(0);
        Debug.Log("Abrindo");
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Menu");
    }
    
    public void FecharJogo()
    {
        Application.Quit();
        Debug.Log("Fechando");
    }

    


}