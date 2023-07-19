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
    //public List<BrickController> BrickControllers { get { return _brickControllers; } }

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

    [SerializeField] private List<Wall> _listWall;

    public GameObject BrickOri1 { get => _brickOri1; set => _brickOri1 = value; }
    public GameObject BrickOri2 { get => _brickOri2; set => _brickOri2 = value; }
    public List<BrickController> BrickControllers { get => _brickControllers; set => _brickControllers = value; }
    [SerializeField] private bool _playTest;
    [SerializeField] private bool _enableItemSpawner;
    public GameObject deathAnim;


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

    void OnEnable()
    {
        BrickController.OnBrickieRemoval += UpdateDestroyedBricks;
    }

    void Start()
    {
        if (!_playTest)
        {
            LoadLevelPrefab();
        }
        //Debug.Log(LevelInfo.levelType);
        

        UIManager.Instance.SetUpTopUI();

        _brickControllers = GetComponentsInChildren<BrickController>().ToList<BrickController>();
        _brickControllers = _brickControllers.OrderBy(b => b.Data.BrickCoordinate.Y).ToList();

        InitGrid();
        SpawnItems();
    }

    private List<BrickController> LoadItemPrefab(string path)
    {
        List<BrickController> items = new List<BrickController>();
        for (int i = 0; i < LevelInfo.NumAddItem; i ++)
        {
            var resource = Resources.Load(path);
            if (resource)
            {
                var itemPrefab = resource as GameObject;
                //Debug.Log(itemPrefab);
                BrickController item = Instantiate(itemPrefab.GetComponent<BrickController>(), Vector3.zero, Quaternion.identity, transform);
                items.Add(item);
            }
            else
            {
                Debug.Log("------------error loading item prefab");
            }
        }
        return items;
    }

    private void SpawnItemRandomly(List<BrickController> items, List<GridCoordinate> emptyCoord)
    {
        var random = new System.Random();
        int index = random.Next(emptyCoord.Count);
        BrickController item = items[0];
        item.Data.BrickCoordinate = emptyCoord[index];
        items.RemoveAt(0);
        _grid[emptyCoord[index].X, emptyCoord[index].Y] = item;
        _brickControllers.Add(item);
        item.Initialize();
        emptyCoord.Remove(emptyCoord[index]);
}

    private void SpawnItems()
    {
        if (_enableItemSpawner)
        {
            int Ymin = _brickControllers[0].Data.BrickCoordinate.Y;
            int Ymax = _brickControllers[_brickControllers.Count-1].Data.BrickCoordinate.Y;

            List<BrickController> addItems = LoadItemPrefab("Prefabs/Item/ItemAdd");
            List<BrickController> damageItems = LoadItemPrefab("Prefabs/Item/GainDamage");
            

            for (int Y = Ymin; Y <= Ymax; Y++)
            {
                List<GridCoordinate> emptyCoord = new List<GridCoordinate>();
                for (int X = 0; X < _gridWidth; X++)
                {
                    if (_grid[X, Y] == null)
                    {
                        emptyCoord.Add(new GridCoordinate(X,Y));
                    }
                }
                if (emptyCoord.Count > 0)
                {
                    if (Y % 2 == 0 && addItems.Count > 0)
                    {
                        SpawnItemRandomly(addItems, emptyCoord);
                    }
                    else if (Y % 5 == 0 && damageItems.Count > 0)
                    {
                        SpawnItemRandomly(damageItems, emptyCoord);
                    }
                }
            }
            _brickControllers = _brickControllers.OrderBy(b => b.Data.BrickCoordinate.Y).ToList();
        }
    }


    private void LoadLevelPrefab()
    {
        int levelID = PlayerPrefs.GetInt("LevelID", 1);
        var resource = Resources.Load("Levels/Level" + levelID.ToString());

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
        //Debug.Log(LevelInfo.levelType);

        // if (LevelInfo.levelType == LevelInfo.LevelType.Action)
        // {


        // }
        // else if (LevelInfo.levelType == LevelInfo.LevelType.Puzzle)
        // {

        //     //
        //     LevelData.TotalTurn = LevelInfo.LevelTurn;
        //     LevelData.CurrentTurn = LevelData.TotalTurn;
        // }
        UIManager.Instance.SetUpTopUI();

        _brickControllers = GetComponentsInChildren<BrickController>().ToList<BrickController>();
        _brickControllers = _brickControllers.OrderBy(b => b.Data.BrickCoordinate.Y).ToList();
        InitGrid();

    }


    public void InitGrid()
    {
        Debug.Log("Number of brick: " + BrickControllers.Count.ToString());
        LevelData.GetTotalBricks(BrickControllers.Where(brick=>brick.CompareTag("Block")).Count());
        LevelData.GetTotalCakes(BrickControllers.Where(brick=>brick.Data.Id == 6 && brick.Data.Type == ObjectType.Brickie).Count());
        _grid = new BrickController[_gridWidth, _gridHeight];
        for (int brickIndex = 0; brickIndex < BrickControllers.Count; brickIndex++)
        {
            BrickController newBrick = BrickControllers[brickIndex];
            if (newBrick?.Data.BrickCoordinate.X < _gridWidth && newBrick?.Data.BrickCoordinate.Y < _gridHeight)
            {
                if (_grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] == null)
                {
                    _grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] = newBrick;
                    newBrick.Initialize();
                } 
                else
                {
                    Debug.LogError("WARNING: DUPLICATE COORDINATE. PLEASE DOULBE CHECK: " + newBrick.gameObject.name + " " + _grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y].name);

                }
            }
        }
        ScaleWall(BrickControllers[0].Ratio);
        
    }

    public void ScaleWall(float ratio)
    {
        for (int i = 0; i < _listWall.Count; i++)
        {
            _listWall[i].Scale(ratio);
        }
    }

    public void UpdateGrid()
    {

        for (int i = 0; i < _removedBrick.Count; i++)
        {
            BrickControllers.Remove(_removedBrick[i]);
        }
        _removedBrick = new List<BrickController>();
        _brickControllers = _brickControllers.OrderBy(b => b.Data.BrickCoordinate.Y).ToList();
        _grid = new BrickController[_gridWidth, _gridHeight];
        for (int brickIndex = 0; brickIndex < BrickControllers.Count; brickIndex++)
        {
            BrickController newBrick = BrickControllers[brickIndex];
            if (newBrick?.Data.BrickCoordinate.X < _gridWidth && newBrick?.Data.BrickCoordinate.Y < _gridHeight)
            {
                if (_grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] == null ||
                    !newBrick.gameObject.CompareTag("MergeMachine"))
                {
                    _grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] = newBrick;
                    //newBrick.Initialize();
                    if (newBrick.gameObject.CompareTag("MergeMachine"))
                    {
                        if (newBrick.GetComponent<MergeMachine>().HeldBrick)
                        {
                            newBrick.GetComponent<MergeMachine>().HeldBrick = null;
                        }
                    }
                } 
                else
                {
                    Debug.Log("WARNING: DUPLICATE COORDINATE. PLEASE DOULBE CHECK: " + newBrick.gameObject.name + " " + _grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y].name);
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
        UpdateGrid();
        List<BrickController> movableObjects = new List<BrickController>();
        for (int i = 0; i < BrickControllers.Count; i++)
        {
            if (BrickControllers[i].Data.Type == ObjectType.Item)
            {
                if (BrickControllers[i].Data.Id == 2 || BrickControllers[i].Data.Id == 3) // if laser
                {
                    if (BrickControllers[i].gameObject.GetComponent<LaserItem>().isTouched)
                    {
                        BrickControllers[i].gameObject.SetActive(false);
                        _removedBrick.Add(BrickControllers[i]);
                    }
                }

            }
            if (BrickControllers[i].Data.movable && BrickControllers[i].gameObject.activeInHierarchy)
            {
                if (!BrickControllers[i].Data.isFreeze && !CheckBlockingObject(BrickControllers[i]))
                {
                    movableObjects.Add(BrickControllers[i]);
                    //StartCoroutine(_brickControllers[i].Move(1));
                    if(!CheckLose(BrickControllers[i])){
                        UpdateBrickCoordinateBySpeed(BrickControllers[i]);
                        CheckLose(BrickControllers[i]);
                    }
                    else
                    {
                        //SceneManager.LoadScene("GameplayTest 2");
                    }
                }
                else if (BrickControllers[i].Data.isFreeze)
                {
                    BrickControllers[i].Data.LvFreeze -= 1;
                    if (BrickControllers[i].Data.LvFreeze == 0)
                    {
                        BrickControllers[i].Data.isFreeze = false;
                        BrickControllers[i].View.DisableChildGraphic();
                        movableObjects.Add(BrickControllers[i]);
                        if(!CheckLose(BrickControllers[i]))
                        {
                            UpdateBrickCoordinateBySpeed(BrickControllers[i]);
                            CheckLose(BrickControllers[i]);
                        }
                    }
                }
            }
            
        }
        //UpdateGrid();

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
        int destroyBricks = LevelData.totalBricks - BrickControllers.Where(brick => brick.CompareTag("Block") && brick.gameObject.activeInHierarchy).Count();
        
        if (LevelData.totalBricks == /*LevelData.DestroyedBricks*/ destroyBricks)
        {
            //Win
            GameFlow.Instance.canShoot = false;
            UIManager.Instance.LoadWinUI();
        }
    }
    // public void UpdateTurn()
    // {
    //     LevelData.DecreaseTurn(1);
    //     UIManager.Instance.UpdateTurnUI();
    // }

    // public void UpdateCandy(int value)
    // {
    //     LevelData.AddCandies(value);
    //     UIManager.Instance.UpdateCandyUI();
    // }

    // public void UpdateCollectedCake()
    // {
    //     LevelData.UpdateCollectedCake();
    //     UIManager.Instance.UpdateCakeUI();
    //     if (LevelData.CollectedCake == LevelData.TotalCake)
    //     {
    //         //Win
    //         GameFlow.Instance.canShoot = false;
    //         UIManager.Instance.LoadWinUI();
    //     }
    // }

    void OnDestroy()
    {
        Debug.Log("-------------Gameboardcontroller destroyed");
        BrickController.OnBrickieRemoval -= UpdateDestroyedBricks;
    }
}
