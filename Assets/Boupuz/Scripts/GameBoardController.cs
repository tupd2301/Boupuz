using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    public static GameBoardController Instance;
    [SerializeField]
    private List<BrickController> _brickControllers = new List<BrickController>();
    public List<BrickController> BrickControllers { get { return _brickControllers; } }

    [SerializeField]
    private int _gridWidth, _gridHeight;

    private BrickController[,] Grid;

    void Awake()
    {
        GameBoardController.Instance = this;
    }

    void Start()
    {
        InitGrid();
        //MoveAll();
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
        UpdateGrid();
        
    }

    public void UpdateGrid()
    {
        Grid = new BrickController[_gridWidth, _gridHeight];
        for (int brickIndex = 0; brickIndex < _brickControllers.Count; brickIndex++)
        {
            BrickController newBrick = _brickControllers[brickIndex];
            if (newBrick?.Data.BrickCoordinate.X < _gridWidth && newBrick?.Data.BrickCoordinate.Y < _gridHeight)
            {
                if (Grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] == null)
                {
                    Grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] = newBrick;
                } 
                else
                {
                    Debug.Log("WARNING: DUPLICATE COORDINATE. PLEASE DOULBE CHECK");
                }
            }
        }
    }

    public void UpdateBrickCoordinate(BrickController brick)
    {
        // update coordinate
        brick.Data.BrickCoordinate.X += (int)(brick.Data.Direction[0] * brick.Data.Speed);
        brick.Data.BrickCoordinate.Y += (int)(brick.Data.Direction[1] * brick.Data.Speed);
    }

    public void MoveAll()
    {
        StopAllCoroutines();
        for (int i = 0; i < _brickControllers.Count; i++)
        {
            // TODO: if not blocked or frozen, move
            if (_brickControllers[i].Data.movable)
            {
                if (!_brickControllers[i].Data.isFreeze)
                {
                    StartCoroutine(_brickControllers[i].Move(1));
                    UpdateBrickCoordinate(_brickControllers[i]);
                }
            }
        }
        UpdateGrid();
    }

    public bool CheckBlockingObject(BrickController brick) //return true if is blocked
    {
        GridCoordinate nextCoordinate = brick.Data.BrickCoordinate + brick.Data.Direction;
        if (Grid[nextCoordinate.X,nextCoordinate.Y] != null)
        {
            return Grid[nextCoordinate.X,nextCoordinate.Y].Data.isFreeze; //|| !Grid[nextCoordinate.X,nextCoordinate.Y].Data.movable;
        }
        return false;
        
    }

    
}
