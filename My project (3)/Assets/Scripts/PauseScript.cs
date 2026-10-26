using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject tela;
    public GameObject[] hideWhilePaused;

    private bool[] wasActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !tela.activeSelf)
        {
            wasActive = new bool[hideWhilePaused.Length];

            for (int i = 0; i < hideWhilePaused.Length; i++)
            {
                if (hideWhilePaused[i] != null)
                {
                    wasActive[i] = hideWhilePaused[i].activeSelf;
                    hideWhilePaused[i].SetActive(false);
                }
            }

            tela.SetActive(true);
        }
    }


    public void ResumeButton()
    {
        tela.SetActive(false);

        if (wasActive == null)
            return;

        for (int i = 0; i < hideWhilePaused.Length; i++)
        {
            if (hideWhilePaused[i] != null && wasActive[i])
            {
                hideWhilePaused[i].SetActive(true);
            }
        }
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