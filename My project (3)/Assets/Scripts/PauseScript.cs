using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject tela;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            tela.SetActive(true);
        }
    }


    public void ResumeButton()
    {
        tela.SetActive(false);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Sent to Menu");
    }

    public void ResetButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}