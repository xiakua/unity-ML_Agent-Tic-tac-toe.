using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _chessboard;

    [SerializeField] private TMPro.TMP_Dropdown _isFirstDropdown;
    public TMPro.TMP_Dropdown IsFirstDropdown => _isFirstDropdown;

    [SerializeField] private TMPro.TMP_Dropdown _difficultyDropdown;
    public TMPro.TMP_Dropdown DifficultyDropdown => _difficultyDropdown;

    [SerializeField] private TMPro.TMP_Text _gameOverText;
    private void Start()
    {
        ShowMainMenu();
    }
    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
        _gameOverMenu.SetActive(false);
        _chessboard.SetActive(false);
    }
    public void ShowGameOverMenu(string str)
    {
        _mainMenu.SetActive(false);
        _gameOverMenu.SetActive(true);
        //_chessboard.SetActive(false);
        _gameOverText.text = str;
    }
    public void ShowChessboard()
    {
        _mainMenu.SetActive(false);
        _gameOverMenu.SetActive(false);
        _chessboard.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
