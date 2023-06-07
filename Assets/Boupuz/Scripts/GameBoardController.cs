using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    
    [SerializeField]
    private List<BrickController> _brickControllers = new List<BrickController>();
    public List<BrickController> BrickControllers { get { return _brickControllers; } }

    [SerializeField]
    private int _gridWidth, _gridHeight;

    private BrickController[,] Grid;

    void Start()
    {
        InitGrid();
        //MoveAll();
    }

    public void InitGrid()
    {
        Debug.Log("Number of brick: " + _brickControllers.Count.ToString());
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

    public void MoveAll()
    {
        StopAllCoroutines();
        for (int i = 0; i < _brickControllers.Count; i++)
        {
            // TODO: if not block, move
            StartCoroutine(_brickControllers[i].Move(1));
        }
    }

    public void BallBrickCollision(Collision2D col)
    {
        
    }

    
}
