using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    Button buttonComp;
    [SerializeField] ButtonType buttonType;

    void Awake()
    {
        buttonComp = GetComponent<Button>();
    }

    void Start()
    {
        switch(buttonType)
        {
            case ButtonType.TitleButton:
                buttonComp.onClick.AddListener(OpenTitleScreen);
                break;
            case ButtonType.StartGameButton:
                buttonComp.onClick.AddListener(StartGame);
                break;
            case ButtonType.CreditsButton:
                buttonComp.onClick.AddListener(OpenCredits);
                break;
            default:
                buttonComp.onClick.AddListener(OpenTitleScreen);
                break;
         }
    }

    public void OpenTitleScreen()
    {
        GameStateManager.Instance.OpenTitleScreen();
    }

    public void StartGame()
    {
        GameStateManager.Instance.StartGame();
    }

    public void OpenCredits()
    {
        GameStateManager.Instance.OpenTitleScreen();
    }

}

enum ButtonType
{
    TitleButton,
    StartGameButton,
    CreditsButton
}
