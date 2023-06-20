using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private Text _levelCheat;
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;

    private int _level = 1;

    public List<Character> ListCharacter { get => _listCharacter; set => _listCharacter = value; }

    private void Awake()
    {
        UIManager.Instance = this;
    }

    public void MinusLevel()
    {
        _level -= 1;
        if (_level < 1)
            _level = 1;
        _levelCheat.text = _level.ToString();
    }
    public void AddLevel()
    {
        _level += 1;
        if (_level > 99)
            _level = 99;
        _levelCheat.text = _level.ToString();
    }

    public void LoadUI(string name)
    {
        if(name == "Play")
        {
            PlayerPrefs.SetInt("LevelID", _level);
            SceneManager.LoadScene("GameplayTest 2", LoadSceneMode.Single);
            return;
        }
        for (int i = 0; i < _listUI.Count; i++)
        {
            if(_listNavigator[i].Parent.name != name)
            {
                Debug.Log("ok");
                _listNavigator[i].Parent.transform.localScale = Vector3.one;
            }
            if(_listUI[i].name != name + "Layout")
            {
                _listUI[i].transform.localPosition = new Vector3(1200,0,0);
            }
            else
            {
                _listUI[i].transform.localPosition = Vector3.zero;
                if(name == "Character")
                {
                    _characterUI.ShowUI(0);
                }
            }
        }
    }

    public void LoadWinUI()
    {
        _winUI.SetActive(true);
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
        if(SceneManager.GetActiveScene().name != "Home")
        {
            UpdateDestroyedBricksUI();
            UpdateCoinUI();
        }
        else
        {
            LoadUI("Home");
            _levelCheat.text = _level.ToString();
        }
    }

    public void UpdateDestroyedBricksUI()
    {
        if (GameBoardController.Instance != null)
        {
            string totalBricks = GameBoardController.Instance.LevelData.totalBricks.ToString();
            string destroyedBricks = GameBoardController.Instance.LevelData.destroyedBricks.ToString();
            _destroyedBricksText.text = destroyedBricks + "/" + totalBricks;
        }
        else
        {
            Debug.Log("GameBoardController is null");
        }
    }

    public void UpdateCandyUI()
    {
        _numCandyText.text = GameBoardController.Instance.LevelData.numCandies.ToString();
    }

    public void UpdateCoinUI()
    {
        _numCoinText.text = GameBoardController.Instance.LevelData.numCoins.ToString();
    }
}
