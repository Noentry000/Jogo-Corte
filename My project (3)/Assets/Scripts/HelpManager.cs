using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private Image panelImage;
    [SerializeField] private Sprite[] pages;

    [Header("Buttons")]
    [SerializeField] private Button openButton;   // the always-onscreen help button
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;

    private int currentPage = 0;

    private void Start()
    {
        openButton.onClick.AddListener(OpenHelp);
        nextButton.onClick.AddListener(NextPage);
        backButton.onClick.AddListener(PreviousPage);
        exitButton.onClick.AddListener(CloseHelp);

        CloseHelp();
    }

    public void OpenHelp()
    {
        currentPage = 0;
        helpPanel.SetActive(true);
        openButton.gameObject.SetActive(false);
        ShowPage();
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);
        openButton.gameObject.SetActive(true);
        currentPage = 0;
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
    }

    private void ShowPage()
    {
        panelImage.sprite = pages[currentPage];

        // Back is only visible once the player has moved past the first page
        backButton.gameObject.SetActive(currentPage > 0);
    }
}