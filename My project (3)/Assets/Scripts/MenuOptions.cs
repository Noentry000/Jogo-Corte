using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Painel")]
    [SerializeField] GameObject optionsPanel;

    [Header("Botões de abrir e fechar")]
    [SerializeField] Button openButton;   // botão OPÇÕES (menu principal)
    [SerializeField] Button backButton;   // botão VOLTAR (dentro do painel)

    [Header("Modos (nesta ordem: Normal, Protanopia, Deuteranopia, Tritanopia)")]
    [SerializeField] Button[] modeButtons;

    [Header("Correção")]
    [SerializeField] Toggle correctToggle;

    [Header("Destaque do botão selecionado")]
    [SerializeField] Color selectedColor = new Color(1f, 0.85f, 0.3f);
    [SerializeField] Color normalColor = Color.white;

    void Start()
    {
        if (openButton != null) openButton.onClick.AddListener(OpenOptions);
        if (backButton != null) backButton.onClick.AddListener(CloseOptions);

        for (int i = 0; i < modeButtons.Length; i++)
        {
            int index = i;
            modeButtons[i].onClick.AddListener(() => SelectMode(index));
        }

        if (correctToggle != null)
            correctToggle.onValueChanged.AddListener(OnCorrectChanged);

        optionsPanel.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        RefreshUI();
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    void SelectMode(int index)
    {
        if (ColorblindScreen.Instance == null) return;
        ColorblindScreen.Instance.SetMode(index);
        RefreshUI();
    }

    void OnCorrectChanged(bool value)
    {
        if (ColorblindScreen.Instance == null) return;
        ColorblindScreen.Instance.SetCorrect(value);
    }

    void RefreshUI()
    {
        var cb = ColorblindScreen.Instance;
        if (cb == null) return;

        for (int i = 0; i < modeButtons.Length; i++)
        {
            var g = modeButtons[i].targetGraphic;
            if (g != null) g.color = (i == (int)cb.CurrentMode) ? selectedColor : normalColor;
        }

        if (correctToggle != null)
            correctToggle.SetIsOnWithoutNotify(cb.Correct);
    }
}