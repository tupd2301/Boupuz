using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private List<GameObject> _listUI;
    [SerializeField] private List<ButtonNavigator> _listNavigator;
    [SerializeField] private List<Character> _listCharacter;
    [SerializeField] private CharacterUI _characterUI;
    [SerializeField] private Text _totalBall;

    [Header("Top Bar UI")]
    [SerializeField] private Text _destroyedBricksText;
    [SerializeField] private Text _numCandyText;
    [SerializeField] private Text _numCoinText;
    [SerializeField] private Image _coinImage;
    [SerializeField] private Image _candyImage;
    [SerializeField] private Text _turnText;
    [SerializeField] private Text _cakeText;
    [SerializeField] private Image _cakeImage;
    [SerializeField] private Image _turnImage;
    [SerializeField] private Image _muteBGMHome;
    [SerializeField] private Image _playButton;
    [SerializeField] private Sprite _homeSprite;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private Text _plusOne;

    [SerializeField] private Image _optionIcon;
    [SerializeField] private Sprite _pauseImage;
    [SerializeField] private Sprite _unpauseImage;

    [SerializeField] private Text _levelCheat;
    [SerializeField] private Text _levelText;
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;

    [SerializeField] private PauseGame _pauseUI;
    [SerializeField] private GameObject _tutorialUI;


    [Header("Praise")]
    [SerializeField] private Text _praiseText;

    private int _level = 1;
    private bool _level1TutorialBooster = false;

    public List<Character> ListCharacter { get => _listCharacter; set => _listCharacter = value; }

    private void Awake()
    {
        UIManager.Instance = this;
    }


    public void MinusLevel()
    {
        SoundManager.Instance.PlaySFX("ui");

        _level -= 1;
        if (_level < 1)
            _level = 1;
        _levelCheat.text = _level.ToString();
    }
    public void AddLevel()
    {
        SoundManager.Instance.PlaySFX("ui");

        _level += 1;
        //if (_level > 34)
        //    _level = 34;

        if (_level > PlayerPrefs.GetInt("LevelMax1"))
            _level = PlayerPrefs.GetInt("LevelMax1");

        _levelCheat.text = _level.ToString();
    }

    public void Ready()
    {
        PlayerPrefs.SetInt("LevelID", _level);
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }

    public void LoadUI(string name)
    {
        UnpauseGame();
        for (int i = 0; i < _listUI.Count; i++)
        {
            if (_listNavigator[i].Parent.name != name)
            {
                Debug.Log("ok");
                _listNavigator[i].GetComponent<Image>().color = new Color32(160, 160, 160, 255);
                _listNavigator[i].Parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(_listNavigator[i].Parent.GetComponent<RectTransform>().anchoredPosition.x, 0);
            }
            else
            {
                _listNavigator[i].GetComponent<Image>().color = Color.white;
                _listNavigator[i].Parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(_listNavigator[i].Parent.GetComponent<RectTransform>().anchoredPosition.x, 55f);
            }
            if(_listNavigator[i].Parent.name == "Play")
            {
                if(_listNavigator[i].GetComponent<Image>().color == Color.white)
                {
                    _playButton.sprite = _playSprite;
                    _playButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(5f, 35f);
                }
                else
                {
                    _playButton.sprite = _homeSprite;
                    _playButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 35f);
                }
            }
            if (_listUI[i].name != name + "Layout")
            {
                _listUI[i].transform.localPosition = new Vector3(1200, 0, 0);
            }
            else
            {
                _listUI[i].transform.localPosition = Vector3.zero;
                if (name == "Character")
                {
                    _characterUI.ShowUI(0);
                }
            }
        }
    }

    public void LoadWinUI()
    {
        _winUI.SetActive(true);
        PlayerPrefs.SetInt("LevelMax1", PlayerPrefs.GetInt("LevelID") + 1);
        PlayerPrefs.SetInt("LevelID", PlayerPrefs.GetInt("LevelID")+1);
    }

    public void LoadLoseUI()
    {
        _loseUI.SetActive(true);
    }

    public void OnSceneHome()
    {
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }

    public void UpdateTotalBall(int totalBall)
    {
        _totalBall.text = "x " + totalBall;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Home")
        {
            UpdateDestroyedBricksUI();
            //UpdateCoinUI();
            //UpdateCakeUI();
            //UpdateTurnUI();
            BrickController.OnBrickieRemoval += DisplayPlusOne;
        }
        else
        {
            LoadUI("Play");
            if (PlayerPrefs.HasKey("LevelID"))
            {
                _level = PlayerPrefs.GetInt("LevelID");
            }
            if (!PlayerPrefs.HasKey("LevelMax1"))
            {
                PlayerPrefs.SetInt("LevelMax1", 1);
                PlayerPrefs.SetInt("LevelID", 1);
            }
            else
            {
                _level = PlayerPrefs.GetInt("LevelMax1");
            }
            //PlayerPrefs.DeleteKey("LevelMax1");
            _levelCheat.text = _level.ToString();
        }

    }

    public void UpdateDestroyedBricksUI()
    {
        if (GameBoardController.Instance != null)
        {
            string totalBricks = GameBoardController.Instance.LevelData.totalBricks.ToString();
            string destroyedBricks = GameBoardController.Instance.LevelData.DestroyedBricks.ToString();
            int destroyBricks = GameBoardController.Instance.LevelData.totalBricks - GameBoardController.Instance.BrickControllers.Where(brick => brick.CompareTag("Block") && brick.gameObject.activeInHierarchy).Count();
            _destroyedBricksText.text = destroyBricks + "/" + totalBricks;
        }
        else
        {
            Debug.Log("GameBoardController is null");
        }
    }

    public void UpdateCakeUI()
    {
        if (GameBoardController.Instance != null)
        {
            string totalCakes = GameBoardController.Instance.LevelData.TotalCake.ToString();
            string collectedCake = GameBoardController.Instance.LevelData.CollectedCake.ToString();
            _cakeText.text = collectedCake + "/" + totalCakes;
        }
        else
        {
            Debug.Log("GameBoardController is null");
        }
    }

    public void UpdateTurnUI()
    {
        _turnText.text = GameBoardController.Instance.LevelData.CurrentTurn.ToString();
    }

    public void UpdateCandyUI()
    {
        _numCandyText.text = GameBoardController.Instance.LevelData.numCandies.ToString();
    }

    public void UpdateCoinUI()
    {
        _numCoinText.text = GameBoardController.Instance.LevelData.numCoins.ToString();
    }

    public void HideTutorialUI()
    {
        if (PlayerPrefs.GetInt("LevelID") == 1 && !_level1TutorialBooster)
        {
            _tutorialUI.GetComponentInChildren<Animator>().Play(PlayerPrefs.GetInt("LevelID").ToString() + "-B");
            _level1TutorialBooster = true;
        }
        else
        {
            _tutorialUI.SetActive(false);
            GameFlow.Instance.canShoot = true;
            SoundManager.Instance.PlaySFX("ui");
        }
    }

    public void SetUpTopUI()
    {
        _coinImage.enabled = true;
        _numCoinText.enabled = true;
        _levelText.enabled = true;
        _levelText.text = "Lv. " + PlayerPrefs.GetInt("LevelID");
        if (GameBoardController.Instance.LevelInfo == null)
        {
            _levelText.text = "Lv. Test";
            _destroyedBricksText.enabled = true;
        }
        else
        {
            if (GameBoardController.Instance.LevelInfo.HaveTutorial)
            {
                _tutorialUI.SetActive(true);
                Invoke("PlayTutorial",0.2f);
            }
            //_numCandyText.enabled = true;
            _destroyedBricksText.enabled = true;
            // if (GameBoardController.Instance.LevelInfo.levelType == LevelInfo.LevelType.Action)
            // {
            //     _numCandyText.enabled = true;
            //     _destroyedBricksText.enabled = true;

            //     _candyImage.enabled = true;
            // }
            // else if (GameBoardController.Instance.LevelInfo.levelType == LevelInfo.LevelType.Puzzle)
            // {
            //     _turnText.enabled = true;
            //     _turnImage.enabled = true;
            //     _cakeText.enabled = true;
            //     _cakeImage.enabled = true;
            // }
        }
    }

    void PlayTutorial()
    {
        GameFlow.Instance.canShoot = false;
        _tutorialUI.GetComponentInChildren<Animator>().Play(PlayerPrefs.GetInt("LevelID").ToString());
    }

    public void PauseGame()
    {
        SoundManager.Instance.PlaySFX("ui");

        ShowPauseUI();
        if (Time.timeScale == 0)
        {
            UnpauseGame();
            return;
        }
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        SoundManager.Instance.PlaySFX("ui");

        Time.timeScale = 1;
    }

    public void ShowPauseUI()
    {
        if (!_pauseUI.IsPause)
        {
            _pauseUI.IsPause = true;
            _pauseUI.Background.color = new Color32(0, 0, 0, 110);
            GameFlow.Instance.canShoot = false;
            for (int i = 0; i < _pauseUI.ListOption.Count; i++)
            {
                _pauseUI.ListOption[i].transform.localPosition = new Vector3(0, 150 + i * 130, 0);
                //StartCoroutine(PauseUIMoving());
            }
            _optionIcon.sprite = _unpauseImage;
        }
        else
        {
            _pauseUI.IsPause = false;
            _pauseUI.Background.color = new Color32(0, 0, 0, 0);
            GameFlow.Instance.canShoot = true;
            for (int i = 0; i < _pauseUI.ListOption.Count; i++)
            {
                _pauseUI.ListOption[i].transform.localPosition = Vector3.zero;
            }
            _optionIcon.sprite = _pauseImage;

        }
    }

    IEnumerator PauseUIMoving()
    {
        if (!_pauseUI.IsPause)
        {
            for (int i = 0; i < _pauseUI.ListOption.Count; i++)
            {
                _pauseUI.ListOption[i].transform.localPosition = Vector3.MoveTowards(_pauseUI.ListOption[i].transform.localPosition, new Vector3(0, 150 + i * 130, 0), 0.01f * 3);
            }
        }
        yield return 0;
    }

    public void DisplayPlusOne()
    {
        StartCoroutine(DisplayPlusOneCoroutine());
    }

    IEnumerator DisplayPlusOneCoroutine()
    {
        _plusOne.enabled = false;
        _plusOne.enabled = true;
        yield return new WaitForSeconds(0.3f);
        _plusOne.enabled = false;
    }

    void OnDestroy()
    {
        //Debug.Log("-------------Gameboardcontroller destroyed");
        BrickController.OnBrickieRemoval -= DisplayPlusOne;
    }
    public void UpdateUISFX(bool isMute)
    {
        if (SceneManager.GetActiveScene().name != "Home")
        {
            if (!isMute)
            {
                _pauseUI.ListOption[2].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                _pauseUI.ListOption[2].GetComponent<Image>().color = new Color32(110, 110, 110, 255);
            }
        }
    }
    public void UpdateUIBGM(bool isMute)
    {
        SoundManager.Instance.PlaySFX("ui");

        if (!isMute)
        {
            if (SceneManager.GetActiveScene().name != "Home")
                _pauseUI.ListOption[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            else
                _muteBGMHome.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "Home")
                _pauseUI.ListOption[1].GetComponent<Image>().color = new Color32(110, 110, 110, 255);
            else
                _muteBGMHome.color = new Color32(110, 110, 110, 255);
        }
    }


    public void UpdatePraiseText(int value)
    {
        //StopCoroutine(UpdatePraiseText(value-1));
        //_praiseText.gameObject.SetActive(false);
        _praiseText.gameObject.SetActive(true);
        switch (value)
        {
            case 1:
                _praiseText.text = "Cool";
                _praiseText.color = new Color32(255, 255, 255, 255);
                break;
            case 2:
                _praiseText.text = "Great";
                _praiseText.color = new Color32(199, 128, 232, 255);
                break;
            case 3:
                _praiseText.text = "Perfect";
                _praiseText.color = new Color32(157, 148, 255, 255);
                break;
            case 4:
                _praiseText.text = "Amazing";
                _praiseText.color = new Color32(89, 173, 246, 255);
                break;
            case 5:
                _praiseText.text = "Excellent";
                _praiseText.color = new Color32(8, 202, 209, 255);
                break;
            case 6:
                _praiseText.text = "Incredible";
                _praiseText.color = new Color32(66, 214, 164, 255);
                break;
            case 7:
                _praiseText.text = "Brilliant";
                _praiseText.color = new Color32(248, 243, 141, 255);
                break;
            case 8:
                _praiseText.text = "Fantastic";
                _praiseText.color = new Color32(255, 180, 128, 255);
                break;
            case 9:
                _praiseText.text = "Awesome";
                _praiseText.color = new Color32(255, 160, 0, 255);
                break;
            case 10:
                _praiseText.text = "Fabulous";
                _praiseText.color = new Color32(255, 105, 97, 255);
                break;
            case int n when (n > 10):
            _praiseText.color = new Color32(255, 105, 97, 255);
                _praiseText.text = "Magnificent x" + (n-10);
                break;
        }
        
    }

    public void DisablePraiseText()
    {
        _praiseText.gameObject.SetActive(false);
    }
}
