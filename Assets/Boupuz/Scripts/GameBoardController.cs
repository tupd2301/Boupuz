using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameBoardController : MonoBehaviour
{
    public static GameBoardController Instance { get; private set; }
    [SerializeField]
    private List<BrickController> _brickControllers = new List<BrickController>();
    public List<BrickController> BrickControllers { get { return _brickControllers; } }

    private List<BrickController> _removedBrick = new List<BrickController>();
    public List<BrickController> RemovedBrick { get { return _removedBrick; } }

    [SerializeField]
    private int _gridWidth, _gridHeight;
    [SerializeField]
    private GameObject _brickOri1;
    [SerializeField]
    private GameObject _brickOri2;

    [SerializeField]
    private int _gridScreenWidth, _gridScreenHeight;
    public int GridScreenWidth { get { return _gridScreenWidth;}}
    public int GridScreenHeight { get { return _gridScreenHeight;}}

    private BrickController[,] _grid;
    public BrickController[,] Grid { get {return _grid;}}

    public LevelData LevelData { get; private set; }
    public LevelInfo LevelInfo { get; private set; }

    public GameObject BrickOri1 { get => _brickOri1; set => _brickOri1 = value; }
    public GameObject BrickOri2 { get => _brickOri2; set => _brickOri2 = value; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        LevelData = GetComponentInChildren<LevelData>();

        //temporary
        //PlayerPrefs.SetInt("coins", 9999);
    }

    void Start()
    {
        int levelID = PlayerPrefs.GetInt("LevelID", 1);
        var resource = Resources.Load("/Levels/Level" + levelID.ToString());
        
        if (resource)
        {   
            var level = resource as GameObject;
            Instantiate(level, Vector3.zero, Quaternion.identity, transform);
        }
        else
        {
            Debug.Log("--------------Error loading level------------------");
        }
        LevelInfo = GetComponentInChildren<LevelInfo>();
        Debug.Log(LevelInfo.levelType);

        if (LevelInfo.levelType == LevelInfo.LevelType.Action)
        {
            

        }
        else if (LevelInfo.levelType == LevelInfo.LevelType.Puzzle)
        {
            
            //
            LevelData.TotalTurn = LevelInfo.LevelTurn;
            LevelData.CurrentTurn = LevelData.TotalTurn;
        }
        UIManager.Instance.SetUpTopUI();

        _brickControllers = GetComponentsInChildren<BrickController>().ToList<BrickController>();
        InitGrid();
        
    }

    /* 4x4 GRID
    [(0,0) (1,0) (2,0) (3,0)
     (0,1) (1,1) (2,1) (3,1)
     (0,2) (1,2) (2,2) (3,2)
     (0,3) (1,3) (2,3) (3,3)]
    */

    public void InitGrid()
    {
        Debug.Log("Number of brick: " + _brickControllers.Count.ToString());
        LevelData.GetTotalBricks(_brickControllers.Where(brick=>brick.CompareTag("Block")).Count());
        LevelData.GetTotalCakes(_brickControllers.Where(brick=>brick.Data.Id == 6 && brick.Data.Type == ObjectType.Brickie).Count());
        _grid = new BrickController[_gridWidth, _gridHeight];
        for (int brickIndex = 0; brickIndex < _brickControllers.Count; brickIndex++)
        {
            BrickController newBrick = _brickControllers[brickIndex];
            if (newBrick?.Data.BrickCoordinate.X < _gridWidth && newBrick?.Data.BrickCoordinate.Y < _gridHeight)
            {
                if (_grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] == null)
                {
                    _grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] = newBrick;
                    newBrick.Initialize();
                } 
                else
                {
                    Debug.Log(newBrick.gameObject.name);
                    Debug.Log("WARNING: DUPLICATE COORDINATE. PLEASE DOULBE CHECK");
                }
            }
        }
        
    }

    public void UpdateGrid()
    {

        for (int i = 0; i < _removedBrick.Count; i++)
        {
            _brickControllers.Remove(_removedBrick[i]);
        }
        _removedBrick = new List<BrickController>();

        _grid = new BrickController[_gridWidth, _gridHeight];
        for (int brickIndex = 0; brickIndex < _brickControllers.Count; brickIndex++)
        {
            BrickController newBrick = _brickControllers[brickIndex];
            if (newBrick?.Data.BrickCoordinate.X < _gridWidth && newBrick?.Data.BrickCoordinate.Y < _gridHeight)
            {
                if (_grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] == null)
                {
                    _grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] = newBrick;
                    //newBrick.Initialize();
                } 
                else
                {
                    Debug.Log(newBrick.gameObject.name);
                    Debug.Log("WARNING: DUPLICATE COORDINATE. PLEASE DOULBE CHECK");
                }
            }
        }
    }

    public void UpdateBrickCoordinateBySpeed(BrickController brick)
    {
        // update coordinate
        brick.Data.BrickCoordinate.X += (int)(brick.Data.Direction[0] * brick.Data.Speed);
        brick.Data.BrickCoordinate.Y += (int)(brick.Data.Direction[1] * brick.Data.Speed);
    }

    // public void UpdateBrickCoordinateByValue(BrickController brick, GridCoordinate newCoordinate)
    // {
    //     // update coordinate
    //     brick.Data.BrickCoordinate = newCoordinate;
        
    // }

    public bool CheckLose(BrickController brick)
    {
        
        if((brick.Data.BrickCoordinate.Y<brick.Data.Speed || brick.Data.BrickCoordinate.Y<1)&&brick.gameObject.CompareTag("Block"))
        {
            Debug.Log("Lose");
            GameFlow.Instance.canShoot = false;
            UIManager.Instance.LoadLoseUI();
            return true;
        }
        
        return false;
    }

    public void MoveAll()
    {
        StopAllCoroutines();
        List<BrickController> movableObjects = new List<BrickController>();
        for (int i = 0; i < _brickControllers.Count; i++)
        {
            if (_brickControllers[i].Data.Type == ObjectType.Item)
            {
                if (_brickControllers[i].Data.Id == 2 || _brickControllers[i].Data.Id == 3) // if laser
                {
                    if (_brickControllers[i].gameObject.GetComponent<LaserItem>().isTouched)
                    {
                        _brickControllers[i].gameObject.SetActive(false);
                        _removedBrick.Add(_brickControllers[i]);
                    }
                }

            }
            if (_brickControllers[i].Data.movable && _brickControllers[i].gameObject.activeInHierarchy)
            {
                if (!_brickControllers[i].Data.isFreeze && !CheckBlockingObject(_brickControllers[i]))
                {
                    movableObjects.Add(_brickControllers[i]);
                    //StartCoroutine(_brickControllers[i].Move(1));
                    if(!CheckLose(_brickControllers[i])){
                        UpdateBrickCoordinateBySpeed(_brickControllers[i]);
                        CheckLose(_brickControllers[i]);
                    }
                    else
                    {
                        //SceneManager.LoadScene("GameplayTest 2");
                    }
                }
                else if (_brickControllers[i].Data.isFreeze)
                {
                    _brickControllers[i].Data.LvFreeze -= 1;
                    if (_brickControllers[i].Data.LvFreeze == 0)
                    {
                        _brickControllers[i].Data.isFreeze = false;
                        _brickControllers[i].View.DisableChildGraphic();
                        movableObjects.Add(_brickControllers[i]);
                        if(!CheckLose(_brickControllers[i]))
                        {
                            UpdateBrickCoordinateBySpeed(_brickControllers[i]);
                            CheckLose(_brickControllers[i]);
                        }
                    }
                }
            }
            
        }
        UpdateGrid();

        for (int i = 0; i < movableObjects.Count; i++)
        {
            StartCoroutine(movableObjects[i].Move(1));
        }
    }

    public bool CheckBlockingObject(BrickController brick) //return true if is blocked
    {
        GridCoordinate nextCoordinate = brick.Data.BrickCoordinate + brick.Data.Direction;
        //Debug.Log(nextCoordinate);
        try{
            if (_grid[nextCoordinate.X,nextCoordinate.Y] != null)
            {
                if (_grid[nextCoordinate.X,nextCoordinate.Y].Data.isFreeze || _grid[nextCoordinate.X,nextCoordinate.Y].Data.isBlocked)
                {
                    brick.Data.isBlocked = true;
                    return true;
                }
            }
           
            brick.Data.isBlocked = false;
        
            return false;
        }
        catch
        {
            return false;
        }
        
        
    }

    public void UpdateDestroyedBricks()
    {
        LevelData.UpdateDestroyedBricks();
        UIManager.Instance.UpdateDestroyedBricksUI();

        if (LevelData.totalBricks == LevelData.destroyedBricks)
        {
            //Win
            GameFlow.Instance.canShoot = false;
            UIManager.Instance.LoadWinUI();
        }
    }
    public void UpdateTurn()
    {
        LevelData.DecreaseTurn(1);
        UIManager.Instance.UpdateTurnUI();
    }

    public void UpdateCandy(int value)
    {
        LevelData.AddCandies(value);
        UIManager.Instance.UpdateCandyUI();
    }

    public void UpdateCollectedCake()
    {
        LevelData.UpdateCollectedCake();
        UIManager.Instance.UpdateCakeUI();
        if (LevelData.CollectedCake == LevelData.TotalCake)
        {
            //Win
            GameFlow.Instance.canShoot = false;
            UIManager.Instance.LoadWinUI();
        }
    }
}
